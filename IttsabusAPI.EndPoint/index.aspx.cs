using IttsabusAPI.EndPoint.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IttsabusAPI.EndPoint
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Test(object sender, EventArgs e)
        {
            var text= FuncionesComunes.Encrypt("ITTSABUS_ENDPOINT");
        }
    }
}