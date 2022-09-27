using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLibrary
{
    public class NonDepositBill : Bill
    {
        public NonDepositBill( float ammount) : base( ammount) { }
    }
}
