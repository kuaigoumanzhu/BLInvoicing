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
   public class PricesRegisterDetailService : DBContext
    {
        public IEnumerable<T_GUIDANCEDETAILSModel> GetAllGUIDANCEDETAILSInfo(IDictionary paraDic)
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
                string sqlstr = "select * from T_GUIDANCEDETAILS where ";
                var result = db.Query<T_GUIDANCEDETAILSModel>(sqlstr + whereStr, dp);
                return result;
            }
        }


        public T_GUIDANCEDETAILSModel AddGUIDANCEDETAILS(T_GUIDANCEDETAILSModel model)
        {

            string sql = @"insert into  T_GUIDANCEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FMARKETPRICE, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FPARENTID, @FGOODSID, @FGOODSNAME, @FUNIT, @FMARKETPRICE, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {

                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GUIDANCEDETAILSModel>("select * from T_GUIDANCEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public bool AddFNCBALANCEDETAILS(List<T_GUIDANCEDETAILSModel> list)
        {

            string sql = "";
            var model = list[0];
            DynamicParameters dp = new DynamicParameters();
            for (int i = 0; i < list.Count(); i++)
            {
                sql += @"insert into  T_GUIDANCEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FMARKETPRICE, FMEMO
) values(@FGUID" + i + ", @FCREATEID" + i + ", @FCREATETIME" + i + ", @FPARENTID" + i + ", @FGOODSID" + i + ", @FGOODSNAME" + i + ", @FUNIT" + i + ", @FMARKETPRICE" + ", @FMEMO" + i + ");";
                dp.Add("@FGUID" + i, Guid.NewGuid().ToString());
                dp.Add("@FCREATEID" + i, list[0].FCREATEID);
                dp.Add("@FCREATETIME" + i, list[0].FCREATETIME);
                dp.Add("@FPARENTID" + i, list[0].FPARENTID);
                dp.Add("@FGOODSID" + i, list[i].FGOODSID);
                dp.Add("@FGOODSNAME" + i, list[i].FGOODSNAME);
                dp.Add("@FUNIT" + i, list[i].FUNIT);
                dp.Add("@FMARKETPRICE" + i, list[i].FMARKETPRICE);
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

        public T_GUIDANCEDETAILSModel EditFNCBALANCEDETAILS(T_GUIDANCEDETAILSModel model)
        {

            string sql = @"update  T_GUIDANCEDETAILS set   FGOODSID=@FGOODSID, FGOODSNAME=@FGOODSNAME, FUNIT=@FUNIT, FQUANTITY=@FQUANTITY, FPRICE=@FPRICE, FMONEY=@FMONEY, FSUPPLIERID=@FSUPPLIERID, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GUIDANCEDETAILSModel>("select * from T_GUIDANCEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public int DelCONSUMABLESDETAILS(string FGUID)
        {

            string sql = @"delete  T_GUIDANCEDETAILS 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public IEnumerable<Object> GetSelectOutGoods(IDictionary paraDic)
        {
            string whereStr = " ";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FWAREHOUSEID") && paraDic["FWAREHOUSEID"].ToString().Trim() != "")
            {
                whereStr += " and c.FWAREHOUSEID=@FWAREHOUSEID";
                dp.Add("@FWAREHOUSEID", paraDic["FWAREHOUSEID"].ToString());

            }
            string sqlstr = @"select c.FWAREHOUSEID,d.FGOODSID,d.FGOODSNAME,d.FUNIT,SUM(case when c.FTYPE='1' then d.FQUANTITY else 0 end)-SUM(case when c.FTYPE='2' then d.FQUANTITY else 0 end) goodsNum from T_CONSUMABLES c
 inner join T_GUIDANCEDETAILS d on c.FGUID=d.FPARENTID where c.FSTATUS='2' " + whereStr + "  group by c.FWAREHOUSEID,d.FGOODSID,d.FGOODSNAME,d.FUNIT having SUM(case when c.FTYPE='1' then d.FQUANTITY else 0 end)-SUM(case when c.FTYPE='2' then d.FQUANTITY else 0 end)>0";

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<Object>(sqlstr, dp).AsList();
                return result;
            }

        }
    }
}
