

namespace InterfaceLibrary
{
    public interface ITransfer<in T>
    {
        Client Transfer(T someBill, float sum, string type);
    }
}
