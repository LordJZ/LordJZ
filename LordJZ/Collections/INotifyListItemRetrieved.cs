
namespace LordJZ.Collections
{
    public delegate void NotifyListItemRetrievedHandler(object collection, int index);

    public interface INotifyListItemRetrieved
    {
        event NotifyListItemRetrievedHandler ListItemRetrieved;
    }
}
