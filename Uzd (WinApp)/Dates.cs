using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uzd__WinApp_
{
    public class Dates
    {
        public string wstart { get;  set; }
        public string wend { get;  set;}
        public int id { get; set; }

        [JsonConstructor]
        public Dates(int id,string start, string end)
        {
            this.wstart = start;
            this.wend = end;
            this.id = id;
        }
        public Dates (string start, string end)
        {
            this.wstart = start;
            this.wend = end;
        }
        public override string ToString()
        {
            return String.Format("Work start: {0,15}   Work End: {1,15} id: {2,3}",wstart,wend,id);
        }
    }
}
