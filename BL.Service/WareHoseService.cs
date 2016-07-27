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
            string sql = "select * from T_WAREHOUSE with(nolock)";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_WAREHOUSEModel>(sql);
            }
        }/// <summary>
         /// 添加仓库(状态默认为未启用)
         /// </summary>
         /// <returns></returns>
        public T_WAREHOUSEModel AddWareHoseInfo(T_WAREHOUSEModel model)
        {
            string sql = "insert into T_WAREHOUSE(FGUID,FID,FNAME,FCATEGORY,FPARENTID,FPROVINCE,FSTATUS) values(@FGUID,@FID,@FNAME,@FCATEGORY,@FPARENTID,@FPROVINCE,@FSTATUS)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_WAREHOUSEModel>("select * from T_WAREHOUSE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public T_WAREHOUSEModel EditWareHoseByGuid(T_WAREHOUSEModel model)
        {
            string sql = "update T_WAREHOUSE set FID=@FID,FNAME=@FNAME,FCATEGORY=@FCATEGORY,FPARENTID=@FPARENTID,FPROVINCE=@FPROVINCE where FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_WAREHOUSEModel>("select * from T_WAREHOUSE with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
                    res.closeCurrent = true;
                    res.message = "修改成功";
                    return res;
                }
                else
                {
                    model.closeCurrent = true;
                    model.statusCode = "300";
                    model.message = "修改失败";
                    return model;
                }
            }
        }

        public bool DelWareHoseByGuid(string FGUID)
        {
            string sql = "delete from T_WAREHOUSE where FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID }) > 0;
            }
        }

        public bool SetWareHoseStatusByGuid(string FGUID, string FSTATUS, DateTime now)
        {
            string sql = string.Empty;
            if (FSTATUS == "2")//设置启用
            {
                sql = "update T_WAREHOUSE set  FSTATUS=@FSTATUS,FSTARTTIME=@time where FGUID=@FGUID";
            }
            else if (FSTATUS == "3")
            {
                sql = "update T_WAREHOUSE set  FSTATUS=@FSTATUS,FENDTIME=@time where FGUID=@FGUID";
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FSTATUS = FSTATUS, time = now, FGUID = FGUID }) > 0;
            }
        }
        /// <summary>
        /// 判断编号是否已存在
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsFID(string FGUID, string FID)
        {
            string sql = "select * from T_WAREHOUSE where FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_GOODSModel>(sql, new { FGUID = FGUID, FID = FID }).Count() > 0;
            }
        }
    }
}
