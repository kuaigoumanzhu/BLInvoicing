using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Framework.Orm;
using BL.Models;
using Dapper;

namespace BL.Service
{
    public class WareHoseService:DBContext
    {
        public IEnumerable<T_WAREHOUSEModel> GetAllWareHoseInfo()
        {
            string sql = "select * from T_WAREHOUSE";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_WAREHOUSEModel>(sql);
            }
        }
    }
}
