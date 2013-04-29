using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project
{
    public class Item
    {
        //Arguments
        public string name;
        public int total;
        public string effect;
        public int stat;
        public string description;
        public string type;
        public int place;

        //Constructor
        public Item(string _type, string _name, string _effect, int _stat, int _total)// string _description, int _cost)
        {
            this.type = _type;
            this.name = _name;
            this.effect = _effect;
            this.stat = _stat;
            this.total = _total;

            /*  this.cost = _cost;
             this.description = _description;*/
        }
    }
}
