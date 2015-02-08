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
    public partial class CMS : System.Web.UI.Page
    {
        int _exercisesCount;
        const byte MAX_LEVEL_COUNT = 64;

        protected void Page_Load(object sender, EventArgs e)
        {
            Snippets.CheckAdmin(User, Server);
        }

        protected void Exercises_Load(object sender, EventArgs e)
        {
            DataRow current = Snippets.getLevelData(1).AsEnumerable().FirstOrDefault();
            for (_exercisesCount = 1; current != null; _exercisesCount++)
            {
                //Megatron erzeugen
                Panel megatron = new Panel();
                megatron.CssClass = "megatron";

                //Überschrift der Aufgabe einfügen
                var title = new HtmlGenericControl("h1");
                title.InnerText = (string)current["descriptionHeader"];
                megatron.Controls.Add(title);

                //"Bearbeiten"-Link einfügen
                LinkButton editLink = new LinkButton();
                editLink.Text = "[Bearbeiten]";
                editLink.PostBackUrl = String.Format(@"~/admin/Edit?exerciseIndex={0}", _exercisesCount);
                megatron.Controls.Add(editLink);

                //Zwischenüberschrift "Beschreibung" hinzufügen
                var topic1 = new HtmlGenericControl("h2");
                topic1.InnerText = "Beschreibung";
                megatron.Controls.Add(topic1);

                //Beschreibung zur Aufgabe einfügen
                TextBox beschreibung = new TextBox();
                beschreibung.TextMode = TextBoxMode.MultiLine;
                beschreibung.Text = (string)current["description"];
                beschreibung.Columns = Snippets.getMaxWidth(beschreibung.Text);
                beschreibung.Rows = Snippets.countLines(beschreibung.Text);
                beschreibung.CssClass = "roundbox";
                beschreibung.ReadOnly = true;
                megatron.Controls.Add(beschreibung);

                //Unterzwischenüberschrift "Beispiel" einfügen
                var topic2 = new HtmlGenericControl("h3");
                topic2.InnerText = "Beispiel";
                megatron.Controls.Add(topic2);

                //Beispiel zur Beschreibung der Aufgabe einfügen
                var example = new HtmlGenericControl("pre");
                example.InnerText = current["example"].ToString();
                example.Attributes.Add("class", "prettyprint lang-cs linenums");
                megatron.Controls.Add(example);

                //Zwischenüberschrift "Aufgabenstellung" hinzufügen
                var topic3 = new HtmlGenericControl("h2");
                topic3.InnerText = "Aufgabenstellung";
                megatron.Controls.Add(topic3);

                //Aufgabenstellung einfügen
                TextBox aufgabe = new TextBox();
                aufgabe.TextMode = TextBoxMode.MultiLine;
                aufgabe.Text = (string)current["task"];
                aufgabe.Columns = Snippets.getMaxWidth(aufgabe.Text);
                aufgabe.CssClass = "roundbox";
                aufgabe.Rows = Snippets.countLines(aufgabe.Text);
                aufgabe.ReadOnly = true;
                megatron.Controls.Add(aufgabe);

                //Unterzwischenüberschrift "Gegeben" hinzufügen
                var topic4 = new HtmlGenericControl("h3");
                topic4.InnerText = "Gegeben";
                megatron.Controls.Add(topic4);

                //Gegebenen Code eingeben
                var given = new HtmlGenericControl("pre");
                given.InnerText = current["given"].ToString();
                given.Attributes.Add("class", "prettyprint lang-cs linenums");
                megatron.Controls.Add(given);

                //Unterzwischenüberschrift "Punkte" hinzufügen
                var topic5 = new HtmlGenericControl("h3");
                topic5.InnerText = "Punkte";
                megatron.Controls.Add(topic5);

                //Anzahl Punkte anzeigen
                var score = new TextBox();
                score.TextMode = TextBoxMode.Number;
                score.ReadOnly = true;
                score.Text = ((int)current["score"]).ToString();
                megatron.Controls.Add(score);

                //"Solutions"
                var solutionsTopic = new HtmlGenericControl("h2");
                solutionsTopic.InnerText = "Solutions";
                megatron.Controls.Add(solutionsTopic);

                int index = 1;
                int lvl = (int)current["lvl"];
                
                string solution = Snippets.getSolution(index, lvl);
                while (solution != String.Empty)
                {
                    var solutionTopic = new HtmlGenericControl("h3");
                    solutionTopic.InnerText = String.Format("[[{0}]]", index);
                    megatron.Controls.Add(solutionTopic);

                    var solutionBox = new HtmlGenericControl("pre");
                    solutionBox.InnerText = solution;
                    solutionBox.Attributes.Add("class", "prettyprint lang-cs linenums");
                    megatron.Controls.Add(solutionBox);

                    solution = Snippets.getSolution(++index, lvl);
                }

                //Megatron zu Seite hinzufügen
                Exercises.Controls.Add(megatron);

                //Induktionsschritt
                current = Snippets.getLevelData(_exercisesCount + 1).AsEnumerable().FirstOrDefault();
            }
            _exercisesCount--; //last one was null
        }

        protected void AddLevel(object sender, EventArgs e)
        {
            byte aimPosition = Convert.ToByte(newPosition.Text);

            if (aimPosition < 1 || aimPosition > _exercisesCount + 1)
            {
                Snippets.alert(String.Format("Bitte geben Sie eine Position zwischen 1 und {0} an.", _exercisesCount));
                return;
            }
            if (aimPosition > MAX_LEVEL_COUNT)
            {
                Snippets.alert(String.Format("Die aktuelle Aufgabenhöchstanzahl beträgt {0}. Bitte geben Sie einen kleineren Wert an.", MAX_LEVEL_COUNT));
                return;
            }

            var conn = new SqlConnection(Snippets.stringOfConnection);
            conn.Open();
            //creating level gap
            for (int i = _exercisesCount; i >= aimPosition; i--)
            {
                var command = conn.CreateCommand();
                command.CommandText = String.Format("UPDATE levl " +
                                                    "SET lvl={1} " +
                                                    "WHERE lvl={0};"
                                                    , _exercisesCount, _exercisesCount + 1);
                command.ExecuteNonQuery();
            }

            //moving solutions too
            for (int i = _exercisesCount; i >= aimPosition; i--)
            {
                var command = conn.CreateCommand();
                command.CommandText = String.Format("UPDATE solutions " +
                                                    "SET fk_levl={1} " +
                                                    "WHERE fk_levl={0};"
                                                    , _exercisesCount, _exercisesCount + 1);
                command.ExecuteNonQuery();
            }
            conn.Close();

            //insert level into gap
            var cmd = conn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO levl " +
                                            "(description, task, given, lvl, descriptionHeader, example, score) " +
                                            "VALUES ('Beschreibung', 'Aufgabenstellung', '//Gegebener Code', {0}, 'Titel', '//Codebeispiel', 0)"
                                            , aimPosition);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Server.Transfer("~/admin/CMS.aspx");
        }
    }
}