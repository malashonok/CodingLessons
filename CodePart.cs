using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CodingLessons
{
    public class CodePart
    {
        public int? solutionNmb;
        public string codePart;
        public TextBox box;
        

        public CodePart(int? solutionNmb, string codePart)
        {
            this.solutionNmb = solutionNmb;
            this.codePart = codePart;
        }
    }
}