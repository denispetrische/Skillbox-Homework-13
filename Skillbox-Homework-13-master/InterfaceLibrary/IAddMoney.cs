
namespace InterfaceLibrary
{
    public interface IAddMoney<out T>
    {
        public T AddMoney(float sumDeposit);
    }
}
