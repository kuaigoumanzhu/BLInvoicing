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
    public class CONSUMABLESDETAILSService : DBContext
    {
        public IEnumerable<T_CONSUMABLESDETAILSModel> GetAllCONSUMABLESDETAILSInfo(IDictionary paraDic)
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
                string sqlstr = "select * from T_CONSUMABLESDETAILS where ";
                var result = db.Query<T_CONSUMABLESDETAILSModel>(sqlstr+whereStr,dp);
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
                var result = db.Query<Object>(sqlstr,dp);
                return result;
            }

        }


        public T_CONSUMABLESDETAILSModel AddFNCBALANCEDETAILS(T_CONSUMABLESDETAILSModel model)
        {

            string sql = @"insert into  T_CONSUMABLESDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FQUANTITY, FPRICE, FMONEY, FSUPPLIERID, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FPARENTID, @FGOODSID, @FGOODSNAME, @FUNIT, @FQUANTITY, @FPRICE, @FMONEY, @FSUPPLIERID, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_CONSUMABLESDETAILSModel>("select * from T_CONSUMABLESDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public T_CONSUMABLESDETAILSModel EditFNCBALANCEDETAILS(T_CONSUMABLESDETAILSModel model)
        {

            string sql = @"update  T_CONSUMABLESDETAILS set   FGOODSID=@FGOODSID, FGOODSNAME=@FGOODSNAME, FUNIT=@FUNIT, FQUANTITY=@FQUANTITY, FPRICE=@FPRICE, FMONEY=@FMONEY, FSUPPLIERID=@FSUPPLIERID, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_CONSUMABLESDETAILSModel>("select * from T_CONSUMABLESDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public IEnumerable<Object> GetSelectOutGoods(IDictionary paraDic)
        {
            string whereStr = " cd.goodsNum>0 ";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FWAREHOUSEID") && paraDic["FWAREHOUSEID"].ToString().Trim() != "")
            {
                whereStr += " and cd.FWAREHOUSEID=@FWAREHOUSEID";
                dp.Add("@FWAREHOUSEID", paraDic["FWAREHOUSEID"].ToString());

            }
            string sqlstr = @"select g.*,cd.* from T_goods g
inner join (select c.FWAREHOUSEID,d.FGOODSID,SUM(case when c.FTYPE=1 then d.FQUANTITY else -d.FQUANTITY end) goodsNum from T_CONSUMABLES c
 inner join T_CONSUMABLESDETAILS d on c.FGUID=d.FPARENTID group by c.FWAREHOUSEID,d.FGOODSID) cd on g.FID=cd.FGOODSID where " + whereStr ;

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<Object>(sqlstr,dp);
                return result;
            }

        } 
    }
}
