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
    public class CONSUMABLESService : DBContext
    {
        public IEnumerable<T_CONSUMABLESModel> GetAllCONSUMABLESInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
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
                dp.Add("@tblName", "T_CONSUMABLES");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_CONSUMABLESModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }
    }
}
