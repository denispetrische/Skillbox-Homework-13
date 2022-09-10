using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_Homework_13._1.Classes
{
    class ScreenInfo
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string bill1Ammount { get; set; }
        public string bill2Ammount { get; set; }

        public ScreenInfo(string secondName, string firstName, string patronymic, string bill1Ammount, string bill2Ammount)
        {
            SecondName = secondName;
            FirstName = firstName;
            Patronymic = patronymic;
            this.bill1Ammount = bill1Ammount;
            this.bill2Ammount = bill2Ammount;
        }
    }
}
