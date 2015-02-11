using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodingLessons
{
    public partial class Exercise : System.Web.UI.Page
    {
        public DataRow _levelData;
        public DataRow _userData;
        public CodePart[] _solutions;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userData = Snippets.getUserData(User).AsEnumerable().FirstOrDefault();

            Snippets.CheckUserData(_userData, Server);

            _levelData = Snippets.getLevelData(_userData).AsEnumerable().FirstOrDefault();

            Snippets.CheckLevelData(_levelData, Server);
        }

        protected void solutionPlaceHolder_Load(object sender, EventArgs e)
        {
            var parts = new List<CodePart>();

            //load codeParts
            var codeParts = devideSolution();

            foreach(CodePart part in codeParts)
            {
                    //create textbox
                    var newBox = new TextBox();
                    newBox.BorderStyle = BorderStyle.None;
                    newBox.Style.Add("Resize", "none");
                    newBox.TextMode = TextBoxMode.MultiLine;
                    
                    //set box size
                    newBox.Rows    = Snippets.countLines (part.codePart);
                    newBox.Columns = Snippets.getMaxWidth(part.codePart);

                    //insert text into box
                    if(part.solutionNmb == null)
                    {
                        newBox.Text = part.codePart;

                        newBox.BackColor = System.Drawing.Color.Transparent;

                        newBox.ReadOnly = true;
                    }
                    else
                    {
                        newBox.Text = @"//Hier bitte ihren Code";

                        newBox.BackColor = System.Drawing.Color.FromArgb(0xb3, 0xb3, 0xb3);

                        //save box
                        part.box = newBox;
                        parts.Add(part);
                    }

                    //Add to panel
                    solutionPlaceHolder.Controls.Add(newBox);
                    solutionPlaceHolder.Controls.Add(new LiteralControl("<br />"));
            }

            //save solution boxes
            _solutions = parts.ToArray();
        }

        CodePart[] devideSolution()
        {
            var codeParts = new List<CodePart>();

            //split solution
            var parts = _levelData["given"].ToString().Split(new string[] { "[[", "]]" }, StringSplitOptions.None);

            //declare clock variable: true means the content is the index of the solution part in the database
            bool clock = parts[0].StartsWith("[[");
            foreach (string part in parts)
            {
                if(clock)
                {
                    //create codepart
                    int index = Convert.ToInt32(part);
                    string code = Snippets.getSolution(index, _userData);
                    var codePart = new CodePart(index, code);

                    //append codepart
                    codeParts.Add(codePart);
                }
                else
                {
                    //create codepart
                    var codePart = new CodePart(null, part);

                    //append codepart
                    codeParts.Add(codePart);
                }

                //tick
                clock = !clock;
            }

            return codeParts.ToArray();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            foreach(CodePart part in _solutions)
            {
                if (part.solutionNmb != null)
                    if (Check(part.codePart, part.box.Text))
                    {
                        //update level and score
                        string constring = Snippets.stringOfConnection;
                        SqlConnection conn = new SqlConnection(constring);
                        var command = conn.CreateCommand();
                        command.CommandText = String.Format(@"
                                                            UPDATE Users
                                                            SET lvl={0}, points={1}
                                                            WHERE name='{2}';
                                              ", (Convert.ToInt32(_userData["lvl"])+1).ToString()
                                               , (Convert.ToInt32(_userData["points"])+Convert.ToInt32(_levelData["score"])).ToString()
                                               , _userData["name"]);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        conn.Close();

                        Server.Transfer("~/Description.aspx");
                    }
                    else
                    {
                        Congratulate(false);
                    }
            }
        }

        /// <summary>
        /// Checks the second string to be equal to the first while ignoring all spaces, tabulators and line breaks.
        /// </summary>
        /// <param name="solution">The correct string.</param>
        /// <param name="toCheck">The string to be checked.</param>
        /// <returns></returns>
        protected bool Check(string solution, string toCheck)
        {
            //removing all spaces
            solution = solution.Replace(" ", String.Empty);
            toCheck  = toCheck .Replace(" ", String.Empty);

            //removing all tabulators
            solution = solution.Replace("\t", String.Empty);
            toCheck  = toCheck .Replace("\t", String.Empty);

            //removing all kinds of line breaks
            solution = solution.Replace("\r", String.Empty);
            toCheck  = toCheck. Replace("\r", String.Empty);
            solution = solution.Replace("\n", String.Empty);
            toCheck  = toCheck .Replace("\n", String.Empty);

            return toCheck == solution;
        }

        protected void Congratulate(bool passed)
        {
            Snippets.alert(passed ? "Glückwunsch! Sie haben diese Aufgabe bestanden."
                                  : "Das war nicht richtig. Bitte überprüfe noch einmal deine Antwort.");
        }
    }
}