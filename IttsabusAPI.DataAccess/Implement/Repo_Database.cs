using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.DataAccess
{
    public class Repo_Database
    {
        public static string EjecutaCmd(string sCnn, string sCmd)
        {
            using (var cnn = new SqlConnection(sCnn))
            {
                sCmd = sCmd.Replace("/", "\"").Replace("~", "/");
                cnn.Open();
                SqlDataAdapter adpter = new SqlDataAdapter(sCmd, cnn);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                adpter.Fill(dt);

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dt);

                return JSONresult;
            }
        }
    }
}
