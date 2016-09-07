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
        public bool AddVIPINFOO(T_VIPINFOModel model)
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

    
        public bool EditVIPINFOO(T_VIPINFOModel model)
        {

            string sql = @"update  T_VIPINFO set  FID=@FID, FNAME=@FNAME, FSEX=@FSEX, FAGE=@FAGE, FTEL=@FTEL, FMOBILE=@FMOBILE, FPLACE=@FPLACE,  FCONSUMPTION=@FCONSUMPTION,  FINTEGRAL=@FINTEGRAL 
where FGUID=@FGUID ";
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
        public bool IsExistsFID(string FGUID, string FID)
        {
            string sql = "select * from T_VIPINFO where FGUID!=@FGUID and FID!=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_VIPINFOModel>(sql, new { FGUID = FGUID, FID = FID }).Count() > 0;
            }
        }

    }
}
