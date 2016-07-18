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
    public class DATADICTService : DBContext
    {
        /// <summary>
        /// 获取字典分类信息
        /// </summary>
        /// <param name="paraDic"></param>
        /// <param name="totalPage"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<T_DATADICTModel> GetAllDictCategoryInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            using (IDbConnection db = OpenConnection())
            {
                string whereStr = " 1=1 ";
                if (paraDic.Contains("FCATEGORY"))
                {
                    whereStr += string.Format(" and FCATEGORY='{0}'", paraDic["FCATEGORY"].ToString());
                }
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_DATADICT");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_DATADICTModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public IEnumerable<T_DATADICTModel> GetAllDictCategoryInfo()
        {
            string sql = "select * from T_DATADICT where FCATEGORY='数据字典类别'";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_DATADICTModel>(sql);
            }
        }

        /// <summary>
        /// 判断字典类别编号是否已存在
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsCategory(string FGUID, string FID)
        {
            string sql = "select * from T_DATADICT where FCATEGORY='数据字典类别' and FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_DATADICTModel>(sql, new { FGUID=FGUID,FID=FID}).Count() > 0;
            }
        }
        /// <summary>
        /// 判断同一类别下字典编号是否重复
        /// </summary>
        /// <param name="FCATEGORY"></param>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsDICT(string FCATEGORY, string FGUID, string FID)
        {
            string sql = "select * from T_DATADICT where FCATEGORY=@FCATEGORY and FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_DATADICTModel>(sql, new {FCATEGORY=FCATEGORY, FGUID = FGUID, FID = FID }).Count() > 0;
            }
        }


        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="paraDic"></param>
        /// <param name="totalPage"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<T_DATADICTModel> GetAllDATADICTInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            using (IDbConnection db = OpenConnection())
            {
                string whereStr = " 1=1 ";
                whereStr += string.Format(" and FCATEGORY!='{0}'", "数据字典类别");
                if (paraDic.Contains("FNAME"))
                {
                    whereStr += string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
                }
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_DATADICT");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_DATADICTModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_DATADICTModel AddDATADICT(T_DATADICTModel model)
        {

            string sql = @"insert into  T_DATADICT(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FCATEGORY, FPARENTID, FORDER, FSTATUS, FSTARTTIME, FENDTIME, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FCATEGORY, @FPARENTID, @FORDER, @FSTATUS, @FSTARTTIME, @FENDTIME, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_DATADICTModel>("select * from T_DATADICT with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public T_DATADICTModel EditDATADICT(T_DATADICTModel model)
        {

            string sql = @"update  T_DATADICT set  FID=@FID, FNAME=@FNAME, FCATEGORY=@FCATEGORY, FPARENTID=@FPARENTID, FORDER=@FORDER, FSTATUS=@FSTATUS, FSTARTTIME=@FSTARTTIME, FENDTIME=@FENDTIME, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_DATADICTModel>("select * from T_DATADICT with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public int DelDATADICT(string FGUID)
        {

            string sql = @"delete  T_DATADICT 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public bool SetDATADICTStatusByGuid(string FGUID, string FSTATUS, DateTime now)
        {
            string sql = string.Empty;
            if (FSTATUS == "2")//设置启用
            {
                sql = "update T_DATADICT set  FSTATUS=@FSTATUS,FSTARTTIME=@time where FGUID=@FGUID";
            }
            else if (FSTATUS == "3")
            {
                sql = "update T_DATADICT set  FSTATUS=@FSTATUS,FENDTIME=@time where FGUID=@FGUID";
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FSTATUS = FSTATUS, time = now, FGUID = FGUID }) > 0;
            }
        }
    }
}
