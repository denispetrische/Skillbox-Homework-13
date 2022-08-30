using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_Homework_13._1
{
    class ScreenInfo
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        
        public string bill1Name { get; set; }
        public string bill1Ammount { get; set; }
        public string bill2Name { get; set; }
        public string bill2Ammount { get; set; }

        public ScreenInfo(string secondName, string firstName, string patronymic, string bill1Name, string bill1Ammount, string bill2Name, string bill2Ammount)
        {
            this.SecondName = secondName;
            this.FirstName = firstName;
            this.Patronymic = patronymic;
            this.bill1Name = bill1Name;
            this.bill1Ammount = bill1Ammount;
            this.bill2Name = bill2Name;
            this.bill2Ammount = bill2Ammount;
        }
    }
}
