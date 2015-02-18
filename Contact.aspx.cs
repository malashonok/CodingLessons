using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodingLessons
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Send(object sender, EventArgs e)
        {
            var conn = new SqlConnection(Snippets.stringOfConnection);
            var cmd = conn.CreateCommand();

            cmd.CommandText = String.Format("INSERT INTO userFeedback " +
                                            "(topic, message, replyTo) " +
                                            "VALUES ('{0}', '{1}', '{2}');"
                                            , topic.Text, message.Text, email.Text);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            Snippets.alert("Vielen Dank für ihre Nachricht! Sie werden in Kürze eine Antwort von uns erhalten.");
            Server.Transfer("~/Default.aspx");
        }
    }
}