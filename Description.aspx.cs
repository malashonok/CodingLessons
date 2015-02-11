using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CodingLessons
{
    public partial class Description : System.Web.UI.Page
    {
        public DataRow _levelData;
        public DataRow _userData;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userData   = Snippets.getUserData(User).AsEnumerable().FirstOrDefault();

            Snippets.CheckUserData(_userData, Server);

            _levelData  = Snippets.getLevelData(_userData).AsEnumerable().FirstOrDefault();

            Snippets.CheckLevelData(_levelData, Server);

            _levelData = formatData();

            //print example
            var code = new HtmlGenericControl("pre");
            code.InnerText = _levelData["example"].ToString();
            code.Attributes.Add("class", "prettyprint lang-cs linenums");
            given.Controls.Add(code);
        }

        DataRow formatData()
        {
            var table = _levelData;

            string current;

            //format description
            current = table["description"].ToString();
            current = current.Replace(@"<", @"&#60;")
                             .Replace(@">", @"&#62;")
                             .Replace(Environment.NewLine, @"<br />");
            table["description"] = current;

            return table;
        }
    }
}