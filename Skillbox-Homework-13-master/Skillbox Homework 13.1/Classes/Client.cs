using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Skillbox_Homework_13._1.Interfaces;

namespace Skillbox_Homework_13._1.Classes
{
    class Client : ITransfer<Bill>
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }

        public Bill depositBill;
        public Bill nonDepositBill;

        public Client(string secondName, string firstName, string patronymic)
        {
            SecondName = secondName;
            FirstName = firstName;
            Patronymic = patronymic;

            depositBill = new DefaultBill(0);
            nonDepositBill = new DefaultBill(0);
        }

        public Client Transfer(Bill someBill, float sum, string type)
        {
            if (type == "DepositBill")
            {
                if (someBill.Ammount >= sum)
                {
                    depositBill.Ammount += sum;
                }
                else
                {
                    MessageBox.Show("Недостаточно средств для перевода");
                }
            }

            if (type == "NonDepositBill")
            {
                if (someBill.Ammount >= sum)
                {
                    nonDepositBill.Ammount += sum;
                }
                else
                {
                    MessageBox.Show("Недостаточно средств для перевода");
                }
            }

            return this;
        }
    }
}
