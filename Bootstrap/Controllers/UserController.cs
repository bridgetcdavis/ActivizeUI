using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Azure.Zumo.Test.Helper;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Bootstrap.Models;
using System.Security.Principal;

namespace Bootstrap.Controllers
{
    public class UserController : Controller
    {
        private const string SQLsForCustomerQuery = @"
            SELECT
            [Timestamp],
            [Id],
            [Steps],
            [Calories],
            [Minutes],
            [Miles]
            FROM [dbo].[FinalDaily]
            where [Timestamp] >= @start AND [Timestamp] <= @end
            and [Id] = @namespace
            order by [Timestamp]";

        private static System.Timers.Timer aTimer;
        public static int count = 1;

        //
        // GET: /User/
        public new ActionResult User()
        {
//            aTimer = new System.Timers.Timer(1000); //1 second
//            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
//
//            aTimer.AutoReset = true;
//            aTimer.Enabled = true;
//
//            Console.WriteLine("TEST");

            return View();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString);

            sqlConnection.Open();
            SqlCommand cmd = sqlConnection.CreateCommand();

            cmd.CommandText = String.Format(
                            "INSERT INTO [dbo].[TestTimer] " +
                            "(Count, Timestamp) " +
                            "VALUES ({0}, {1})", count++, DateTime.Now);
            cmd.ExecuteNonQuery();

        }

        // GET: /User/DailySteps
        public ActionResult DailySteps()
        {
            ViewBag.Message = "Graph of your daily steps.";
            return View();
        }

        //POST: /User/DailySteps
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DailySteps(RegisterViewModel model) //need a new view model
        {


            return View(model);
        }
//
//        //
//        // POST: /Account/Register
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DailySteps(RegisterViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new ApplicationUser() { UserName = model.UserName };
//                var result = await UserManager.CreateAsync(user, model.Password);
//                if (result.Succeeded)
//                {
//                    await SignInAsync(user, isPersistent: false);
//                    return RedirectToAction("Index", "Home");
//                }
//                else
//                {
//                    AddErrors(result);
//                }
//            }
//
//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

        public ActionResult DailyCalories()
        {
            ViewBag.Message = "Graph of your daily calories burned.";
            return View();
        }

        public ActionResult DailyMinutes()
        {
            ViewBag.Message = "Graph of your daily active minutes.";
            return View();
        }

        public ActionResult DailyMiles()
        {
            ViewBag.Message = "Graph of your daily active miles.";
            return View();
        }

        public ActionResult ViewGoals()
        {
            ViewBag.Message = "Your Goals.";

            return View("~/Views/Goal/ViewGoals.cshtml");
        }

        public static string queryField(string name, string field)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString);
            sqlConnection.Open();
            string query =
                String.Format(
                    "SELECT [Values] FROM [dbo].[Daily] WHERE " +
                    //[Timestamp] >= DATEADD(minute, -5, CURRENT_TIMESTAMP) 
                    "[Metric] = '{0}' AND [Name] = '{1}' AND [Timestamp] = '2014-05-27 09:25:00.000'", field, name);
            string cols = string.Empty;
            SqlCommand queryCommand = new SqlCommand(query, sqlConnection);
            try
            {
                SqlDataReader queryReader = queryCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(queryReader);

                cols += dataTable.Rows[0][dataTable.Columns[0].ColumnName];
            }
            catch
            {
                cols += "0.0";
            }

