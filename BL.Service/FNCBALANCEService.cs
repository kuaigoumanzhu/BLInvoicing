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
    public class FNCBALANCEService : DBContext
    {
        public IEnumerable<T_FNCBALANCEModel> GetAllFNCBALANCEInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " 1=1 ";
            if (paraDic.Contains("Fdate") && paraDic["Fdate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')=0", paraDic["Fdate"].ToString());
            }
            if (paraDic.Contains("FCode") && paraDic["FCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FCODE like '%{0}%'", paraDic["FCode"].ToString());
            }
            if (paraDic.Contains("FState") && paraDic["FState"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FSTATUS='{0}'", paraDic["FState"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_FNCBALANCE");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_FNCBALANCEModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_FNCBALANCEModel AddFncbalance(T_FNCBALANCEModel model, int id, int number, CommonService service)
        {

            string sql = @"insert into  T_FNCBALANCE( FGUID, FCREATEID, FCREATETIME, FDATE, FNUMBER, FCODE, FWAREHOUSEID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FDATE, @FNUMBER, @FCODE, @FWAREHOUSEID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    if (service.UpdateNumberById(db, transaction, id, number))
                    {
                        db.Execute(sql, model, transaction);
                        var res = db.QuerySingle<T_FNCBALANCEModel>("select * from T_FNCBALANCE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID }, transaction);
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
                        model.message = "添加失败";
                        return model;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public bool submitFNCBALANCE(string FGUID)
        {
            string sql = @"update  T_FNCBALANCE set   FSTATUS='2'
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

        public IEnumerable<T_FNCBALANCEModel> GetFncbalanceModes(IDictionary dic)
        {
            string sqlstr = "select * from T_FNCBALANCE where 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (dic.Contains("FGUID"))
            {
                sqlstr += " and FGUID=@FGUID";
                dp.Add("@FGUID", dic["FGUID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<T_FNCBALANCEModel>(sqlstr, dp).AsList();
                return result;
            }
        }
    }
}
