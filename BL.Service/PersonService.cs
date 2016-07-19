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
    public class PersonService : DBContext
    {
        public IEnumerable<T_PERSONModel> GetAllPERSONInfo(IDictionary paraDic, ref int totalPage, int pageIndex = 1, int pageSize = 10)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " 1=1 ";
            if (paraDic.Contains("FID")&&paraDic["FID"].ToString().Trim()!="")
            {
                whereStr +=string.Format(" and FID like '%{0}%'",paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@tblName", " T_PERSON");
                dp.Add("@strWhere", whereStr);
                dp.Add("@fldName", "*");
                dp.Add("@strOrder", "FCREATETIME desc");
                dp.Add("@PageSize", pageSize);
                dp.Add("@PageIndex", pageIndex);
                //return db.Query<T_GOODSModel>(sql);
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                var resultPage = result.Read<Int32>();
                var resultGrid = result.Read<T_PERSONModel>();
                totalPage = resultPage.First();
                return resultGrid;
            }
        }

        public T_PERSONModel AddPERSON(T_PERSONModel model)
        {

            string sql = @"insert into   T_PERSON(FGUID, FCREATEID, FCREATETIME, FID, FNAME, FCOMPANYID, FCOMPANY, FDEPTID, FDEPT, FPOSTID, FPOST, FSTATUS, FSTARTTIME, FENDTIME, FFUNCURL
) values(@FGUID, @FCREATEID, @FCREATETIME, @FID, @FNAME, @FCOMPANYID, @FCOMPANY, @FDEPTID, @FDEPT, @FPOSTID, @FPOST, @FSTATUS,@FSTARTTIME, @FENDTIME, @FFUNCURL
)";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_PERSONModel>("select * from T_PERSON with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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

        public T_PERSONModel EditPERSON(T_PERSONModel model)
        {

            string sql = @"update   T_PERSON set   FID=@FID, FNAME=@FNAME, FCOMPANYID=@FCOMPANYID, FCOMPANY=@FCOMPANY, FDEPTID=@FDEPTID, FDEPT=@FDEPT, FPOSTID=@FPOSTID, FPOST=@FPOST
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_PERSONModel>("select * from  T_PERSON with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
            string sql = "select * from  T_PERSON where FGUID!=@FGUID and FID=@FID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_PERSONModel>(sql, new { FGUID = FGUID, FID = FID }).Count() > 0;
            }
        }

        public int DelPERSON(string FGUID)
        {

            string sql = @"delete   T_PERSON 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public bool SetPERSONStatusByGuid(string FGUID, string FSTATUS, DateTime now)
        {
            string sql = string.Empty;
            if (FSTATUS == "2")//设置启用
            {
                sql = "update  T_PERSON set  FSTATUS=@FSTATUS,FSTARTTIME=@time where FGUID=@FGUID";
            }
            else if (FSTATUS == "3")
            {
                sql = "update  T_PERSON set  FSTATUS=@FSTATUS,FENDTIME=@time where FGUID=@FGUID";
            }
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FSTATUS = FSTATUS, time = now, FGUID = FGUID }) > 0;
            }
        }
    }
}
