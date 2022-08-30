using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Skillbox_Homework_13._1
{
    class Client<T> : IShowBill<T>, IChangeBill<T>
    {
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }

        public T bill1;
        public T bill2;

        public Client(string secondName, string firstName, string patronymic)
        {
            this.SecondName = secondName;
            this.FirstName = firstName;
            this.Patronymic = patronymic;
        }

        T IShowBill<T>.ShowBill(int billNumber)
        {
            if (billNumber == 1)
            {
                return bill1;
            }

            if (billNumber == 2)
            {
                return bill2;
            }

            throw new Exception("Выберите 1 или 2 счёт");
        }

        void IChangeBill<T>.ChangeBill(T bill, int billNumber)
        {
            if (billNumber == 1)
            {
                bill1 = bill;
            }

            if (billNumber == 2)
            {
                bill2 = bill;
            }

            if (billNumber != 1 && billNumber != 2)           
            {
                throw new Exception("Выберите 1 или 2 счёт");
            }
        }
    }
}
