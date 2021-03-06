﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;
using BL.Framework.Orm;
using System.Collections;

namespace BL.Service
{
    public class OtherOutService: DBContext
    {
        public IEnumerable<ViewOTHEROUTModel> GetAllOtherOutInfo(IDictionary paraDic,int pageIndex, int pageSize, out int total)
        {
            string whereStr = " 1=1 ";
            if (paraDic.Contains("FDate") && paraDic["FDate"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and datediff(day,FDATE,'{0}')=0", paraDic["FDate"].ToString());
            }
            if (paraDic.Contains("FCode") && paraDic["FCode"].ToString().Trim() != "")
            {
                whereStr += string.Format(" and FCode like '%{0}%'", paraDic["FCode"].ToString());
            }
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@tblName", "T_OTHEROUT a with(nolock) left join T_Person b with(nolock) on a.FAPPLYID = b.FID");
            dp.Add("@strWhere", whereStr);
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
                        return model;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<T_OTHEROUTDETAILSModel> GetAllOtherOutDetailsByParentId(string parentId)
        {
            string sql = @"select * from T_OTHEROUTDETAILS with(nolock) where FPARENTID=@parentId";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_OTHEROUTDETAILSModel>(sql, new { parentId = parentId });
            }
        }

        public IEnumerable<T_REPERTORYModel> GetGoodsInfoByIdAndBatchWare(string goodsId, string batch, string ware)
        {
            string sql = @"select * from T_REPERTORY with(nolock) where FGOODSID=@FGOODSID and FBATCH=@FBATCH and FWAREHOUSEID=@FWAREHOUSEID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query< T_REPERTORYModel>(sql, new { FGOODSID = goodsId, FBATCH = batch, FWAREHOUSEID = ware });
            }
        }

        public IEnumerable<T_REPERTORYModel> GetGoodsInfoByWare(string ware)
        {
            string sql = @"select * from T_REPERTORY with(nolock) where FWAREHOUSEID=@FWAREHOUSEID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_REPERTORYModel>(sql, new { FWAREHOUSEID = ware });
            }
        }

        public UiResponse AddOtherOutDetailInfo(List<T_OTHEROUTDETAILSModel> models,string wareId,string userName)
        {
            string sql = @"insert into T_OTHEROUTDETAILS(FGUID,FCREATEID,FCREATETIME,FPARENTID,FBATCH,FGOODSID,FGOODSNAME,FUNIT,FQUANTITY,FPRICE,FMONEY,FMEMO)
                        values(@FGUID,@FCREATEID,@FCREATETIME,@FPARENTID,@FBATCH,@FGOODSID,@FGOODSNAME,@FUNIT,@FQUANTITY,@FPRICE,@FMONEY,@FMEMO)";
            string updateSql = @"update T_REPERTORY set FSURPLUS=@FSURPLUS,FENABLE=@FENABLE where FGOODSID=@FGOODSID and FWAREHOUSEID=@FWAREHOUSEID and FBATCH=@FBATCH";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    var now = DateTime.Now;
                    foreach(var model in models)
                    {
                        db.Execute(updateSql, new { FSURPLUS = 0, FENABLE = 0, FGOODSID = model.FGOODSID, FWAREHOUSEID = wareId, FBATCH = model.FBATCH }, transaction);
                        model.FGUID = Guid.NewGuid().ToString();
                        model.FCREATEID = userName;
                        model.FCREATETIME = now;
                        if (db.Execute(sql, model, transaction) > 0)
                        {
                        }
                        else
                        {
                            transaction.Rollback();
                            return new UiResponse { statusCode = "300", message = "添加失败" };
                        }
                    }
                    transaction.Commit();
                    return new UiResponse { statusCode = "200", message = "添加成功" };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public UiResponse DelGoodsBackDetail(string guid)
        {
            string sql = @"delete from T_OTHEROUTDETAILS where FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                if(db.Execute(sql, new { FGUID = guid }) > 0)
                {
                    return new UiResponse { statusCode = "200", message = "删除成功" };
                }
                else
                {
                    return new UiResponse { statusCode = "300", message = "删除失败！" };
                }
            }
        }
    }
}
