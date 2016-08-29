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
  public  class GOODSALLOTService : DBContext
    {
        public IEnumerable<T_GOODSALLOTModel> GetAllGOODSALLOTInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
                whereStr += string.Format(" and FPERSONID like '%{0}%'", paraDic["FPERSONID"].ToString());
            }
            if (paraDic.Contains("FStatus") && paraDic["FStatus"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FSTATUS='{0}'", paraDic["FStatus"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_GOODSALLOT");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_GOODSALLOTModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_GOODSALLOTModel AddGOODSALLOT(T_GOODSALLOTModel model)
        {

            string sql = @"insert into  T_GOODSALLOT(FGUID, FCREATEID, FCREATETIME, FDATE, FNUMBER, FCODE, FOUTWAREHOUSEID,FINWAREHOUSEID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FDATE, @FNUMBER, @FCODE, @FOUTWAREHOUSEID,@FINWAREHOUSEID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GOODSALLOTModel>("select * from T_GOODSALLOT with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public IEnumerable<object> SearchGOODSALLOT(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
                dp.Add("@tblName", @"T_GOODSALLOT c
inner join T_GOODSALLOTDETAILS d on c.FGUID=d.FPARENTID");
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

        public IEnumerable<object> SearchGOODSALLOTHourse(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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

        public bool submitGOODSALLOT(string FGUID)
        {
            string sql = @"update  T_GOODSALLOT set   FSTATUS='2'
where FGUID=@FGUID; ";
            sql += "update T_REPERTORY set T_REPERTORY.FSURPLUS=T_REPERTORY.FSURPLUS-a.qcou from (select sum(g.FQUANTITY) qcou,g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID from T_GOODSALLOTDETAILS g inner join T_GOODSALLOT p on p.FGUID=g.FPARENTID where p.FGUID=@FGUID group by g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID)a where T_REPERTORY.FBATCH=a.FBATCH and T_REPERTORY.FWAREHOUSEID=a.FOUTWAREHOUSEID and T_REPERTORY.FGOODSID=a.FGOODSID;";
            sql += @"update T_REPERTORYCHILD set FQUANTITY=T_REPERTORYCHILD.FQUANTITY+a.FQUANTITY,
FSURPLUS=T_REPERTORYCHILD.FSURPLUS+a.FQUANTITY,FENABLE=T_REPERTORYCHILD.FENABLE+a.FQUANTITY,
FPRICE=a.FPRICE,FMARKETPRICE=a.FMARKETPRICE,
FBARCODE=convert(varchar(20),a.FINWAREHOUSEID)+' '+CONVERT(varchar(20),a.FGOODSID)+' '+CONVERT(varchar(20), a.FMARKETPRICE)+' '+CONVERT(varchar(20),T_REPERTORYCHILD.FQUANTITY+a.FQUANTITY)
from (select g.FCREATEID,g.FCREATETIME,gd.FBATCH,g.FOUTWAREHOUSEID,g.FINWAREHOUSEID,gd.FBARCODE
,gd.FGOODSID,gd.FGOODSNAME,gd.FUNIT,gd.FQUANTITY ,gd.FPRICE,gd.FMARKETPRICE,gd.FMEMO from T_GOODSALLOT g 
inner join T_GOODSALLOTDETAILS gd on g.FGUID=gd.FPARENTID where g.FGUID=@FGUID) a where T_REPERTORYCHILD.FBATCH=a.FBATCH and T_REPERTORYCHILD.FINWAREHOUSEID=a.FINWAREHOUSEID
and T_REPERTORYCHILD.FOUTWAREHOUSEID=a.FOUTWAREHOUSEID and T_REPERTORYCHILD.FGOODSID=a.FGOODSID;";
            sql += @"insert into T_REPERTORYCHILD (FGUID,FCREATEID,FCREATETIME,FBATCH,FOUTWAREHOUSEID,FINWAREHOUSEID,FBARCODE,FGOODSID,FGOODSNAME,FUNIT,FQUANTITY,FSURPLUS,FENABLE,FPRICE,FMARKETPRICE,FMEMO) select NEWID(),g.FCREATEID,g.FCREATETIME,gd.FBATCH,g.FOUTWAREHOUSEID,g.FINWAREHOUSEID,gd.FBARCODE
,gd.FGOODSID,gd.FGOODSNAME,gd.FUNIT,gd.FQUANTITY,gd.FQUANTITY,gd.FQUANTITY,gd.FPRICE,gd.FMARKETPRICE,gd.FMEMO from T_GOODSALLOT g 
inner join T_GOODSALLOTDETAILS gd on g.FGUID=gd.FPARENTID where g.FGUID=@FGUID and not exists(select 1 from T_REPERTORYCHILD t where t.FINWAREHOUSEID=g.FINWAREHOUSEID and t.FOUTWAREHOUSEID=g.FOUTWAREHOUSEID 
and t.FBATCH=gd.FBATCH and t.FGOODSID=gd.FGOODSID) ";
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

        public IEnumerable<T_GOODSALLOTModel> GetGoodsallotModes(IDictionary dic)
        {
            string sqlstr = "select * from T_GOODSALLOT where 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (dic.Contains("FGUID"))
            {
                sqlstr += " and FGUID=@FGUID";
                dp.Add("@FGUID", dic["FGUID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<T_GOODSALLOTModel>(sqlstr, dp).AsList();
                return result;
            }
        }
    }
}