            return cols;
        }


        public static string queryTestTimer()
        {
            int newCount = count;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString);
            sqlConnection.Open();
            string query = "SELECT * FROM [dbo].[TestTimer]";
            SqlCommand queryCommand = new SqlCommand(query, sqlConnection);

            SqlDataReader queryReader = queryCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(queryReader);
            string cols = string.Empty;

            for (int i = 0; i < newCount; i++)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    cols += dataTable.Rows[i][col.ColumnName] + " | ";
                }
            }

            return cols;
        }


        public static string getSQLHistory(DateTime? from, DateTime? to, string ns)
        {
//            insertSQL();

//            DateTime end = to == null ? DateTime.Now - TimeSpan.FromDays(1) : to.Value;
//            DateTime start = from == null ? end - TimeSpan.FromDays(7) : from.Value;

//            start = start.Date;
//            end = end.Date;

            DateTime date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime end = date.Date + ts;
            DateTime start = date.AddDays(-30).Date + ts;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            // SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString);

            // string connStr = ConfigurationManager.ConnectionStrings["MessagingSQL"].ConnectionString;
            List<NamespaceSQL> sqlResult;

            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
                {
//                    string query = "INSERT"

                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
                        sqlConn,
                        SQLsForCustomerQuery,
                        new SqlParameter("start", start),
                        new SqlParameter("end", end),
                        new SqlParameter("namespace", "1112"));
                });
            }

            JArray namespaceStepsArray = new JArray();
            JArray namespaceCaloriesArray = new JArray();
            JArray namespaceMilesArray = new JArray();
            JArray namespaceMinutesArray = new JArray();
            string scaleUnit = string.Empty;

            foreach (NamespaceSQL kpi in sqlResult)
            {
//                JArray StepsRecord =
//                   new JArray(DateTimeToUnixTimestamp(kpi.Timestamp),
//                       kpi.Steps);

                namespaceStepsArray.Add(kpi.Steps);

//                JArray CaloriesRecord =
//                   new JArray(DateTimeToUnixTimestamp(kpi.Timestamp),
//                       kpi.Calories);

                namespaceCaloriesArray.Add(kpi.Calories);

//                JArray MinutesRecord =
//                   new JArray(DateTimeToUnixTimestamp(kpi.Timestamp),
//                       kpi.Minutes);

                namespaceMinutesArray.Add(kpi.Minutes);

//                JArray MilesRecord =
//                new JArray(DateTimeToUnixTimestamp(kpi.Timestamp),
//                    kpi.Miles);

                namespaceMilesArray.Add(kpi.Miles);
            }

            if (sqlResult.Count > 0)
            {
                scaleUnit = sqlResult[0].Id;
            }

            JArray SQLsForNameSpace = new JArray();

            JObject namespaceSteps = new JObject();
            namespaceSteps["name"] = "Steps";
//            namespaceSteps["pointInterval"] = 24*3600*1000;
//            namespaceSteps["pointStart"] = start;
            namespaceSteps["data"] = namespaceStepsArray;
            SQLsForNameSpace.Add(namespaceSteps);

            JObject namespaceCalories = new JObject();
            namespaceCalories["name"] = "Calories";
            namespaceCalories["data"] = namespaceCaloriesArray;
            SQLsForNameSpace.Add(namespaceCalories);

            JObject namespaceMinutes = new JObject();
            namespaceMinutes["name"] = "Minutes";
            namespaceMinutes["data"] = namespaceMinutesArray;
            SQLsForNameSpace.Add(namespaceMinutes);

            JObject namespaceMiles = new JObject();
            namespaceMiles["name"] = "Miles";
            namespaceMiles["data"] = namespaceMilesArray;
            SQLsForNameSpace.Add(namespaceMiles);

            JObject result = new JObject();
            result["NamespaceName"] = ns;
            result["ScaleUnit"] = scaleUnit;
            result["NamespaceSQLs"] = SQLsForNameSpace;

            return result.ToString();
        }

        //using string to determine metric yields Internal Server Error. Sticking with separate methods for now.
