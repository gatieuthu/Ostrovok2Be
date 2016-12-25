using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ostrovok2Be.Function
{
    public class list2objbuilder
    {
        public static List<List<string>> ListCreater(List<string> input,int n)
        {
            if (input != null)
            {
                List<List<string>> tempstring = new List<List<string>>();
                if (input.Count() <= n)
                {
                    tempstring.Add(input);
                    return tempstring;
                }
                else
                {
                    for (int i = 0; i <= input.Count()/n; i++)
                    {       
                        tempstring.Add(input.Skip(i*n).Take(n).ToList());
                    }
                        return tempstring;
                }
            }
            else
            {
                return null;
            }

        }
    }
}
