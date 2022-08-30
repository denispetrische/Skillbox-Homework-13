using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_Homework_13._1
{
    public class Bill
    {
        private protected float ammount;
        private protected string name;

        public float Ammount 
        { 
            get { return ammount; }
            set { ammount = value; }
        }

        public string Name 
        {
            get { return name; }
            set { name = value; } 
        }

        public Bill(string name, float ammount)
        {
            this.name = name;
            this.ammount = ammount;
        }
    }
}
