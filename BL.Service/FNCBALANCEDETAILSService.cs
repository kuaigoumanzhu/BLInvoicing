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
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FID like '%{0}%'", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
            }
            if (paraDic.Contains("FPARENTID") && paraDic["FPARENTID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FPARENTID='{0}'", paraDic["FPARENTID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                string sqlstr = "select * from T_FNCBALANCEDETAILS";
                var result = db.Query<T_FNCBALANCEDETAILSModel>(sqlstr);
                return result;
            }
        }

        public IEnumerable<Object> GetSaleDayBook(IDictionary paraDic)
        {
            string whereStr = " 1=1 ";
            if (paraDic.Contains("FINWAREHOUSEID") && paraDic["FINWAREHOUSEID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FINWAREHOUSEID='{0}'", paraDic["FINWAREHOUSEID"].ToString());
            }
            if (paraDic.Contains("Fdate") && paraDic["Fdate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,Fdate,'{0}')=0", paraDic["Fdate"].ToString());
            }
            string sqlstr = @"select FINWAREHOUSEID FWAREHOUSEID,SUM(FMARKETMONEY) FMARKETMONEY,0 FBACKTMONEY,0 FDIFFERMONEY from dbo.T_SALEDAYBOOK where " + whereStr + @"
group by FINWAREHOUSEID";

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<Object>(sqlstr);
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
