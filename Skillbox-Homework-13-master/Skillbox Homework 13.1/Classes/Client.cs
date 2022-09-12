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
        public delegate void ClientInformation(string message);

        public static event ClientInformation? ClientNotification;        

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

            ClientNotification?.Invoke($"{DateTime.Now} Создан клиент с именем {SecondName} {FirstName} {Patronymic}");
        }

        public Client Transfer(Bill someBill, float sum, string type)
        {
            if (type == "DepositBill")
            {
                if (someBill.Ammount >= sum)
                {
                    depositBill.Ammount += sum;

                    ClientNotification?.Invoke($"{DateTime.Now} Клиенту {SecondName} {FirstName} {Patronymic} переведено на депозитный счёт {sum}");
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

                    ClientNotification?.Invoke($"{DateTime.Now} Клиенту {SecondName} {FirstName} {Patronymic} переведено на недепозитный счёт {sum}");
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
