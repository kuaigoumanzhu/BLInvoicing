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
    public class WARNINGService : DBContext
    {
        /// <summary>
        /// 获取销售预警信息
        /// </summary>
        /// <param name="paraDic"></param>
        /// <param name="totalPage"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<T_DATADICTModel> GetWarningInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            using (IDbConnection db = OpenConnection())
            {
                string whereStr = " 1=1 ";
                if (paraDic.Contains("FWAREHOUSEID") && paraDic["FWAREHOUSEID"].ToString().Trim() != "")
                {
                    whereStr += string.Format(" and FWAREHOUSEID='{0}'", paraDic["FWAREHOUSEID"].ToString());
                }
                if (paraDic.Contains("startDate") && paraDic["startDate"].ToString().Trim() != "")
                {
                    whereStr += string.Format(" and datediff(day,FENDTIME,'{0}')<=0", paraDic["startDate"].ToString());
                }
                if (paraDic.Contains("endDate") && paraDic["endDate"].ToString().Trim() != "")
                {
                    whereStr += string.Format("  and datediff(day,FENDTIME,'{0}')>=0", paraDic["endDate"].ToString());
                }
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", "T_WARNING");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_DATADICTModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public bool InserWarning(T_WARNINGModel model)
        {
            using (IDbConnection db = OpenConnection())
            {
                //开始事务
                var trans = db.BeginTransaction();
                var rows = db.Execute("delete from T_WARNING ",null, trans);
                if (rows > 0)
                {
                    rows = db.Execute("insert into T_WARNING (FGUID,FCREATEID,FCREATETIME,FWAREHOUSEID,FGOODSID,FGOODSNAME,FENDTIME) select NEWID(),@FCREATEID,@FCREATETIME,FINWAREHOUSEID,FGOODSID,FGOODSNAME,(select max(FDATE) from T_SALEDAYBOOK where FPARENTID=T_REPERTORYCHILD.FGUID) from T_REPERTORYCHILD", new { FCREATEID = model.FCREATEID, FCREATETIME = model.FCREATETIME }, trans);
                    if (rows > 0) 
                        trans.Commit();
                    else
                        trans.Rollback();
                }
                else
                    trans.Rollback();
                return rows > 0;
            }
        }
    }
}
