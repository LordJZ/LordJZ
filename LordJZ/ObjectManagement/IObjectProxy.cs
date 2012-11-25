
namespace LordJZ.ObjectManagement
{
    public interface IObjectProxy
    {
        object ProxifiedObject { get; }
    }

    public interface IObjectProxy<out T> : IObjectProxy
    {
        new T ProxifiedObject { get; }
    }
}
