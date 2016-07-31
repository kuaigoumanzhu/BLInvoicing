using BL.Framework.Orm;
using BL.Models;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BL.Service
{
    public class FNCBALANCEDETAILSService : DBContext
    {
        public IEnumerable<T_FNCBALANCEDETAILSModel> GetAllFNCBALANCEInfo(IDictionary paraDic)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += " and FID like '%'+@FID+'%'";// string.Format(" and FID like '%{0}%'", paraDic["FID"].ToString());
                dp.Add("@FID", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += " and FNAME like '%'+@FNAME+'%'";// string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
                dp.Add("@FNAME", paraDic["FNAME"].ToString());
            }
            if (paraDic.Contains("FPARENTID") && paraDic["FPARENTID"].ToString().Trim() != "")
            {
                whereStr += " and FPARENTID=@FPARENTID";// string.Format(" and FPARENTID=@FPARENTID", paraDic["FPARENTID"].ToString());
                dp.Add("@FPARENTID", paraDic["FPARENTID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                string sqlstr = "select * from T_FNCBALANCEDETAILS where ";
                var result = db.Query<T_FNCBALANCEDETAILSModel>(sqlstr+whereStr,dp);
                return result;
            }
        }

        public IEnumerable<Object> GetSaleDayBook(IDictionary paraDic)
        {
            string whereStr = " 1=1 ";

            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FINWAREHOUSEID") && paraDic["FINWAREHOUSEID"].ToString().Trim() != "")
            {
                whereStr += " and FINWAREHOUSEID=@FINWAREHOUSEID";// string.Format(" and FINWAREHOUSEID='{0}'", paraDic["FINWAREHOUSEID"].ToString());
                dp.Add("@FINWAREHOUSEID", paraDic["FINWAREHOUSEID"].ToString());
            }
            if (paraDic.Contains("Fdate") && paraDic["Fdate"].ToString().Trim() != "")
            {
                whereStr += " and datediff(day,Fdate,@Fdate)=0";// string.Format(" and datediff(day,Fdate,'{0}')=0", paraDic["Fdate"].ToString());
                dp.Add("@Fdate", paraDic["Fdate"].ToString());
            }
            string sqlstr = @"select FINWAREHOUSEID FWAREHOUSEID,SUM(FMARKETMONEY) FMARKETMONEY,0 FBACKTMONEY,0 FDIFFERMONEY from dbo.T_SALEDAYBOOK where " + whereStr + @"
group by FINWAREHOUSEID";

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<Object>(sqlstr, dp);
                return result;
            }

        }


        public T_FNCBALANCEDETAILSModel AddFNCBALANCEDETAILS(T_FNCBALANCEDETAILSModel model)
        {

            string sql = @"insert into  T_FNCBALANCEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FWAREHOUSEID, FMARKETMONEY, FBACKTMONEY, FDIFFERMONEY, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FPARENTID, @FWAREHOUSEID, @FMARKETMONEY, @FBACKTMONEY, @FDIFFERMONEY, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_FNCBALANCEDETAILSModel>("select * from T_FNCBALANCEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
                    res.closeCurrent = true;
                    res.message = "添加成功";
                    return res;
                }
                else
                {
                    model.closeCurrent = true;
                    model.statusCode = "300";
                    model.message = "添加失败";
                    return model;
                }
            }
        }
        public bool AddFNCBALANCEDETAILS(List<T_FNCBALANCEDETAILSModel> list)
        {

            string sql = "";
            var model = list[0];
            DynamicParameters dp = new DynamicParameters();
            for (int i = 0; i < list.Count(); i++)
            {
                sql += @"insert into  T_FNCBALANCEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FWAREHOUSEID, FMARKETMONEY, FBACKTMONEY, FDIFFERMONEY, FMEMO
) values(@FGUID" + i + ", @FCREATEID" + i + ", @FCREATETIME" + i + ", @FPARENTID" + i + ", @FWAREHOUSEID" + i + ", @FMARKETMONEY" + i + ", @FBACKTMONEY" + i + ", @FDIFFERMONEY" + i + ", @FMEMO" + i + ");";
                dp.Add("@FGUID" + i, Guid.NewGuid().ToString());
                dp.Add("@FCREATEID" + i, list[0].FCREATEID);
                dp.Add("@FCREATETIME" + i, list[0].FCREATETIME);
                dp.Add("@FPARENTID" + i, list[0].FPARENTID);
                dp.Add("@FWAREHOUSEID" + i, list[i].FWAREHOUSEID);
                dp.Add("@FMARKETMONEY" + i, list[i].FMARKETMONEY);
                dp.Add("@FBACKTMONEY" + i, list[i].FBACKTMONEY);
                dp.Add("@FDIFFERMONEY" + i, list[i].FDIFFERMONEY);
                dp.Add("@FMEMO" + i, list[i].FMEMO);

            }
            using (IDbConnection db = OpenConnection())
            {
                var trans = db.BeginTransaction();
                if (db.Execute(sql, dp, trans) > 0)
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.Rollback();
                    return false;
                }
            }
        }

        public T_FNCBALANCEDETAILSModel EditFNCBALANCEDETAILS(T_FNCBALANCEDETAILSModel model)
        {

            string sql = @"update  T_FNCBALANCEDETAILS set   FBACKTMONEY=@FBACKTMONEY, FDIFFERMONEY=@FDIFFERMONEY, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_FNCBALANCEDETAILSModel>("select * from T_FNCBALANCEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
                    res.closeCurrent = true;
                    res.message = "修改成功";
                    return res;
                }
                else
                {
                    model.closeCurrent = true;
                    model.statusCode = "300";
                    model.message = "修改失败";
                    return model;
                }
            }
        }
    }
}