//        public string getSQLHistory(string metric)
//        {
//            DateTime date = DateTime.Now;
//            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
//            DateTime end = date.Date + ts;
//            DateTime start = date.AddDays(-30).Date + ts;
//
//
//            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
//            builder["Data Source"] = "(local)";
//            builder["integrated Security"] = true;
//            builder["Initial Catalog"] = "Test";
//
//            List<NamespaceSQL> sqlResult;
//
//            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
//            {
//                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
//                {
//                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
//                        sqlConn,
//                        SQLsForCustomerQuery,
//                        new SqlParameter("start", start),
//                        new SqlParameter("end", end),
//                        new SqlParameter("namespace", "1112"));
//                });
//            }
//
//            JArray namespaceArray = new JArray();
//
//            foreach (NamespaceSQL kpi in sqlResult)
//            {
//                JArray Record;
//                switch (metric)
//                {
//                    case "Steps":
//                        Record = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Steps);
//                        break;
//                    case "Calories":
//                        Record = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Calories);
//                        break;
//                    case "Minutes":
//                        Record = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Minutes);
//                        break;
//                    case "Miles":
//                        Record = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Miles);
//                        break;
//                    default:
//                        Record = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Timestamp);
//                        break;
//                }
//                
//
//                namespaceArray.Add(Record);
//            }
//
//            JArray SQLsForNameSpace = new JArray();
//
//            JObject namespaces = new JObject();
//            namespaces["name"] = metric;
//            namespaces["data"] = namespaceArray;
//            SQLsForNameSpace.Add(namespaces);
//
//            JObject result = new JObject();
//            result["NamespaceName"] = sqlResult[0].Id;
//            result["ScaleUnit"] = metric;
//            result["NamespaceSQLs"] = SQLsForNameSpace;
//
//            return result.ToString();
//        }


        public string getStepHistory()
        {
            DateTime date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime end = date.Date + ts;
            DateTime start = date.AddDays(-30).Date + ts;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            List<NamespaceSQL> sqlResult;

            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
                {
                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
                        sqlConn,
                        SQLsForCustomerQuery,
                        new SqlParameter("start", start),
                        new SqlParameter("end", end),
                        new SqlParameter("namespace", "1112"));
                });
                sqlConn.Close();
            }

            JArray stepNamespaceArray = new JArray();

            foreach (NamespaceSQL kpi in sqlResult)
            {
                JArray stepRecord = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Steps);

                stepNamespaceArray.Add(stepRecord);
            }

            JArray StepsForNameSpace = new JArray();

            JObject stepNamespaces = new JObject();
            stepNamespaces["name"] = "Steps";
            stepNamespaces["data"] = stepNamespaceArray;
            StepsForNameSpace.Add(stepNamespaces);

            JObject stepResult = new JObject();
            stepResult["NamespaceName"] = sqlResult[0].Id;
            stepResult["ScaleUnit"] = "Steps";
            stepResult["NamespaceSQLs"] = StepsForNameSpace;

            return stepResult.ToString();
        }

        public string getCalHistory()
        {
            DateTime date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime end = date.Date + ts;
            DateTime start = date.AddDays(-30).Date + ts;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            List<NamespaceSQL> sqlResult;

            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
                {
                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
                        sqlConn,
                        SQLsForCustomerQuery,
                        new SqlParameter("start", start),
                        new SqlParameter("end", end),
                        new SqlParameter("namespace", "1112"));
                });
                sqlConn.Close();
            }

            JArray calNamespaceArray = new JArray();

            foreach (NamespaceSQL kpi in sqlResult)
            {
                JArray calRecord = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Calories);

                calNamespaceArray.Add(calRecord);
            }

            JArray CalsForNameSpace = new JArray();

            JObject calNamespaces = new JObject();
            calNamespaces["name"] = "Calories";
            calNamespaces["data"] = calNamespaceArray;
            CalsForNameSpace.Add(calNamespaces);

            JObject calResult = new JObject();
            calResult["NamespaceName"] = sqlResult[0].Id;
            calResult["ScaleUnit"] = "Calories";
            calResult["NamespaceSQLs"] = CalsForNameSpace;

            return calResult.ToString();
        }

        public string getMinHistory()
        {
            DateTime date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime end = date.Date + ts;
            DateTime start = date.AddDays(-30).Date + ts;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            List<NamespaceSQL> sqlResult;

            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
                {
                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
                        sqlConn,
                        SQLsForCustomerQuery,
                        new SqlParameter("start", start),
                        new SqlParameter("end", end),
                        new SqlParameter("namespace", "1112"));
                });
                sqlConn.Close();
            }

            JArray minNamespaceArray = new JArray();

            foreach (NamespaceSQL kpi in sqlResult)
            {
                JArray minRecord = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Minutes);

                minNamespaceArray.Add(minRecord);
            }

            JArray minsForNameSpace = new JArray();

            JObject minNamespaces = new JObject();
            minNamespaces["name"] = "Minutes";
            minNamespaces["data"] = minNamespaceArray;
            minsForNameSpace.Add(minNamespaces);

            JObject minResult = new JObject();
            minResult["NamespaceName"] = sqlResult[0].Id;
            minResult["ScaleUnit"] = "Minutes";
            minResult["NamespaceSQLs"] = minsForNameSpace;

            return minResult.ToString();
        }

        public string getMileHistory()
        {
            DateTime date = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0);
            DateTime end = date.Date + ts;
            DateTime start = date.AddDays(-30).Date + ts;


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
            builder.InitialCatalog = "ActivizeDB";
            builder.Encrypt = true;
            builder.TrustServerCertificate = false;
            builder.UserID = "afadmin@houhwxrzrr";
            builder.Password = "pass@word1";

            List<NamespaceSQL> sqlResult;

            using (SqlConnection sqlConn = new SqlConnection(builder.ConnectionString))
            {
                sqlResult = SqlHelpers.RetryExecute<List<NamespaceSQL>>(() =>
                {
                    return SqlHelpers.ExecuteSqlCommandWithResults(NamespaceSQL.Initializer,
                        sqlConn,
                        SQLsForCustomerQuery,
                        new SqlParameter("start", start),
                        new SqlParameter("end", end),
                        new SqlParameter("namespace", "1112"));
                });
                sqlConn.Close();
            }

            JArray mileNamespaceArray = new JArray();

            foreach (NamespaceSQL kpi in sqlResult)
            {
                JArray mileRecord = new JArray(DateTimeToUnixTimestamp(kpi.Timestamp), kpi.Miles);

                mileNamespaceArray.Add(mileRecord);
            }

            JArray MilesForNameSpace = new JArray();

            JObject mileNamespaces = new JObject();
            mileNamespaces["name"] = "Miles";
            mileNamespaces["data"] = mileNamespaceArray;
            MilesForNameSpace.Add(mileNamespaces);

            JObject mileResult = new JObject();
            mileResult["NamespaceName"] = sqlResult[0].Id;
            mileResult["ScaleUnit"] = "Miles";
            mileResult["NamespaceSQLs"] = MilesForNameSpace;

            return mileResult.ToString();
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToUniversalTime()).TotalMilliseconds;
        }


    }

    class NamespaceSQL
    {
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
        public double Steps { get; set; }
        public double Calories { get; set; }
        public double Minutes { get; set; }
        public double Miles { get; set; }

        public static NamespaceSQL Initializer(SqlDataReader reader)
        {
            NamespaceSQL namespaceSQL = new NamespaceSQL();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                switch (reader.GetName(i))
                {
                    case "Timestamp": namespaceSQL.Timestamp = (DateTime)reader[i];
                        break;
                    case "Id": namespaceSQL.Id = reader[i].ToString();
                        break;
                    case "Steps": namespaceSQL.Steps = Convert.ToDouble(reader[i]);
                        break;
                    case "Calories": namespaceSQL.Calories = Convert.ToDouble(reader[i]);
                        break;
                    case "Minutes": namespaceSQL.Minutes = Convert.ToDouble(reader[i]);
                        break;
                    case "Miles": namespaceSQL.Miles = Convert.ToDouble(reader[i]);
                        break;
                }
            }

            return namespaceSQL;
        }
    }
}