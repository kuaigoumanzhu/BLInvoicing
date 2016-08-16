using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;
using BL.Framework.Orm;

namespace BL.Service
{
    public class OtherOutService: DBContext
    {
        public IEnumerable<ViewOTHEROUTModel> GetAllOtherOutInfo(int pageIndex, int pageSize, out int total)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@tblName", "T_OTHEROUT a with(nolock) left join T_Person b with(nolock) on a.FAPPLYID = b.FID");
            dp.Add("@strWhere", " 1=1 ");
            dp.Add("@fldName", "a.*,b.FNAME as FAPPLYName");
            dp.Add("@strOrder", "a.FCREATETIME desc");
            dp.Add("@PageSize", pageSize);
            dp.Add("@PageIndex", pageIndex);
            using (IDbConnection db = OpenConnection())
            {
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                total = result.Read<int>().Single();
                return result.Read<ViewOTHEROUTModel>();
            }
        }

        public ViewOTHEROUTModel AddOtherOut(ViewOTHEROUTModel model,int id,int number,CommonService service)
        {
            string sql = @"insert into T_OTHEROUT (FGUID,FCREATEID,FCREATETIME,FDATE,FCODE,
                                    FWAREHOUSEID,FMEMO,FSTATUS,
                                    FAPPLYID,FAPPLYTIME) values(@FGUID,@FCREATEID,@FCREATETIME,@FDATE,@FCODE,
                                    @FWAREHOUSEID,@FMEMO,@FSTATUS,
                                    @FAPPLYID,@FAPPLYTIME)";
            string getSql = @"select a.*,b.FNAME as FAPPLYName from T_OTHEROUT a with(nolock)
                            left join T_Person b with(nolock) on a.FAPPLYID=b.FID
                            where a.FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    if (service.UpdateNumberById(db, transaction, id, number))
                    {
                        db.Execute(sql, model, transaction);
                        var res = db.QuerySingle<ViewOTHEROUTModel>(getSql, new { FGUID = model.FGUID }, transaction);
                        //res.FAPPLYName = service.GetNameById(model.FAPPLYID);
                        transaction.Commit();
                        res.closeCurrent = true;
                        res.message = "添加成功";
                        return res;
                    }
                    else
                    {
                        transaction.Rollback();
                        model.closeCurrent = true;
                        model.statusCode = "300";
                        model.message = "新增流水号已被占用，请重新保存！";
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
