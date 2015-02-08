using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CodingLessons
{
    public static class Snippets
    {
        public static string stringOfConnection = Pwds.webConString;
        //public static string stringOfConnection = Pwds.localConString;
        public static void CheckAdmin(IPrincipal User, HttpServerUtility Server)
        {
            if (User.IsInRole("admin"))
                return;

            alert("Diese Seite erfordert Administrationsrechte. Bitte melden Sie sich an.");
            Server.Transfer(@"~/Account/Login.aspx");
        }

        public static void CheckUserData(DataRow _userData, HttpServerUtility Server)
        {
            if (_userData == null) //not logged in
            {
                alert("Sie sind nicht angemeldet. Bitte melden Sie sich an.");

                //redirect 
                Server.Transfer("Default.aspx");
            }
        }

        public static void CheckLevelData(DataRow _levelData, HttpServerUtility Server)
        {
            if (_levelData == null) //no more levels
            {
                alert("Glückwunsch! Sie haben alle Aufgaben gelöst! In nächster Zukunft werden hier neue Aufgaben erscheinen.");

                //redirect 
                Server.Transfer("Default.aspx");
            }
        }

        public static void alert(string message)
        {
            HttpContext.Current.Response.Write
                (
                    String.Format
                    (
                        "<script> alert(\"{0}\") </script>"
                        , message
                    )
                );
        }

        public static DataTable getUserData(IPrincipal User)
        {
            //Get level from Database
            var table = new DataTable();
            string constring = Snippets.stringOfConnection;
            SqlConnection conn = new SqlConnection(constring);
            var command = conn.CreateCommand();
            command.CommandText = String.Format(@"
                SELECT * 
                FROM Users 
                WHERE name = '{0}';
            ", User.Identity.GetUserId());
            command.Connection.Open();

            var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            conn.Close();
            adapter.Dispose();

            return table;
        }

        public static DataTable getLevelData(DataRow _userData)
        {
            return getLevelData((int)_userData["lvl"]);
        }

        public static DataTable getLevelData(int lvl)
        {
            //Get data from Database
            var table = new DataTable();
            string constring = Snippets.stringOfConnection;
            SqlConnection conn = new SqlConnection(constring);
            var command = conn.CreateCommand();
            command.CommandText = String.Format(@"
                SELECT *
                FROM levl
                WHERE lvl = {0};
            ", lvl);
            command.Connection.Open();

            var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            conn.Close();
            adapter.Dispose();

            return table;
        }

        public static int countLines(string lines)
        {
            var linesArray = lines.Split('\n');

            return linesArray.Count();
        }

        public static int getMaxWidth(string input)
        {
            var lines = input.Split('\n');
            int maxLength = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxLength)
                    maxLength = line.Length;
            }

            return maxLength;
        }

        public static string getSolution(int index, DataRow _userData)
        {
            return getSolution(index, (int)_userData["lvl"]);
        }

        public static string getSolution(int index, int lvl)
        {
            //Get solution from Database
            var table = new DataTable();
            string constring = stringOfConnection;
            SqlConnection conn = new SqlConnection(constring);
            var command = conn.CreateCommand();
            command.CommandText = String.Format(@"
                SELECT *
                FROM solutions
                WHERE fk_levl = {0} AND pageOrder = {1}
            ", lvl, index);
            command.Connection.Open();

            var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);

            conn.Close();
            adapter.Dispose();

            var solutionRow = table.AsEnumerable().FirstOrDefault();
            if (solutionRow == null)
                return String.Empty;

            return solutionRow["solution"].ToString();
        }
    }
}