using BL.Framework.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BL.Service
{
    public class GoodsService : DBContext
    {
        public IEnumerable<T_GOODSModel> GetAllGoodsInfo(IDictionary paraDic,ref int totalPage,int pageIndex=1,int pageSize=10)
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
                dp.Add("@tblName", "T_GOODS");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName","*");
                dp.Add("@strOrder","FCREATETIME desc");
                dp.Add("@PageSize",pageSize);
                dp.Add("@PageIndex",pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result= db.QueryMultiple("sp_SplitPage_GetList",dp,null,null,CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_GOODSModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_GOODSModel AddGoods(T_GOODSModel model)
        {

            string sql = @"insert into  T_GOODS(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FSTANDARD, FUNIT, FCALCTYPE, FCATEGORY, FISCONSUMABLES, FSTATUS, FSTARTTIME, FENDTIME, FMEMO
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FSTANDARD, @FUNIT, @FCALCTYPE, @FCATEGORY, @FISCONSUMABLES, @FSTATUS, @FSTARTTIME, @FENDTIME, @FMEMO
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GOODSModel>("select * from T_GOODS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public T_GOODSModel EditGoods(T_GOODSModel model)
        {

            string sql = @"update  T_GOODS set  FID=@FID, FNAME=@FNAME, FSTANDARD=@FSTANDARD, FUNIT=@FUNIT, FCALCTYPE=@FCALCTYPE, FCATEGORY=@FCATEGORY, FISCONSUMABLES=@FISCONSUMABLES,  FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GOODSModel>("select * from T_GOODS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        /// 判断编号是否已存在
        /// </summary>
        /// <param name="FGUID"></param>
        /// <param name="FID"></param>
        /// <returns></returns>
        public bool IsExistsFID(string FGUID, string FID)
        {
            string sql = "select * from T_GOODS where FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query < T_GOODSModel>(sql, new { FGUID = FGUID,FID=FID }).Count()>0;
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
