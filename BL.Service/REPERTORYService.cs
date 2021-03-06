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
    class REPERTORYService : DBContext
    {
        public IEnumerable<T_REPERTORYModel> GetAllREPERTORYInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
            if (paraDic.Contains("FType") && paraDic["FType"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FType='{0}'", paraDic["FType"].ToString());
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
                dp.Add("@tblName", "T_REPERTORY");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_REPERTORYModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_REPERTORYModel AddREPERTORY(T_REPERTORYModel model)
        {

            string sql = @"insert into  T_REPERTORY(FGUID, FCREATEID, FCREATETIME, FTYPE, FDATE, FNUMBER, FCODE, FPERSONID, FWAREHOUSEID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FTYPE, @FDATE, @FNUMBER, @FCODE, @FPERSONID, @FWAREHOUSEID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_REPERTORYModel>("select * from T_REPERTORY with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public IEnumerable<object> SearchREPERTORY(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " c.FSTATUS='2' ";
            if (paraDic.Contains("startFDate") && paraDic["startFDate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')<=0", paraDic["startFDate"].ToString());
            }
            if (paraDic.Contains("endFDate") && paraDic["endFDate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')>=0", paraDic["endFDate"].ToString());
            }
            if (paraDic.Contains("FCode") && paraDic["FCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and c.FCode like '%{0}%'", paraDic["FCode"].ToString());
            }
            if (paraDic.Contains("FGoodsName") && paraDic["FGoodsName"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and d.FGOODSNAME like '%{0}%'", paraDic["FGoodsName"].ToString());
            }
            if (paraDic.Contains("FType") && paraDic["FType"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and d.FType='{0}'", paraDic["FType"].ToString());
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
                dp.Add("@tblName", @"T_REPERTORY c
inner join T_REPERTORYDETAILS d on c.FGUID=d.FPARENTID");
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

        public IEnumerable<object> SearchREPERTORYHourse(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
d.FUNIT,d.FPRICE,SUM(case when c.FTYPE='1' then d.FMONEY else 0 end)-SUM(case when c.FTYPE='2' then d.FMONEY else 0 end) FMONEY from T_REPERTORY c
inner join T_REPERTORYDETAILS d on c.FGUID=d.FPARENTID where " + whereStr + @"
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

        public bool submitREPERTORY(string FGUID)
        {
            string sql = @"update  T_REPERTORY set   FSTATUS='2'
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, new { FGUID = FGUID }) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}