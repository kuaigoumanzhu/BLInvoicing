using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Framework.Orm;
using BL.Models;
using Dapper;
using System.Collections;

namespace BL.Service
{
    public class WareHoseService:DBContext
    {

        public IEnumerable<T_WAREHOUSEModel> GetAllWareHoseInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
            if (paraDic.Contains("FPROVINCE") && paraDic["FPROVINCE"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FPROVINCE ='{0}'", paraDic["FPROVINCE"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_WAREHOUSE");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_WAREHOUSEModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }
        public IEnumerable<T_WAREHOUSEModel> GetAllWareHoseInfo()
        {
            string sql = "select * from T_WAREHOUSE where FCATEGORY=2";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_WAREHOUSEModel>(sql);
            }
        }

        public IEnumerable<T_WAREHOUSEModel> GetAllWareHoseInfo(IDictionary paraDic)
        {
            
            string sql = "select * from T_WAREHOUSE with(nolock)";
            string whereStr = " where 1=1";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += " and FID = @FID";
                dp.Add("@FID", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FCATEGORY") && paraDic["FCATEGORY"].ToString().Trim() != "")
            {
                whereStr += " and FCATEGORY = @FCATEGORY";
                dp.Add("@FCATEGORY", paraDic["FCATEGORY"].ToString());
            }
            if (paraDic.Contains("FSTATUS") && paraDic["FSTATUS"].ToString().Trim() != "")
            {
                whereStr += " and FSTATUS = @FSTATUS";
                dp.Add("@FSTATUS", paraDic["FSTATUS"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_WAREHOUSEModel>(sql+whereStr,dp);
            }
        }
        /// <summary>
         /// 添加仓库(状态默认为未启用)
         /// </summary>
         /// <returns></returns>
        public T_WAREHOUSEModel AddWareHoseInfo(T_WAREHOUSEModel model)
        {
            string sql = "insert into T_WAREHOUSE(FGUID,FID,FNAME,FCATEGORY,FPARENTID,FPROVINCE,FSTATUS,FSTARTTIME) values(@FGUID,@FID,@FNAME,@FCATEGORY,@FPARENTID,@FPROVINCE,@FSTATUS,@FSTARTTIME)";
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
