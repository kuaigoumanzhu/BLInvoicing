using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;
using BL.Framework.Orm;

namespace BL.Service
{
    public class RepertoryCheckService : DBContext
    {
        public IEnumerable<ViewREPERTORYCHECK> GetAllRepertoryCheckInfo(int pageIndex, int pageSize, out int total)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@tblName", "T_REPERTORYCHECK a with(nolock) left join T_Person b with(nolock) on a.FAPPLYID = b.FID");
            dp.Add("@strWhere", " 1=1 ");
            dp.Add("@fldName", "a.*,b.FNAME as FAPPLYName");
            dp.Add("@strOrder", "a.FCREATETIME desc");
            dp.Add("@PageSize", pageSize);
            dp.Add("@PageIndex", pageIndex);
            using (IDbConnection db = OpenConnection())
            {
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                total = result.Read<int>().Single();
                return result.Read<ViewREPERTORYCHECK>();
            }
        }
    }
}
