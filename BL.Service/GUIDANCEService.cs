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
    public class GUIDANCEService : DBContext
    {
        public IEnumerable<T_GUIDANCEModel> GetAllGUIDANCEInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
            if (paraDic.Contains("FStatus") && paraDic["FStatus"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FSTATUS='{0}'", paraDic["FStatus"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_GUIDANCE");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_GUIDANCEModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_GUIDANCEModel AddGUIDANCE(T_GUIDANCEModel model, int id, int number, CommonService service)
        {

            string sql = @"insert into  T_GUIDANCE(FGUID, FCREATEID, FCREATETIME, FDATE, FNUMBER, FCODE, FWAREHOUSEID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FDATE, @FNUMBER, @FCODE,  @FWAREHOUSEID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    if (service.UpdateNumberById(db, transaction, id, number))
                    {
                        db.Execute(sql, model, transaction);
                        var res = db.QuerySingle<T_GUIDANCEModel>("select * from T_GUIDANCE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID }, transaction);
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

        public IEnumerable<object> SearchGUIDANCE(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
                dp.Add("@tblName", @"T_GUIDANCE c
inner join T_GUIDANCEDETAILS d on c.FGUID=d.FPARENTID");
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

        public IEnumerable<object> SearchGUIDANCEHourse(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
d.FUNIT,d.FPRICE,SUM(case when c.FTYPE='1' then d.FMONEY else 0 end)-SUM(case when c.FTYPE='2' then d.FMONEY else 0 end) FMONEY from T_GUIDANCE c
inner join T_GUIDANCEDETAILS d on c.FGUID=d.FPARENTID where "+whereStr+@"
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

        public bool submitGUIDANCE(string FGUID)
        {
            string sql = @"update  T_GUIDANCE set   FSTATUS='2'
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, new { FGUID=FGUID }) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IEnumerable<T_GUIDANCEModel> GetGuidanceModes(IDictionary dic)
        {
            string sqlstr = "select * from T_GUIDANCE where 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (dic.Contains("FGUID"))
            {
                sqlstr += " and FGUID=@FGUID";
                dp.Add("@FGUID", dic["FGUID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<T_GUIDANCEModel>(sqlstr, dp).AsList();
                return result;
            }
        }
    }
}
