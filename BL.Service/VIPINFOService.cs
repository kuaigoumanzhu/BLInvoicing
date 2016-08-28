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
     public  class VIPINFOOService : DBContext
    {
        public IEnumerable<T_VIPINFOModel> GetVIPINFOInfo(IDictionary paraDic)
        {
            string sql = "select * from T_VIPINFO";
            DynamicParameters dp = new DynamicParameters();
            string whereStr = " where 1=1 ";
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += " and FID like '%'+@FID+'%'";
                dp.Add("@FID", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += " and FNAME like '%'+@FNAME+'%'";
                dp.Add("@FNAME", paraDic["FNAME"].ToString());
            }

            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_VIPINFOModel>(sql + whereStr, dp);
            }
        }
       
    }
}
