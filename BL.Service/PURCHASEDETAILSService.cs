﻿using BL.Framework.Orm;
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
   public class PURCHASEDETAILSService : DBContext
    {
        public IEnumerable<T_PURCHASEDETAILSModel> GetAllPURCHASEDETAILSInfo(IDictionary paraDic)
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
                string sqlstr = "select * from T_PURCHASEDETAILS where ";
                var result = db.Query<T_PURCHASEDETAILSModel>(sqlstr + whereStr, dp);
                return result;
            }
        }



        public T_PURCHASEDETAILSModel AddPURCHASEDETAILS(T_PURCHASEDETAILSModel model)
        {

            string sql = @"insert into  T_PURCHASEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FCALCTYPE, FQUANTITY, FMONEY, FPRICE, FSUPPLIERID, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FPARENTID, @FGOODSID, @FGOODSNAME, @FUNIT, @FCALCTYPE, @FQUANTITY, @FMONEY, @FPRICE, @FSUPPLIERID, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {

                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_PURCHASEDETAILSModel>("select * from T_PURCHASEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public bool AddPURCHASEDETAILS(List<T_PURCHASEDETAILSModel> list)
        {

            string sql = "";
            var model = list[0];
            DynamicParameters dp = new DynamicParameters();
            for (int i = 0; i < list.Count(); i++)
            {
                sql += @"insert into  T_PURCHASEDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FCALCTYPE, FQUANTITY, FMONEY, FPRICE, FSUPPLIERID, FMEMO
) values(@FGUID" + i + ", @FCREATEID" + i + ", @FCREATETIME" + i + ", @FPARENTID" + i + ", @FGOODSID" + i + ", @FGOODSNAME" + i + ", @FUNIT" + i + ", @FCALCTYPE" + i + ", @FQUANTITY" + i + ", @FMONEY" + i + ",@FPRICE" + i + ", @FSUPPLIERID" + i + ", @FMEMO" + i + ");";
                dp.Add("@FGUID" + i, Guid.NewGuid().ToString());
                dp.Add("@FCREATEID" + i, list[0].FCREATEID);
                dp.Add("@FCREATETIME" + i, list[0].FCREATETIME);
                dp.Add("@FPARENTID" + i, list[0].FPARENTID);
                dp.Add("@FGOODSID" + i, list[i].FGOODSID);
                dp.Add("@FGOODSNAME" + i, list[i].FGOODSNAME);
                dp.Add("@FUNIT" + i, list[i].FUNIT);
                dp.Add("@FCALCTYPE" + i, list[i].FCALCTYPE);
                dp.Add("@FQUANTITY" + i, list[i].FQUANTITY);
                dp.Add("@FPRICE" + i, list[i].FPRICE);
                dp.Add("@FMONEY" + i, list[i].FMONEY);
                dp.Add("@FSUPPLIERID" + i, list[i].FSUPPLIERID);
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
        public T_PURCHASEDETAILSModel EditPURCHASEDETAILS(T_PURCHASEDETAILSModel model)
        {

            string sql = @"update  T_PURCHASEDETAILS set   FGOODSID=@FGOODSID, FGOODSNAME=@FGOODSNAME, FUNIT=@FUNIT, FQUANTITY=@FQUANTITY, FPRICE=@FPRICE, FMONEY=@FMONEY, FSUPPLIERID=@FSUPPLIERID, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_PURCHASEDETAILSModel>("select * from T_PURCHASEDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public int DelPURCHASEDETAILS(string FGUID)
        {

            string sql = @"delete  T_PURCHASEDETAILS 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public IEnumerable<Object> GetSelectGoods(IDictionary paraDic)
        {
            DynamicParameters dp = new DynamicParameters();
            string sqlstr = @"select FID as FGOODSID,FNAME as FGOODSNAME,FUNIT,FCALCTYPE from T_goods where FSTATUS='2' and FISCONSUMABLES='0'";

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<Object>(sqlstr, dp).AsList();
                return result;
            }

        }
    }
}
