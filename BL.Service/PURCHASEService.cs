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
   public class PURCHASEService : DBContext
    {
        public IEnumerable<T_PURCHASEModel> GetAllPURCHASEInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " 1=1 ";
            if (paraDic.Contains("FDate") && paraDic["FDate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')=0", paraDic["FDate"].ToString());
            }
            if (paraDic.Contains("FCode") && paraDic["FCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FCode like '%{0}%'", paraDic["FCode"].ToString());
            }
            if (paraDic.Contains("FPERSONID") && paraDic["FPERSONID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FPERSONID='{0}'", paraDic["FPERSONID"].ToString());
            }
            if (paraDic.Contains("FStatus") && paraDic["FStatus"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FSTATUS='{0}'", paraDic["FStatus"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_PURCHASE");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_PURCHASEModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_PURCHASEModel AddPURCHASE(T_PURCHASEModel model,int id, int number, CommonService service)
        {

            string sql = @"insert into  T_PURCHASE(FGUID, FCREATEID, FCREATETIME, FDATE, FNUMBER, FCODE, FWAREHOUSEID,FPERSONID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME,FCHECKID,FCHECKTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FDATE, @FNUMBER, @FCODE, @FWAREHOUSEID,@FPERSONID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME, @FCHECKID, @FCHECKTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try { 
                if (service.UpdateNumberById(db, transaction, id, number))
                {
                    db.Execute(sql, model, transaction);
                    var res = db.QuerySingle<T_PURCHASEModel>("select * from T_PURCHASE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID },transaction);
                    transaction.Commit();
                    res.closeCurrent = true;
                    res.message = "添加成功";
                    return res;
                }
                else
                {
                    transaction.Rollback();
                    model.closeCurrent = true;
                    model.statusCode = "300";
                    model.message = "新增流水号已被占用，请重新保存！";
                    return model;
                }
                }catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<object> SearchPURCHASE(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " c.FSTATUS='2' ";
            if (paraDic.Contains("FDate") && paraDic["FDate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')=0", paraDic["FDate"].ToString());
            }
            if (paraDic.Contains("FCode") && paraDic["FCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and c.FCode like '%{0}%'", paraDic["FCode"].ToString());
            }
            if (paraDic.Contains("FGoodsName") && paraDic["FGoodsName"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and d.FGOODSNAME like '%{0}%'", paraDic["FGoodsName"].ToString());
            }
            if (paraDic.Contains("FPERSONID") && paraDic["FPERSONID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FPERSONID like '%{0}%'", paraDic["FPERSONID"].ToString());
            }
            if (paraDic.Contains("FStatus") && paraDic["FStatus"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and c.FSTATUS='{0}'", paraDic["FStatus"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", @"T_PURCHASE c
inner join T_PURCHASEDETAILS d on c.FGUID=d.FPARENTID");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "c.*,d.FGOODSID,d.FGOODSNAME,d.FUNIT,d.FQUANTITY,d.FPRICE,d.FMONEY");
                dp.Add("@strOrder", " c.FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<object>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public IEnumerable<object> SearchPURCHASEHourse(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " c.FSTATUS='2' ";
            if (paraDic.Contains("FGoodsCode") && paraDic["FGoodsCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and c.FGOODSID like '%{0}%'", paraDic["FCode"].ToString());
            }
            if (paraDic.Contains("FGoodsName") && paraDic["FGoodsName"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and d.FGOODSNAME like '%{0}%'", paraDic["FGoodsName"].ToString());
            }

            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", @"(select d.FGOODSID,d.FGOODSNAME,SUM(case when c.FTYPE='1' then d.FQUANTITY else 0 end)-SUM(case when c.FTYPE='2' then d.FQUANTITY else 0 end) syCou,
        d.FUNIT,d.FPRICE,SUM(case when c.FTYPE='1' then d.FMONEY else 0 end)-SUM(case when c.FTYPE='2' then d.FMONEY else 0 end) FMONEY from T_CONSUMABLES c
        inner join T_CONSUMABLESDETAILS d on c.FGUID=d.FPARENTID where " + whereStr + @"
        group by d.FGOODSID,d.FGOODSNAME,d.FUNIT,d.FPRICE) cd");
                dp.Add("@strWhere", "");
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", " FGOODSID desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<object>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public bool submitPURCHASE(string FGUID,string userName,DateTime dt,string FStatus)
        {
            string sql = @"update  T_PURCHASE set   FSTATUS=@FSTATUS,FCHECKID=@FCHECKID,FCHECKTIME=@FCHECKTIME
where FGUID=@FGUID ";
            sql += @"insert into T_REPERTORY(
       [FCREATEID]
      ,[FCREATETIME]
      ,[FBATCH]
      ,[FSUPPLIERID]
      ,[FWAREHOUSEID]
      ,[FPERSONID]
      ,[FGOODSID]
      ,[FGOODSNAME]
      ,[FUNIT]
      ,[FCALCTYPE]
      ,[FQUANTITY]
      ,[FSURPLUS]
      ,[FENABLE]
      ,[FPRICE]
      ,[FMEMO])
      select
      a.FCREATEID,
      getdate(),
      a.FCODE,
      b.FSUPPLIERID,
      a.FWAREHOUSEID,
      a.FPERSONID,
      b.FGOODSID,
      b.FGOODSNAME,
      b.FUNIT,
      b.FCALCTYPE,
      b.FQUANTITY,
      b.FQUANTITY,
      b.FQUANTITY,
      b.FPRICE,
      b.FMEMO
      from T_PURCHASE a inner join T_PURCHASEDETAILS b on a.FGUID= b.FPARENTID where a.FGUID= @FGUID";
            using (IDbConnection db = OpenConnection())
            {
                //if (db.Execute(sql, new { FGUID = FGUID }) > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                var trans = db.BeginTransaction();
                if (db.Execute(sql, new
                {
                    FSTATUS = FStatus,
                    FCHECKID = userName,
                    FCHECKTIME = dt,
                    FGUID = FGUID
                }, trans) > 0)
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


        public bool submitPURCHASE(string FGUID)
        {
            string sql = @"update  T_PURCHASE set   FSTATUS='2' where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, new
                {
                    FGUID = FGUID
                }) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool submitGOODSALLOT(string FGUID)
        {
            string sql = @"update  T_GOODSALLOT set   FSTATUS='2'
where FGUID=@FGUID; ";
            sql += "update T_REPERTORY set T_REPERTORY.FSURPLUS=T_REPERTORY.FSURPLUS-a.qcou from (select sum(g.FQUANTITY) qcou,g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID from T_GOODSALLOTDETAILS g inner join T_GOODSALLOT p on p.FGUID=g.FPARENTID where p.FGUID=@FGUID group by g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID)a where T_REPERTORY.FBATCH=a.FBATCH and T_REPERTORY.FWAREHOUSEID=a.FOUTWAREHOUSEID and T_REPERTORY.FGOODSID=a.FGOODSID";
            using (IDbConnection db = OpenConnection())
            {
                //if (db.Execute(sql, new { FGUID = FGUID }) > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                var trans = db.BeginTransaction();
                if (db.Execute(sql, new { FGUID = FGUID }, trans) > 0)
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

        public IEnumerable<T_PURCHASEModel> GetPurchaseModes(IDictionary dic)
        {
            string sqlstr = "select * from T_PURCHASE where 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (dic.Contains("FGUID"))
            {
                sqlstr += " and FGUID=@FGUID";
                dp.Add("@FGUID", dic["FGUID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<T_PURCHASEModel>(sqlstr, dp).AsList();
                return result;
            }
        }
    }
}
