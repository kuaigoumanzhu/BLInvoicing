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
    public class SUPPLIERService : DBContext
    {
        public IEnumerable<T_SUPPLIERModel> GetAllSUPPLIERInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
            if (paraDic.Contains("FCATEGORY") && paraDic["FCATEGORY"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FCATEGORY='{0}'", paraDic["FCATEGORY"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_SUPPLIER");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_SUPPLIERModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_SUPPLIERModel AddSUPPLIER(T_SUPPLIERModel model)
        {

            string sql = @"insert into  T_SUPPLIER(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FADDRESS, FTEL, FPROVINCE, FCATEGORY, FSTATUS, FSTARTTIME, FENDTIME, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FADDRESS, @FTEL, @FPROVINCE, @FCATEGORY, @FSTATUS, @FSTARTTIME, @FENDTIME, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_SUPPLIERModel>("select * from T_SUPPLIER with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public T_SUPPLIERModel EditSUPPLIER(T_SUPPLIERModel model)
        {

            string sql = @"update  T_SUPPLIER set  FID=@FID, FNAME=@FNAME, FADDRESS=@FADDRESS, FTEL=@FTEL, FPROVINCE=@FPROVINCE, FCATEGORY=@FCATEGORY, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_SUPPLIERModel>("select * from T_SUPPLIER with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        /// <summary>
        /// 获取编号是否存在
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsFID(string FGUID, string FID)
        {
            string sql = "select * from T_SUPPLIER where FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_SUPPLIERModel>(sql, new { FGUID = FGUID, FID = FID }).Count() > 0;
            }
        }
        public int DelSUPPLIER(string FGUID)
        {

            string sql = @"delete  T_SUPPLIER 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public bool SetSUPPLIERStatusByGuid(string FGUID, string FSTATUS, DateTime now)
        {
            string sql = string.Empty;
            if (FSTATUS == "2")//设置启用
            {
                sql = "update T_SUPPLIER set  FSTATUS=@FSTATUS,FSTARTTIME=@time where FGUID=@FGUID";
            }
            else if (FSTATUS == "3")
            {
                sql = "update T_SUPPLIER set  FSTATUS=@FSTATUS,FENDTIME=@time where FGUID=@FGUID";
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FSTATUS = FSTATUS, time = now, FGUID = FGUID }) > 0;
            }
        }

        public IEnumerable<T_SUPPLIERModel> GetSupplierInfo(IDictionary paraDic)
        {

            string sql = "select * from T_SUPPLIER with(nolock)";
            string whereStr = " where 1=1";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += " and FID = @FID";
                dp.Add("@FID", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FSTATUS") && paraDic["FSTATUS"].ToString().Trim() != "")
            {
                whereStr += " and FSTATUS = @FSTATUS";
                dp.Add("@FSTATUS", paraDic["FSTATUS"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_SUPPLIERModel>(sql + whereStr, dp);
            }
        }
    }
}
