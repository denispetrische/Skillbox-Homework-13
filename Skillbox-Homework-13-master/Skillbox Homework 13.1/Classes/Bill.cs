using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skillbox_Homework_13._1.Interfaces;

namespace Skillbox_Homework_13._1.Classes
{
    public class Bill : IAddMoney<Bill>
    {
        private protected float ammount;

        public float Ammount
        {
            get { return ammount; }
            set { ammount = value; }
        }

        public Bill( float ammount)
        {
            this.ammount = ammount;
        }

        public Bill AddMoney(float sum)
        {
            float temp = ammount + sum;

            return new Bill(temp);
        }
    }
}
