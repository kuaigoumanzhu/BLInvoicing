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
    public class GOODSALLOTDETAILSService : DBContext
    {
        public IEnumerable<T_GOODSALLOTDETAILSModel> GetAllGOODSALLOTDETAILSInfo(IDictionary paraDic)
        {
            //string sql = "select * from T_GOODS";
            string whereStr = " 1=1 ";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FID") && paraDic["FID"].ToString().Trim() != "")
            {
                whereStr += " and FID like '%'+@FID+'%'";// string.Format(" and FID like '%{0}%'", paraDic["FID"].ToString());
                dp.Add("@FID", paraDic["FID"].ToString());
            }
            if (paraDic.Contains("FNAME") && paraDic["FNAME"].ToString().Trim() != "")
            {
                whereStr += " and FNAME like '%'+@FNAME+'%'";// string.Format(" and FNAME like '%{0}%'", paraDic["FNAME"].ToString());
                dp.Add("@FNAME", paraDic["FNAME"].ToString());
            }
            if (paraDic.Contains("FPARENTID") && paraDic["FPARENTID"].ToString().Trim() != "")
            {
                whereStr += " and FPARENTID=@FPARENTID";// string.Format(" and FPARENTID=@FPARENTID", paraDic["FPARENTID"].ToString());
                dp.Add("@FPARENTID", paraDic["FPARENTID"].ToString());
            }
            using (IDbConnection db = OpenConnection())
            {
                string sqlstr = "select * from T_GOODSALLOTDETAILS where ";
                var result = db.Query<T_GOODSALLOTDETAILSModel>(sqlstr + whereStr, dp);
                return result;
            }
        }


        public bool AddGOODSALLOTDETAILS(List<T_GOODSALLOTDETAILSModel> list, string FINWAREHOUSEID)
        {

            string sql = "";
            var model = list[0];
            DynamicParameters dp = new DynamicParameters();
            for (int i = 0; i < list.Count(); i++)
            {
                sql += @"insert into  T_GOODSALLOTDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT,FCALCTYPE,FBATCH, FQUANTITY, FPRICE, FMONEY,FMARKETPRICE,FMARKETMONEY,FBARCODE, FMEMO
) values(@FGUID" + i + ", @FCREATEID" + i + ", @FCREATETIME" + i + ", @FPARENTID" + i + ", @FGOODSID" + i + ", @FGOODSNAME" + i + ", @FUNIT" + i + ", @FCALCTYPE" + i + ", @FBATCH" + i + ", @FQUANTITY" + i + ", @FPRICE" + i + ", @FMONEY" + i + ", @FMARKETPRICE" + i + ", @FMARKETMONEY" + i + ", @FBARCODE" + i + ", @FMEMO" + i + ");";
                dp.Add("@FGUID" + i, Guid.NewGuid().ToString());
                dp.Add("@FCREATEID" + i, list[0].FCREATEID);
                dp.Add("@FCREATETIME" + i, list[0].FCREATETIME);
                dp.Add("@FPARENTID" + i, list[0].FPARENTID);
                dp.Add("@FGOODSID" + i, list[i].FGOODSID);
                dp.Add("@FGOODSNAME" + i, list[i].FGOODSNAME);
                dp.Add("@FUNIT" + i, list[i].FUNIT);
                dp.Add("@FCALCTYPE" + i, list[i].FCALCTYPE);
                dp.Add("@FBATCH" + i, list[i].FBATCH);
                dp.Add("@FQUANTITY" + i, list[i].FQUANTITY);
                dp.Add("@FPRICE" + i, list[i].FPRICE);
                dp.Add("@FMONEY" + i, list[i].FQUANTITY * list[i].FPRICE);//list[i].FMONEY
                dp.Add("@FMARKETPRICE" + i, list[i].FMARKETPRICE);
                dp.Add("@FMARKETMONEY" + i, list[i].FQUANTITY * list[i].FMARKETPRICE);//list[i].FMARKETMONEY
                dp.Add("@FBARCODE" + i, FINWAREHOUSEID+" "+list[i].FGOODSID.ToString() + " " + list[i].FMARKETPRICE.ToString() + " " + list[i].FQUANTITY.ToString());//list[i].FBARCODE
                dp.Add("@FMEMO" + i, list[i].FMEMO);

            }
            sql += "update T_REPERTORY set T_REPERTORY.FENABLE=T_REPERTORY.FENABLE-a.qcou from (select sum(g.FQUANTITY) qcou,g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID from T_GOODSALLOTDETAILS g inner join T_GOODSALLOT p on p.FGUID=g.FPARENTID where p.FGUID=@FPARENTID0 group by g.FBATCH,g.FGOODSID,p.FOUTWAREHOUSEID)a where T_REPERTORY.FBATCH=a.FBATCH and T_REPERTORY.FWAREHOUSEID=a.FOUTWAREHOUSEID and T_REPERTORY.FGOODSID=a.FGOODSID";
            using (IDbConnection db = OpenConnection())
            {
                var trans = db.BeginTransaction();
                if (db.Execute(sql, dp, trans) > 0)
                {
                    trans.Commit();
                    return true;
                }
                else
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
//        public T_GOODSALLOTDETAILSModel AddGOODSALLOTDETAILS(T_GOODSALLOTDETAILSModel model)
//        {

//            string sql = @"insert into  T_GOODSALLOTDETAILS(FGUID, FCREATEID, FCREATETIME, FPARENTID, FGOODSID, FGOODSNAME, FUNIT, FCALCTYPE, FQUANTITY, FMONEY, FPRICE, FSUPPLIERID, FMEMO
//) values(@FGUID, @FCREATEID, @FCREATETIME, @FPARENTID, @FGOODSID, @FGOODSNAME, @FUNIT, @FCALCTYPE, @FQUANTITY, @FMONEY, @FPRICE, @FSUPPLIERID, @FMEMO
//)";
//            using (IDbConnection db = OpenConnection())
//            {

//                if (db.Execute(sql, model) > 0)
//                {
//                    var res = db.QuerySingle<T_GOODSALLOTDETAILSModel>("select * from T_GOODSALLOTDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
//                    res.closeCurrent = true;
//                    res.message = "添加成功";
//                    return res;
//                }
//                else
//                {
//                    model.closeCurrent = true;
//                    model.statusCode = "300";
//                    model.message = "添加失败";
//                    return model;
//                }
//            }
//        }

        public T_GOODSALLOTDETAILSModel EditGOODSALLOTDETAILS(T_GOODSALLOTDETAILSModel model)
        {

            string sql = @"update  T_GOODSALLOTDETAILS set   FGOODSID=@FGOODSID, FGOODSNAME=@FGOODSNAME, FUNIT=@FUNIT, FQUANTITY=@FQUANTITY, FPRICE=@FPRICE, FMONEY=@FMONEY, FSUPPLIERID=@FSUPPLIERID, FMEMO=@FMEMO
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                if (db.Execute(sql, model) > 0)
                {
                    var res = db.QuerySingle<T_GOODSALLOTDETAILSModel>("select * from T_GOODSALLOTDETAILS with(nolock) where FGUID=@FGUID", new { FGUID = model.FGUID });
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
        public int DelGOODSALLOTDETAILS(string FGUID)
        {

            string sql = @"delete  T_GOODSALLOTDETAILS 
where FGUID=@FGUID ";
            using (IDbConnection db = OpenConnection())
            {
                return db.Execute(sql, new { FGUID = FGUID });
            }
        }
        public IEnumerable<object> GetSelectOutGoods(IDictionary paraDic)
        {
            string whereStr = " ";
            DynamicParameters dp = new DynamicParameters();
            if (paraDic.Contains("FWAREHOUSEID") && paraDic["FWAREHOUSEID"].ToString().Trim() != "")
            {
                whereStr += " and r.FWAREHOUSEID=@FWAREHOUSEID";
                dp.Add("@FWAREHOUSEID", paraDic["FWAREHOUSEID"].ToString());

            }
            if (paraDic.Contains("FDATE") && paraDic["FDATE"].ToString().Trim() != "")
            {
                dp.Add("@FDATE", paraDic["FDATE"].ToString());

            }
            //string sqlstr = @"select * from T_REPERTORY where FSURPLUS>0";
            string sqlstr = @"select r.*,isnull(a.FMARKETPRICE,0) FMARKETPRICE from T_REPERTORY r
inner join (
select g.FDATE,g.FWAREHOUSEID,gd.* from T_GUIDANCE g
inner join T_GUIDANCEDETAILS gd on g.FGUID=gd.FPARENTID
where datediff(day,g.FDATE,@FDATE)=0
) a on r.FGOODSID=a.FGOODSID and a.FWAREHOUSEID=r.FWAREHOUSEID  where FSURPLUS>0 " +whereStr;

            using (IDbConnection db = OpenConnection())
            {
                var result = db.Query<object>(sqlstr, dp).AsList();
                return result;
            }

        }
    }
}
