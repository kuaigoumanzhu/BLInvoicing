using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using System.Data;
using BL.Framework.Orm;
using Dapper;

namespace BL.Service
{
    public class CommonService: DBContext
    {
        /// <summary>
        /// 获取流水号及单据编号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetNumberAndCodeById(int id,out int number)
        {
            string sql = "select * from T_NUMBER with(nolock) where Id=@id";
            DateTime now = DateTime.Now;
            using (IDbConnection db = OpenConnection())
            {
                var model= db.QuerySingle<T_NUMBERModel>(sql, new { id = id });
                number = model.FNUMBER;
                return model.FConstant+"-"+now.Year.ToString() + now.Month.ToString().PadLeft(2,'0') + now.Day.ToString().PadLeft(2,'0') + "-" + (number+1).ToString().PadLeft(model.FPadLeft,'0');
            }
        }
        public bool UpdateNumberById(IDbConnection conn,IDbTransaction tran,int id,int number)
        {
            string sql = "update T_NUMBER set FNUMBER=@unumber where Id=@id and FNUMBER=@number";
            var unumber = number + 1;
            return conn.Execute(sql, new { unumber = unumber, id = id, number = number },tran) > 0;
        }
        /// <summary>
        /// 用户Id得到姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetNameById(string id)
        {
            string sql = "select FNAME from T_PERSON with(nolock) where FID=@id";
            using (IDbConnection db = OpenConnection())
            {
                return db.QuerySingle<string>(sql, new { id = id });
            }
        }

        public IEnumerable<SelectWareHouseModel> GetWareHouseSelect()
        {
            string sql = "select FID,FNAME from T_WAREHOUSE with(nolock)";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<SelectWareHouseModel>(sql);
            }
        }
        /// <summary>
        /// hpf 分仓库存表带回数据根据调入仓库即当前仓库
        /// </summary>
        /// <param name="inWareHouse">分仓调入仓库即当前分仓仓库</param>
        /// <returns></returns>
        public IEnumerable<T_REPERTORYCHILDModel> GetRepertoryChildByInWareHouse(string inWareHouse)
        {
            string sql = "select * from T_REPERTORYCHILD with(nolock) where FINWAREHOUSEID=@inWareHouse";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_REPERTORYCHILDModel>(sql, new { inWareHouse = inWareHouse });
            }
        }
    }
}
