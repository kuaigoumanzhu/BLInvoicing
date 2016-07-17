using BL.Framework.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace BL.Service
{
    public class GoodsService : DBContext
    {
        public IEnumerable<T_GOODSModel> GetAllGoodsInfo()
        {
            string sql = "select * from T_GOODS";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_GOODSModel>(sql);
            }
        }

        public int AddGoods(T_GOODSModel model)
        {

            string sql = @"insert into  T_GOODS(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FSTANDARD, FUNIT, FCALCTYPE, FCATEGORY, FISCONSUMABLES, FSTATUS, FSTARTTIME, FENDTIME, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FSTANDARD, @FUNIT, @FCALCTYPE, @FCATEGORY, @FISCONSUMABLES, @FSTATUS, @FSTARTTIME, @FENDTIME, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
               // SqlParameter[] parames={
               //     new SqlParameter("FGUID",model.FGUID),
               //     new SqlParameter("FCREATEID",model.FCREATEID),
               //     new SqlParameter("FCREATETIME",model.FCREATETIME),
               //     new SqlParameter("FID",model.FID),
               //     new SqlParameter("FNAME",model.FNAME),
               //     new SqlParameter("FSTANDARD",model.FSTANDARD),
               //     new SqlParameter("FUNIT",model.FUNIT),
               //     new SqlParameter("FCALCTYPE",model.FCALCTYPE),
               //     new SqlParameter("FCATEGORY",model.FCATEGORY),
               //     new SqlParameter("FISCONSUMABLES",model.FISCONSUMABLES),
               //     new SqlParameter("FSTATUS",model.FSTATUS),
               //     new SqlParameter("FSTARTTIME",model.FSTARTTIME),
               //     new SqlParameter("FENDTIME",model.FENDTIME),
               //     new SqlParameter("FMEMO",model.FMEMO)
               // };
               //return Convert.ToInt32( db.(sql,parames));
                return db.Execute(sql, model);
            }
        }

        public int EditGoods(T_GOODSModel model)
        {

            string sql = @"update  T_GOODS set FCREATEID=@FCREATEID, FCREATETIME=@FCREATETIME, FID=@FID, FNAME=@FNAME, FSTANDARD=@FSTANDARD, FUNIT=@FUNIT, FCALCTYPE=@FCALCTYPE, FCATEGORY=@FCATEGORY, FISCONSUMABLES=@FISCONSUMABLES, FSTATUS=@FSTATUS, FSTARTTIME=@FSTARTTIME, FENDTIME=@FENDTIME, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, model);
            }
        }

        public int DelGoods(string FGUID)
        {

            string sql = @"delete  T_GOODS 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID=FGUID});
            }
        }
        public bool SetGoodsStatusByGuid(string FGUID, string FSTATUS, DateTime now)
        {
            string sql = string.Empty;
            if (FSTATUS == "2")//设置启用
            {
                sql = "update T_GOODS set  FSTATUS=@FSTATUS,FSTARTTIME=@time where FGUID=@FGUID";
            }
            else if (FSTATUS == "3")
            {
                sql = "update T_GOODS set  FSTATUS=@FSTATUS,FENDTIME=@time where FGUID=@FGUID";
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FSTATUS = FSTATUS, time = now, FGUID = FGUID }) > 0;
            }
        }
    }
}
