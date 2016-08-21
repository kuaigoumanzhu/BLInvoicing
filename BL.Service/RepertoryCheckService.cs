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
    public class RepertoryCheckService : DBContext
    {
        public IEnumerable<ViewREPERTORYCHECK> GetAllRepertoryCheckInfo(int pageIndex, int pageSize, out int total)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@tblName", "T_REPERTORYCHECK a with(nolock) left join T_Person b with(nolock) on a.FAPPLYID = b.FID");
            dp.Add("@strWhere", " 1=1 ");
            dp.Add("@fldName", "a.*,b.FNAME as FAPPLYName");
            dp.Add("@strOrder", "a.FCREATETIME desc");
            dp.Add("@PageSize", pageSize);
            dp.Add("@PageIndex", pageIndex);
            using (IDbConnection db = OpenConnection())
            {
                var result = db.QueryMultiple("sp_SplitPage_GetList", dp, null, null, CommandType.StoredProcedure);
                total = result.Read<int>().Single();
                return result.Read<ViewREPERTORYCHECK>();
            }
        }

        public ViewREPERTORYCHECK AddRepertoryCheck(ViewREPERTORYCHECK model, int id, int number, CommonService service)
        {
            string sql = @"insert into T_REPERTORYCHECK (FGUID,FCREATEID,FCREATETIME,FDATE,FCODE,
                                    FWAREHOUSEID,FMEMO,FSTATUS,
                                    FAPPLYID,FAPPLYTIME) values(@FGUID,@FCREATEID,@FCREATETIME,@FDATE,@FCODE,
                                    @FWAREHOUSEID,@FMEMO,@FSTATUS,
                                    @FAPPLYID,@FAPPLYTIME)";
            string getSql = @"select a.*,b.FNAME as FAPPLYName from T_REPERTORYCHECK a with(nolock)
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
                        var res = db.QuerySingle<ViewREPERTORYCHECK>(getSql, new { FGUID = model.FGUID }, transaction);
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

        public IEnumerable<T_REPERTORYCHECKDETAILSModel> GetAllRepertoryCheckDetailsInfo(string parentId, string wareHouse)
        {
            string sql = "select * from T_REPERTORY with(nolock) where FWAREHOUSEID=@wareHouse and FSURPLUS>0";
            using (IDbConnection db = OpenConnection())
            {
                var goodsWareHouse = db.Query<T_REPERTORYModel> (sql, new { wareHouse = wareHouse });
                IList<T_REPERTORYCHECKDETAILSModel> details = new List<T_REPERTORYCHECKDETAILSModel>();
                foreach (var model in goodsWareHouse)
                {
                    T_REPERTORYCHECKDETAILSModel detail = new T_REPERTORYCHECKDETAILSModel();
                    detail.FGOODSID = model.FGOODSID;
                    detail.FGOODSNAME = model.FGOODSNAME;
                    detail.FPARENTID = parentId;
                    detail.FQUANTITY = (float)model.FSURPLUS;
                    detail.FREALQUANTITY = (float)model.FSURPLUS;
                    detail.FUNIT = model.FUNIT;
                    detail.FDIFFERQUANTITY = detail.FQUANTITY - detail.FREALQUANTITY;
                    details.Add(detail);
                }
                return details;
            }
        }
        /// <summary>
        /// 添加库存明细
        /// </summary>
        /// <param name="details">明细</param>
        /// <param name="parentId">主表</param>
        /// <param name="userId">当前用户</param>
        /// <param name="wareId">仓库</param>
        /// <returns></returns>
        public UiResponse AddRepertoryCheckDetails(List<T_REPERTORYCHECKDETAILSModel> details,string parentId,string userId,string wareId)
        {
            string sql = @"insert into T_REPERTORYCHECKDETAILS(FGUID,FCREATEID,FCREATETIME,FPARENTID,FBARCODE,FGOODSID,FGOODSNAME,FUNIT,FQUANTITY,FREALQUANTITY,FDIFFERQUANTITY,FMEMO)
                            values(@FGUID,@FCREATEID,@FCREATETIME,@FPARENTID,@FBARCODE,@FGOODSID,@FGOODSNAME,@FUNIT,@FQUANTITY,@FREALQUANTITY,@FDIFFERQUANTITY,@FMEMO)";
            string updateSql = @"update T_REPERTORY set FSURPLUS=@FSURPLUS where FGOODSID=@FGOODSID and FWAREHOUSEID=@FWAREHOUSEID";
            string updateParent = @"update T_REPERTORYCHECK set FSTATUS=2,FAPPLYID=@FAPPLYID,FAPPLYTIME=@FAPPLYTIME";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    foreach(var model in details)
                    {
                        model.FGUID = Guid.NewGuid().ToString();
                        model.FCREATEID = userId;
                        model.FCREATETIME = DateTime.Now;
                        model.FPARENTID = parentId;
                        var bol = db.Execute(sql, model, transaction) > 0;
                        if(!bol)
                        {
                            transaction.Rollback();
                            return new UiResponse {statusCode="300" ,message = "保存失败", closeCurrent = true };
                        }
                        db.Execute(updateSql, new { FSURPLUS = model.FREALQUANTITY, FGOODSID=model.FGOODSID, FWAREHOUSEID = wareId }, transaction);
                    }
                    db.Execute(updateParent, new { FAPPLYID = userId, FAPPLYTIME = DateTime.Now });
                    transaction.Commit();
                    return new UiResponse { statusCode = "200", message = "保存成功", closeCurrent = true };
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
