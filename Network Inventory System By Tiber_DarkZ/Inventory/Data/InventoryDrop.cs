using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : NetworkBehaviour, IDropHandler
{
    public int slotNumber;
    public NetworkInventory partOfInventory;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (transform.childCount != 0) return;
        
        InventoryDrag draggableItem = dropped.GetComponent<InventoryDrag>();
        draggableItem.parentAfterDrag = transform;
        
        partOfInventory.SetItem(slotNumber, draggableItem.itemId, draggableItem.itemAmount);
        partOfInventory.RemoveItem(draggableItem.slotId);

        draggableItem.slotId = slotNumber;
    }
}
