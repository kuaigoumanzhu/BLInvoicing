using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BL.Framework;
using BL.MVC;
using BL.Service;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Mvc;
using ZXing;
using ZXing.Common;
using BL.Models;
using BL.Framework.Orm;

namespace BL.Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        VIPINFOService vipInfoService = new VIPINFOService();
        protected void Button1_Click(object sender, EventArgs e)
        {
            T_VIPINFOModel model = new T_VIPINFOModel();
            model.FID= "6946589601204";
            model.FNAME = "zzz";
            model.FSEX = "男";
            model.FAGE = "26";
            model.FTEL = "12312365487";
            if (vipInfoService.IsExistsFID(model.FID))
            {
                 vipInfoService.EditVIPINFO(model);
            }
            else
            {
                model.FGUID = Guid.NewGuid().ToString();
                 vipInfoService.AddVIPINFO(model);
            }
        }
    }
}