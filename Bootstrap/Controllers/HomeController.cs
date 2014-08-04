using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace Bootstrap.Controllers
{
    public class HomeController : Controller
    {
        public static string[,] leaderboard = new string[20,6];

        public ActionResult Index()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    leaderboard[i,j] = "";
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserInfo()
        {
            return View("~/Views/User/User.cshtml");
        }

        public static void getLeaderboard()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
            {
                throw new Exception("Fatal error: missing connection string in web.config file");
            }
            string conString = mySetting.ConnectionString;

            SqlConnection sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
            string query = String.Format("SELECT" +
                                         "[timestamp]," +
                                         "[companyId]," +
                                         "[totalStep]," +
                                         "[runStep]," +
                                         "[walkStep]," +
                                         "[calories]," +
                                         "[distance]" +
                                         "FROM [dbo].[Leaderboard]" +
                                         "ORDER BY [timestamp] DESC, [totalStep] DESC");
            string cols = string.Empty;
            SqlCommand queryCommand = new SqlCommand(query, sqlConnection);

            SqlDataReader queryReader = queryCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(queryReader);

            for (int i = 0; i < 20; i++)
            {
                int count = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (count != 0)
                    {
                        try
                        {
                            leaderboard[i, count - 1] += dataTable.Rows[i][col.ColumnName];
                        }
                        catch
                        {
                            leaderboard[i, count - 1] += "";
                        }
                    }
                    count++;
                }
            }
            sqlConnection.Close();
        }

    }
}