using BL.Framework;
using BL.MVC;
using BL.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;

namespace BL.Web.Controllers
{
    public class CommonController : Controller
    {
        CommonService commonService = new CommonService();
        [JsonException]
        public string GetWareHoseJson()
        {
            var result = commonService.GetWareHouseSelect();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in result)
            {
                sb.Append("{\"" + item.FID + "\":\"" + item.FNAME + "\"},");
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1) + "]";
        }
        /// <summary>
        /// 分仓库存表数据带回
        /// </summary>
        /// <returns></returns>
        public ActionResult LookUpRepertoryChild(string inWareHouse)
        {
            var lst=commonService.GetRepertoryChildByInWareHouse(inWareHouse);
            return View(lst);
        }

        public ActionResult GetBarCodeImage(string text)
        {
            EncodingOptions options = new EncodingOptions { Width = 300, Height = 100 };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = options;
            Bitmap barCode = writer.Write(text);
            MemoryStream ms = new MemoryStream();
            barCode.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] bytes = ms.GetBuffer();
            ms.Close();
            return new FileContentResult(bytes, "image/jpeg");
        }
    }
}
