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
                total = result.Read<int>().SingleOrDefault();
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
        /// <param name="parentId">商品回库主表</param>
        /// <param name="inWareHouse">商品分仓库</param>
        /// <returns></returns>
        public IEnumerable<T_GOODSBACKDETAILSModel> GetAllGoodsBackDetailsInfo(string parentId,string inWareHouse, string wareHouse)
        {
            string sql = "select * from T_GOODSBACK where FGUID=@parentId";
            string sqlWareHouse = @"select min(FOUTWAREHOUSEID) as FOUTWAREHOUSEID,min(FINWAREHOUSEID) FINWAREHOUSEID,min(FBARCODE) as FBARCODE,
            min(FGOODSID) as FGOODSID,min(FGOODSNAME) as FGOODSNAME,min(FUNIT) as FUNIT,SUM(FQUANTITY) as FQUANTITY,SUM(FSURPLUS) as FSURPLUS,
            SUM(FENABLE) as FENABLE,min(FPRICE) as FPRICE,
            (select b.FMARKETPRICE from T_GUIDANCE a with(nolock) left join T_GUIDANCEDETAILS b with(nolock) on a.FGUID=b.FPARENTID where a.FWAREHOUSEID=@wareHouse and a.FDATE=convert(varchar(10),getdate(),120) and b.FGOODSID=min(c.FGOODSID)) as FMARKETPRICE
            from T_REPERTORYCHILD c with(nolock) where FINWAREHOUSEID=@inWareHouse group by FBARCODE";
            using (IDbConnection db = OpenConnection())
            {
                var goodsBack = db.QuerySingle<T_GOODSBACKModel>(sql, new { parentId = parentId });
                if (goodsBack.FSTATUS != "1")
                {
                    return db.Query<T_GOODSBACKDETAILSModel>("select * from T_GOODSBACKDETAILS where FPARENTID=@parentId", new { parentId = parentId });
                }
                else
                {
                    var goodsWareHouse = db.Query<T_REPERTORYCHILDModel>(sqlWareHouse, new { inWareHouse = inWareHouse, wareHouse = wareHouse });
                    IList<T_GOODSBACKDETAILSModel> details = new List<T_GOODSBACKDETAILSModel>();
                    foreach (var model in goodsWareHouse)
                    {
                        T_GOODSBACKDETAILSModel detail = new T_GOODSBACKDETAILSModel();
                        detail.FACTUALQUANTITY = 0;
                        detail.FBARCODE = model.FBARCODE;
                        detail.FBATCH = model.FBATCH;
                        detail.FDIFFERMONEY = float.Parse((model.FSURPLUS * model.FPRICE).ToString("f2"));//差异金额
                        detail.FDIFFERQUANTITY = model.FSURPLUS;//差异数量
                        detail.FGOODSID = model.FGOODSID;
                        detail.FGOODSNAME = model.FGOODSNAME;
                        detail.FMARKETPRICE = model.FMARKETPRICE;//销售单价
                        detail.FPRICE = model.FPRICE;//单价
                        detail.FQUANTITY = model.FSURPLUS;//账存数量，剩余数量
                        detail.FUNIT = model.FUNIT;//单位
                        details.Add(detail);
                    }
                    return details;
                }
                //return db.Query<T_GOODSBACKDETAILSModel>(sql, new { parentId = parentId });
            }
        }
        /// <summary>
        /// 保存商品回库信息，修改分仓库存表可用数量
        /// </summary>
        public IEnumerable<T_GOODSBACKDETAILSModel> AddGoodsBackDetailUpdateChild(IList<T_GOODSBACKDETAILSModel> lst,string userId, string parentId, string outWare)
        {
            string sql = @"insert into T_GOODSBACKDETAILS(FGUID,FCREATEID,FCREATETIME,FPARENTID,FGOODSID,FGOODSNAME,FUNIT,
                            FQUANTITY,FPRICE,FMARKETPRICE,FDIFFERQUANTITY,FDIFFERMONEY,FBARCODE,FMEMO) values(@FGUID,
                            @FCREATEID,@FCREATETIME,@FPARENTID,@FGOODSID,@FGOODSNAME,@FUNIT,
                            @FQUANTITY,@FPRICE,@FMARKETPRICE,@FDIFFERQUANTITY,@FDIFFERMONEY,@FBARCODE,@FMEMO)";
            string getsql = @"select * from T_GOODSBACKDETAILS with(nolock) where FGUID=@FGUID";
            string getChildSql = @"select * from T_REPERTORYCHILD where FBARCODE=@FBARCODE and FINWAREHOUSEID=@outWare";
            string updatesql = @"update T_REPERTORYCHILD set FENABLE=@current where FBARCODE=@FBARCODE and FENABLE=@FENABLE and FGUID=@FGUID";
            string updateStatus = @"update T_GOODSBACK set FSTATUS='3' where FGUID=@parentId";
            var now = DateTime.Now;
            IList<T_GOODSBACKDETAILSModel> models = new List<T_GOODSBACKDETAILSModel>();
            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    var goodsBack=db.QuerySingle<T_GOODSBACKModel>("select * from T_GOODSBACK where FGUID=@parentId", new { parentId = parentId }, transaction);
                    if(goodsBack.FSTATUS=="2")
                    {
                        //transaction.Rollback();
                        throw new Exception("已经提交无法再保存！");
                    }
                    else if(goodsBack.FSTATUS=="3")
                    {
                        //transaction.Rollback();
                        throw new Exception("已经保存无法再保存！");
                    }

                    foreach(var model in lst)
                    {
                        model.FGUID = Guid.NewGuid().ToString();
                        model.FCREATEID = userId;
                        model.FCREATETIME = now;
                        model.FPARENTID = parentId;
                        db.Execute(sql, model, transaction);
                        models.Add(db.QuerySingle<T_GOODSBACKDETAILSModel>(getsql, new { FGUID = model.FGUID },transaction));
                        //获取分仓库存可用数量
                        var childModels = db.Query<T_REPERTORYCHILDModel>(getChildSql, new { FBARCODE = model.FBARCODE, outWare= outWare },transaction);
                        var fenables = model.FACTUALQUANTITY;
                        var number = 0f;
                        foreach(var childModel in childModels)
                        {
                            if (childModel.FENABLE >= fenables)//如果可用数量大于实际总数直接减去
                            {
                                number = childModel.FENABLE - fenables;
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = number, FENABLE = childModel.FENABLE, FGUID=childModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("可用数量已被其他人修改过！请重新保存");
                                }
                            }
                            else
                            {
                                fenables = fenables - childModel.FENABLE;//总数减去当前批次可用数量
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = 0, FENABLE = childModel.FENABLE, FGUID = childModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("可用数量已被其他人修改过！请重新保存");
                                }
                            }
                        }
                    }
                    db.Execute(updateStatus, new { parentId = parentId }, transaction);
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
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public string GetGoodsBackStatusByParentId(string parentId)
        {
            string sql = "select FSTATUS from T_GOODSBACK  with(nolock) where FGUID=@FGUID";
            using (IDbConnection db = OpenConnection())
            {
                return db.Query<string>(sql, new { FGUID = parentId }).Single();
            }
        }

        public bool ApplayGoodsBackDetail(IList<T_GOODSBACKDETAILSModel> lst,string parentId, string outWare)
        {
            string goodsBacksql = "select * from T_GOODSBACK with(nolock) where FGUID=@parentId";//获取调出仓库FOUTWAREHOUSEID
            string getChildSql = @"select * from T_REPERTORYCHILD where FBARCODE=@FBARCODE and FINWAREHOUSEID=@outWare";
            string updatesql = @"update T_REPERTORYCHILD set FSURPLUS=@current where FBARCODE=@FBARCODE and FSURPLUS=@FSURPLUS and FGUID=@FGUID";
            string updateStatus = @"update T_GOODSBACK set FSTATUS='2' where FGUID=@parentId";
            string repertorysql = @"select * from T_REPERTORY where FGOODSID=@FGOODSID";
            string updateKcSql = @"update T_REPERTORY set FSURPLUS=@current,FENABLE=@current1 where FBARCODE=@FBARCODE and FSURPLUS=@FSURPLUS and FENABLE=@FENABLE and FGUID=@FGUID";
            //分仓库存表（T_ REPERTORYCHILD）调入仓库=商品回库调出仓库

            //库存表（T_ REPERTORY）FWAREHOUSEID仓库=分仓库存表的调出仓库（FOUTWAREHOUSEID）

            using (IDbConnection db = OpenConnection())
            {
                IDbTransaction transaction = db.BeginTransaction();
                try
                {
                    foreach (var model in lst)
                    {
                        //获取分仓库存可用数量
                        var childModels = db.Query<T_REPERTORYCHILDModel>(getChildSql, new { FBARCODE = model.FBARCODE, outWare = outWare }, transaction);
                        var fenables = model.FACTUALQUANTITY;
                        var number = 0f;
                        foreach (var childModel in childModels)
                        {
                            if (childModel.FSURPLUS >= fenables)//如果可用数量大于实际总数直接减去
                            {
                                number = childModel.FSURPLUS - fenables;
                                fenables = 0;
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = number, FSURPLUS = childModel.FSURPLUS, FGUID = childModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("可用数量已被其他人修改过！请重新保存");
                                }
                            }
                            else
                            {
                                fenables = fenables - childModel.FSURPLUS;//总数减去当前批次可用数量
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = 0, FSURPLUS = childModel.FSURPLUS, FGUID = childModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("可用数量已被其他人修改过！请重新保存");
                                }
                            }
                        }
                        var zs = model.FACTUALQUANTITY;
                        var synumber = 0f;
                        var kcModels = db.Query<T_REPERTORYModel>(repertorysql, new { FGOODSID = model.FGOODSID });
                        foreach(var kcModel in kcModels)
                        {
                            if ((float)kcModel.FSURPLUS >= zs)//如果可用数量大于实际总数直接减去
                            {
                                synumber = (float)kcModel.FSURPLUS - zs;
                                zs = 0;
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = synumber, current1= synumber, FSURPLUS = kcModel.FSURPLUS, FENABLE=kcModel.FENABLE, FGUID = kcModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("数量已被其他人修改过！请重新保存");
                                }
                            }
                            else
                            {
                                zs = zs - (float)kcModel.FSURPLUS;//总数减去当前批次可用数量
                                if (db.Execute(updatesql, new { FBARCODE = model.FBARCODE, current = 0, current1 = 0, FSURPLUS = kcModel.FSURPLUS, FENABLE = kcModel.FENABLE, FGUID = kcModel.FGUID }, transaction) <= 0)
                                {
                                    //transaction.Rollback();
                                    throw new Exception("数量已被其他人修改过！请重新保存");
                                }
                            }
                        }
                    }
                    db.Execute(updateStatus, new { parentId = parentId }, transaction);
                    transaction.Commit();
                    return true;
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
