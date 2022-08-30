using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_Homework_13._1
{
    interface IShowBill<out T>
    {
        T ShowBill(int billNumber);
    }
}
