using BL.Framework.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;

namespace BL.Service
{
    public class GoodsBackService: DBContext
    {
        public IEnumerable<T_GOODSBACKModel> GetAllGoodsBackInfo()
        {
            string sql = "select * from T_GOODSBACK";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_GOODSBACKModel>(sql);
            }
        }
    }
}
