using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skillbox_Homework_13._1.Classes;

namespace Skillbox_Homework_13._1.Interfaces
{
    interface ITransfer<in T>
    {
        Client Transfer(T someBill, float sum, string type);
    }
}
