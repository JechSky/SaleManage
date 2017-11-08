using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ListItem
    {
        string id = string.Empty;
        string name = string.Empty;
        public ListItem(string sid,string name)
        {
            this.id = sid;
            this.name = name;
        }
        public string ID { get { return this.id; } set { this.id = value; } }
        public string Name { get { return this.name; } set { this.name = value; } }

        public void Add(ListItem item)
        {
            this.Add(item);
        }

        public override string ToString()
        {
            return this.name;
        }
      

    }
}
