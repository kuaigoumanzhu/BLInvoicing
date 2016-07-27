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
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FID like '%{0}%'", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
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

        public T_FNCBALANCEModel AddFncbalance(T_FNCBALANCEModel model)
        {

            string sql = @"insert into  T_FNCBALANCE( FGUID, FCREATEID, FCREATETIME, FDATE, FNUMBER, FCODE, FWAREHOUSEID, FMEMO, FSTATUS, FAPPLYID, FAPPLYTIME
) values(@FGUID, @FCREATEID, @FCREATETIME, @FDATE, @FNUMBER, @FCODE, @FWAREHOUSEID, @FMEMO, @FSTATUS, @FAPPLYID, @FAPPLYTIME
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_FNCBALANCEModel>("select * from T_FNCBALANCE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
    }
}
