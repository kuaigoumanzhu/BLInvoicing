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
     public  class VIPINFOService : DBContext
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
        public bool AddVIPINFO(T_VIPINFOModel model)
        {

            string sql = @"insert into  T_VIPINFO(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FSEX, FAGE, FTEL, FMOBILE, FPLACE, FCONSUMPTION, FINTEGRAL
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FSEX, @FAGE, @FTEL, @FMOBILE, @FPLACE, @FCONSUMPTION, @FINTEGRAL
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    
        public bool EditVIPINFO(T_VIPINFOModel model)
        {

            string sql = @"update  T_VIPINFO set  FNAME=@FNAME, FSEX=@FSEX, FAGE=@FAGE, FTEL=@FTEL, FMOBILE=@FMOBILE, FPLACE=@FPLACE,  FCONSUMPTION=@FCONSUMPTION,  FINTEGRAL=@FINTEGRAL 
where FID=@FID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }
        /// <summary>
        /// 判断编号是否已存在
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsFID( string FID)
        {
            string sql = "select * from T_VIPINFO where FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_VIPINFOModel>(sql, new { FID = FID }).Count() > 0;
            }
        }
        public IEnumerable<T_VIPINFOModel> GetAllVIPINFO(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
                dp.Add("@tblName", "T_VIPINFO");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_VIPINFOModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

    }
}
