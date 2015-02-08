using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CodingLessons.admin
{
    public partial class Edit : System.Web.UI.Page
    {
        DataRow _levelData;
        int _lvl;
        int _solutionsCount;

        TextBox beschreibung = new TextBox();
        TextBox title = new TextBox();
        TextBox beispiel = new TextBox();
        TextBox aufgabe = new TextBox();
        TextBox given = new TextBox();
        TextBox score = new TextBox();

        protected void Page_Load(object sender, EventArgs e)
        {
            Snippets.CheckAdmin(User, Server);

            try
            {
                _lvl = Convert.ToInt32(Request.QueryString["exerciseIndex"]);
                _levelData = Snippets.getLevelData(_lvl).AsEnumerable().FirstOrDefault();
                object test = _levelData[2];
            }
            catch
            {
                Server.Transfer(@"~/admin/CMS.aspx");
            }
        }

        protected void megatron_Load(object sender, EventArgs e)
        {
            //Überschrift der Aufgabe einfügen
            title.Text = (string)_levelData["descriptionHeader"];
            megatron.Controls.Add(title);

            //"Löschen"-Knopf hinzufügen
            megatron.Controls.Add(new LiteralControl("<br />"));
            Button delButton = new Button();
            delButton.Text = "Aufgabe löschen";
            delButton.Click += DeleteTask;
            megatron.Controls.Add(delButton);

            //Zwischenüberschrift "Beschreibung" hinzufügen
            var topic1 = new HtmlGenericControl("h2");
            topic1.InnerText = "Beschreibung";
            megatron.Controls.Add(topic1);

            //Beschreibung zur Aufgabe einfügen
            beschreibung.TextMode = TextBoxMode.MultiLine;
            beschreibung.Text = (string)_levelData["description"];
            beschreibung.Columns = Snippets.getMaxWidth(beschreibung.Text);
            beschreibung.Rows = Snippets.countLines(beschreibung.Text);
            beschreibung.ReadOnly = false;
            megatron.Controls.Add(beschreibung);

            //Unterzwischenüberschrift "Beispiel" einfügen
            var topic2 = new HtmlGenericControl("h3");
            topic2.InnerText = "Beispiel";
            megatron.Controls.Add(topic2);

            //Beispiel zur Beschreibung der Aufgabe einfügen
            beispiel.TextMode = TextBoxMode.MultiLine;
            beispiel.Text = (string)_levelData["example"];
            beispiel.Columns = Snippets.getMaxWidth(beispiel.Text);
            beispiel.Rows = Snippets.countLines(beispiel.Text);
            beispiel.ReadOnly = false;
            megatron.Controls.Add(beispiel);

            //Zwischenüberschrift "Aufgabenstellung" hinzufügen
            var topic3 = new HtmlGenericControl("h2");
            topic3.InnerText = "Aufgabenstellung";
            megatron.Controls.Add(topic3);

            //Aufgabenstellung einfügen
            aufgabe.TextMode = TextBoxMode.MultiLine;
            aufgabe.Text = (string)_levelData["task"];
            aufgabe.Columns = Snippets.getMaxWidth(aufgabe.Text);
            aufgabe.Rows = Snippets.countLines(aufgabe.Text);
            aufgabe.ReadOnly = false;
            megatron.Controls.Add(aufgabe);

            //Unterzwischenüberschrift "Gegeben" hinzufügen
            var topic4 = new HtmlGenericControl("h3");
            topic4.InnerText = "Gegeben";
            megatron.Controls.Add(topic4);

            //Gegebenen Code hinzufügen
            given.TextMode = TextBoxMode.MultiLine;
            given.Text = (string)_levelData["given"];
            given.Columns = Snippets.getMaxWidth(given.Text);
            given.Rows = Snippets.countLines(given.Text);
            given.ReadOnly = false;
            megatron.Controls.Add(given);

            //Unterzwischenüberschrigt "Punkte" hinzufügen
            var topic5 = new HtmlGenericControl("h3");
            topic5.InnerText = "Punkte";
            megatron.Controls.Add(topic5);

            //Anzahl Punkte hinzufügen
            score.TextMode = TextBoxMode.Number;
            score.Text = ((int)_levelData["score"]).ToString();
            megatron.Controls.Add(score);

            //"Solutions"
            var solutionsTopic = new HtmlGenericControl("h2");
            solutionsTopic.InnerText = "Solutions";
            megatron.Controls.Add(solutionsTopic);

            _solutionsCount = 1;

            string solution = Snippets.getSolution(_solutionsCount, _lvl);
            while (solution != String.Empty)
            {
                var solutionTopic = new HtmlGenericControl("h3");
                solutionTopic.InnerText = String.Format("[[{0}]]", _solutionsCount);
                megatron.Controls.Add(solutionTopic);

                var solutionBox = new TextBox();
                solutionBox.TextMode = TextBoxMode.MultiLine;
                solutionBox.Text = solution;
                solutionBox.ReadOnly = false;
                solutionBox.Rows = Snippets.countLines(solution);
                solutionBox.Columns = Snippets.getMaxWidth(solution);
                solutionBox.ID = "solution" + _solutionsCount;
                megatron.Controls.Add(solutionBox);

                //"Löschen"-Knopf hinzufügen
                var deleteButton = new Button();
                deleteButton.Text = "Solution löschen";
                deleteButton.ID = "deleteButton" + _solutionsCount;
                deleteButton.Click += DeleteSolution;
                megatron.Controls.Add(deleteButton);

                solution = Snippets.getSolution(++_solutionsCount, _lvl);
            }
            //last one was null
            _solutionsCount--;

            //add "add"-button
            megatron.Controls.Add(new LiteralControl("<br />"));
            Button addSolution = new Button();
            addSolution.Text = "Solution hinzufügen";
            addSolution.Click += AddSolution;
            megatron.Controls.Add(addSolution);

            //"Speichern"-Knopf hinzufügen
            megatron.Controls.Add(new LiteralControl("<br /><br />"));
            Button saveButton = new Button();
            saveButton.Text = "Änderungen speichern";
            saveButton.Click += Save;
            megatron.Controls.Add(saveButton);
        }

        void AddSolution(object sender, EventArgs e)
        {
            SaveNonReturn();

            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmd = conn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO solutions " +
                                            "(fk_levl, pageOrder, solution) " +
                                            "VALUES ({0}, {1}, '{2}');"
                                            , _lvl, _solutionsCount + 1, "//Hier bitte ihren Code");
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Refresh();
        }

        void DeleteSolution(object sender, EventArgs e)
        {
            SaveNonReturn();

            int pageOrder = Convert.ToInt32(((Button)sender).ID.Substring(12));

            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmd = conn.CreateCommand();
            cmd.CommandText = String.Format("DELETE FROM solutions " +
                                            "WHERE fk_levl = {0} and pageOrder = {1};"
                                            , _lvl, pageOrder);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            //filling solutions gap
            for (int i = pageOrder + 1; i <= _solutionsCount; i++)
            {
                var command = conn.CreateCommand();
                command.CommandText = String.Format("UPDATE solutions " +
                                                    "SET pageOrder={1} " +
                                                    "WHERE pageOrder={0};"
                                                    , i, i - 1);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            Refresh();
        }

        void Refresh()
        {
            Server.Transfer("~/admin/Edit.aspx?exerciseIndex=" + _lvl);
        }

        void DeleteTask(object sender, EventArgs e)
        {
            //removing solutions
            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmand = conn.CreateCommand();
            cmand.CommandText = String.Format("DELETE FROM solutions " +
                                              "WHERE fk_levl = {0};"
                                              , _lvl);
            conn.Open();
            cmand.ExecuteNonQuery();
            conn.Close();

            //filling solutions gap
            for (int i = _lvl + 1; Snippets.getLevelData(i).AsEnumerable().FirstOrDefault() != null; i++)
            {
                var command = conn.CreateCommand();
                command.CommandText = String.Format("UPDATE solutions " +
                                                    "SET fk_levl={1} " +
                                                    "WHERE fk_levl={0};"
                                                    , i, i - 1);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            //removing exercise
            var cmd = conn.CreateCommand();
            cmd.CommandText = String.Format("DELETE FROM levl " +
                                            "WHERE lvl={0}"
                                            , _lvl);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            //filling level gap
            for (int i = _lvl + 1; Snippets.getLevelData(i).AsEnumerable().FirstOrDefault() != null; i++)
            {
                var command = conn.CreateCommand();
                command.CommandText = String.Format("UPDATE levl " +
                                                    "SET lvl={1} " +
                                                    "WHERE lvl={0};"
                                                    , i, i-1);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            Return();
        }

        void Save(object sender, EventArgs e)
        {
            SaveNonReturn();
            Return();
        }

        void Return()
        {
            Server.Transfer("~/admin/CMS.aspx");
        }

        void SaveNonReturn()
        {
            //update database
            string constring = Snippets.stringOfConnection;
            var conn = new SqlConnection(constring);
            var command = conn.CreateCommand();
            command.CommandText = String.Format("UPDATE levl " +
                                                "SET description='{1}', descriptionHeader='{2}', example='{3}', task='{4}', given='{5}', score={6} " +
                                                "WHERE lvl={0};"
                                                , _lvl, beschreibung.Text, title.Text, beispiel.Text, aufgabe.Text, given.Text, score.Text);
            conn.Open();
            command.ExecuteNonQuery();

            foreach (Control control in megatron.Controls)
            {
                if (control.ID != null && control.ID.StartsWith("solution"))
                {
                    var cmd = conn.CreateCommand();
                    command.CommandText = String.Format("UPDATE solutions " +
                                                        "SET solution='{2}' " +
                                                        "WHERE fk_levl={0} and pageOrder={1}"
                                                        , _lvl, control.ID.Substring(8), ((TextBox)control).Text);
                    command.ExecuteNonQuery();
                }
            }

            conn.Close();
        }
    }
}