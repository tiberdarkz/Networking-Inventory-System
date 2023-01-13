using System.Linq;
using UnityEngine;

public class ItemList
{
    private Item[] itemList;

    public ItemList()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        itemList = Resources.LoadAll<Item>("Items");
    }
    
    public Item FindItemById(int id)
    {
        return itemList.FirstOrDefault(item => item.id == id);
    }
}
