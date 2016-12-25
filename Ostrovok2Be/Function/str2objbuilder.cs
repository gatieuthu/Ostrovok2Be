using System.Collections.Generic;

namespace Ostrovok2Be.Function
{
    public class str2objbuilder
    {
       private List<string> str;
        public str2objbuilder(List<string> tempstr)
        {
            str = tempstr;
        }
        public  string listIds2Object()
        {//
            string result="";

           
            foreach (var item in str)
            {
                result += item+@""",""";
            }

            if (result.Length > 2)
                result = result.Substring(0,result.Length-3);
            return result;
            

        }
    }
}