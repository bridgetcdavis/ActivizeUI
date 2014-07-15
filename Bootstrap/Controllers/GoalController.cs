using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bootstrap.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bootstrap.Controllers
{
    public class GoalController : Controller
    {
        //
        // GET: /Goal/
        public ActionResult Goals()
        {
        //    ViewBag.Id = "3bd18fc4-2c3c-4dd2-b835-3022b6a82fda";
            return View();
        }

        public ActionResult ViewGoals()
        {
            ViewBag.Message = "Your Goals:";
            return View();
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        [Authorize]
        [HttpPost]
        public ActionResult Goals(GoalViewModel model)
        {
            if (ModelState.IsValid)
            {

                ActionResult action;

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "tcp:houhwxrzrr.database.windows.net";
                builder.InitialCatalog = "ActivizeDB";
                builder.Encrypt = true;
                builder.TrustServerCertificate = false;
                builder.UserID = "afadmin@houhwxrzrr";
                builder.Password = "pass@word1";

                SqlConnection sqlConnection = new SqlConnection(builder.ConnectionString);
                
                
                String id = getID(User.Identity.GetUserName());
                Double stepGoal = model.StepGoal;
                Double calGoal = model.CalGoal;
                Double minGoal = model.MinGoal;
                Double mileGoal = model.MileGoal;
                SqlCommand cmd = sqlConnection.CreateCommand();

                try
                {
                    sqlConnection.Open();
                    if (hasGoal(User.Identity.GetUserName()))
                    {
                        cmd.CommandText = String.Format(
                            "UPDATE [dbo].[UserGoals] " +
                            "SET [StepGoal] = {0}, [CalGoal] = {1}, [MinGoal] = {2}, [MileGoal] = {3} " +
                            "WHERE [Id] = '{4}'", stepGoal, calGoal, minGoal, mileGoal, id);
                    }
                    else
                    {
                        cmd.CommandText = String.Format(
                            "INSERT INTO [dbo].[UserGoals] " +
                            "(Id, StepGoal, CalGoal, MinGoal, MileGoal) " +
                            "VALUES ('{0}', {1}, {2}, {3}, {4})", id, stepGoal, calGoal, minGoal, mileGoal);
                    }

                    cmd.ExecuteNonQuery();
                    action = RedirectToAction("ViewGoals", "Goal");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    action = RedirectToAction("Index", "Home");
                }
                finally
                {
                    sqlConnection.Close();

                }

                return action;
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public static string queryGoals(string id, string field)
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
            string query = String.Format("SELECT [{0}] FROM [dbo].[UserGoals] WHERE [Id] = '{1}'", field, id);
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
            finally
            {
                sqlConnection.Close();
            }

            return cols;
        }

        public static string getID(string user)
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
            string query = String.Format("SELECT [Id] FROM [dbo].[AspNetUsers] WHERE [UserName] = '{0}'", user);
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
                cols += "Not Found";
            }
            finally
            {
                sqlConnection.Close();
            }

            return cols;
        }

        public static bool hasGoal(string user)
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
            string query = String.Format("SELECT * FROM [dbo].[UserGoals] WHERE [Id] = '{0}'", getID(user));
            string cols = String.Empty;
            try
            {
                SqlCommand queryCommand = new SqlCommand(query, sqlConnection);
                SqlDataReader queryReader = queryCommand.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(queryReader);

                cols += dataTable.Rows[0][dataTable.Columns[0].ColumnName];
            }
            catch
            {
            }
            finally
            {
                sqlConnection.Close();
            }

            if (String.IsNullOrEmpty(cols))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

	}
}