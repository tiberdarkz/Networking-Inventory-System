using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite itemUIimage;
}