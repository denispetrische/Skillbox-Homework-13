
namespace InterfaceLibrary
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
