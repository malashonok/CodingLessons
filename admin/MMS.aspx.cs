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
    public partial class MMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Snippets.CheckAdmin(User, Server);
        }

        protected void Messages_Load(object sender, EventArgs e)
        {
            List<string[]> messageList = GetMessages();

            foreach(string[] message in messageList)
            {
                Panel megatron = new Panel();
                megatron.CssClass = "megatron";
                megatron.ID = "message" + message[3];

                //Betreff
                var title = new HtmlGenericControl("h1");
                title.InnerText = message[0];
                megatron.Controls.Add(title);

                //Inhalt
                TextBox content = new TextBox();
                content.TextMode = TextBoxMode.MultiLine;
                content.Text = message[1];
                content.Columns = Snippets.getMaxWidth(content.Text);
                content.Rows = Snippets.countLines(content.Text);
                content.CssClass = "roundbox";
                content.ReadOnly = true;
                megatron.Controls.Add(content);

                megatron.Controls.Add(new HtmlGenericControl("br"));

                //Absender
                var replyTo = new Label();
                replyTo.Text = String.Format("Von: {0}", message[2]);
                megatron.Controls.Add(replyTo);

                megatron.Controls.Add(new HtmlGenericControl("br"));

                //Optionen
                var replyButton = new Button();
                replyButton.OnClientClick = String.Format("window.open('mailto:{0}?subject=Re: {1}');", message[2], message[0]);
                replyButton.Text = "Antworten";
                megatron.Controls.Add(replyButton);

                var deleteButton = new Button();
                deleteButton.Click += Delete;
                deleteButton.Text = "Löschen";
                megatron.Controls.Add(deleteButton);

                Messages.Controls.Add(megatron);
            }
        }

        protected void Delete(object sender, EventArgs e)
        {
            //get ID
            string ID = ((Control)sender).Parent.ID.Substring(7);

            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmd = conn.CreateCommand();

            cmd.CommandText = String.Format("DELETE FROM userFeedback " +
                                            "WHERE fk_id={0};", ID);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Server.Transfer("~/admin/MMS.aspx");
        }

        protected List<string[]> GetMessages()
        {
            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmd = conn.CreateCommand();

            cmd.CommandText = String.Format("SELECT TOP 50 " +
                                            "topic, message, replyTo, fk_id " +
                                            "FROM userFeedback;");

            var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();

            conn.Open();
            adapter.Fill(table);
            conn.Close();

            var output = new List<string[]>();
            foreach (DataRow row in table.Rows)
            {
                string[] rowData = new string[4];

                rowData[0] = row.ItemArray[0].ToString();
                rowData[1] = row.ItemArray[1].ToString();
                rowData[2] = row.ItemArray[2].ToString();
                rowData[3] = row.ItemArray[3].ToString();

                output.Add(rowData);
            }

            return output;
        }
    }
}