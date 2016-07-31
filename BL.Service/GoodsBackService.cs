using BL.Framework.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.Models;
using Dapper;
using System.Data;

namespace BL.Service
{
    public class GoodsBackService: DBContext
    {
        public IEnumerable<ViewGOODSBACKModel> GetAllGoodsBackInfo(int pageIndex, int pageSize,out int total)
        {
            //string sql = @"select a.*,b.FNAME as FAPPLYName from T_GOODSBACK a with(nolock)
            //                left join T_Person b with(nolock) on a.FAPPLYID=b.FID";
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@tblName", "T_GOODSBACK a with(nolock) left join T_Person b with(nolock) on a.FAPPLYID = b.FID");
            dp.Add("@strWhere", " 1=1 ");
            dp.Add("@fldName", "a.*,b.FNAME as FAPPLYName");
            dp.Add("@strOrder", "a.FCREATETIME desc");
            dp.Add("@PageSize", pageSize);
            dp.Add("@PageIndex", pageIndex);
            using (IDbConnection db = OpenConnection())
            {
                var result=db.QueryMultiple("sp_SplitPage_GetList", dp, null,null,CommandType.StoredProcedure);
                total = result.Read<int>().Single();
                return result.Read<ViewGOODSBACKModel>();
            }
        }

        public ViewGOODSBACKModel AddGoodsBack(ViewGOODSBACKModel model,int id,int number,CommonService service)
        {
            string sql = @"insert into T_GOODSBACK (FGUID,FCREATEID,FCREATETIME,FDATE,FCODE,
                                    FOUTWAREHOUSEID,FINWAREHOUSEID,FMEMO,FSTATUS,
                                    FAPPLYID,FAPPLYTIME) values(@FGUID,@FCREATEID,@FCREATETIME,@FDATE,@FCODE,
                                    @FOUTWAREHOUSEID,@FINWAREHOUSEID,@FMEMO,@FSTATUS,
                                    @FAPPLYID,@FAPPLYTIME)";
            string getSql = @"select a.*,b.FNAME as FAPPLYName from T_GOODSBACK a with(nolock)
                            left join T_Person b with(nolock) on a.FAPPLYID=b.FID
                            where a.FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    if (service.UpdateNumberById(db,transaction,id, number))
                    {
                        db.Execute(sql, model,transaction);
                        var res = db.QuerySingle<ViewGOODSBACKModel>(getSql, new { FGUID = model.FGUID },transaction);
                        res.FAPPLYName = service.GetNameById(model.FAPPLYID);
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
        /// <summary>
        /// 获取商品回库明细表（根据主表主键）
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<T_GOODSBACKDETAILSModel> GetAllGoodsBackDetailsInfo(string parentId)
        {
            string sql = "select * from T_GOODSBACKDETAILS where FPARENTID=@parentId";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<T_GOODSBACKDETAILSModel>(sql, new { parentId = parentId });
            }
        }
        /// <summary>
        /// 保存商品回库信息，修改分仓库存表可用数量
        /// </summary>
        public IEnumerable<T_GOODSBACKDETAILSModel> AddGoodsBackDetailUpdateChild(IList<T_GOODSBACKDETAILSModel> lst,string userId)
        {
            string sql = @"insert into T_GOODSBACKDETAILS(FGUID,FCREATEID,FCREATETIME,FPARENTID,FGOODSID,FGOODSNAME,FUNIT,FBATCH,
                            FQUANTITY,FACTUALQUANTITY,FPRICE,FMARKETPRICE,FDIFFERQUANTITY,FDIFFERMONEY,FBARCODE,FMEMO) values(@FGUID,
                            @FCREATEID,@FCREATETIME,@FPARENTID,@FGOODSID,@FGOODSNAME,@FUNIT,@FBATCH,
                            @FQUANTITY,@FACTUALQUANTITY,@FPRICE,@FMARKETPRICE,@FDIFFERQUANTITY,@FDIFFERMONEY,@FBARCODE,@FMEMO)";
            string getsql = @"select * from T_GOODSBACKDETAILS with(nolock) where FGUID=@FGUID";
            string getChildSql = @"select * from T_REPERTORYCHILD where FBARCODE=@FBARCODE";
            string updatesql = @"update T_REPERTORYCHILD set FENABLE=@current where FBARCODE=@FBARCODE and FENABLE=@FENABLE";
            var now = DateTime.Now;
            IList<T_GOODSBACKDETAILSModel> models = new List<T_GOODSBACKDETAILSModel>();
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    foreach(var model in lst)
                    {
                        model.FGUID = Guid.NewGuid().ToString();
                        model.FCREATEID = userId;
                        model.FCREATETIME = now;
                        db.Execute(sql, model, transaction);
                        models.Add(db.QuerySingle<T_GOODSBACKDETAILSModel>(getsql, new { FGUID = model.FGUID },transaction));
                        //获取分仓库存可用数量
                        var childModel = db.QuerySingle<T_REPERTORYCHILDModel>(getChildSql, new { FBARCODE = model.FBARCODE },transaction);
                        var number = childModel.FENABLE - model.FACTUALQUANTITY;
                        if (number < 0)
                        {
                            transaction.Rollback();
                            throw new Exception("可用数量小于0！");
                        }
                        if(db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current=number, FENABLE = childModel.FENABLE },transaction)<=0)
                        {
                            transaction.Rollback();
                            throw new Exception("可用数量已被其他人修改过！请重新保存");
                        }
                    }
                    transaction.Commit();
                    return models;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        
        public bool DelGoodsBackDetail(string guid)
        {
            string sql = "delete from T_GOODSBACKDETAILS where FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = guid }) > 0;
            }
        }
        public bool GetGoodsBackByParentId(string parentId)
        {
            string sql = "select count(*) from T_GOODSBACK  with(nolock) where FGUID=@FGUID and FSTATUS='1'";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<int>(sql,new { FGUID=parentId}).Single() > 0;
            }
        }

        public void ApplayGoodsBackDetail(IList<T_GOODSBACKDETAILSModel> lst,string parentId)
        {
            string goodsBacksql = "select * from T_GOODSBACK with(nolock) where FGUID=@parentId";//获取调出仓库FOUTWAREHOUSEID
            //分仓库存表（T_ REPERTORYCHILD）调入仓库=商品回库调出仓库

            //库存表（T_ REPERTORY）FWAREHOUSEID仓库=分仓库存表的调出仓库（FOUTWAREHOUSEID）
        }
    }
}
