    using TMPro;
    using Unity.Netcode;
    using UnityEngine;
    using UnityEngine.UI;

    public class NetworkInventory : NetworkBehaviour, IDataPersistance
    {
        public int identifier;
        public int length;
        public GameObject slot;
        public GameObject UIParent;

        public NetworkVariable<NetworkInventoryData> inventory = new();
        public NetworkInventoryData localInventory;
        
        [ServerRpc(RequireOwnership = false)]
        protected void UpdateInventoryServerRpc(int id, int slotSize, SlotData[] pSlots)
        {
            inventory.Value = new NetworkInventoryData
            {
                inventorysId = id,
                inventorySlotSizes = slotSize,
                slots = pSlots
            };
        }

        protected virtual void OnInventoryUpdated(NetworkInventoryData previousvalue, NetworkInventoryData newvalue) { }

        protected void AddSlot(SlotData data)
        {
            GameObject obj = Instantiate(slot, UIParent.transform);
            Image image = obj.GetComponentsInChildren<Image>()[1];
            Item item = new ItemList().FindItemById(data.itemId);
            image.sprite = item.itemUIimage;

            if(data.itemId == 0) Destroy(image.gameObject);
            InventoryDrag drag = image.gameObject.AddComponent<InventoryDrag>();
            drag.root = GetComponentInChildren<Canvas>().gameObject.transform;
            drag.image = image;
            drag.itemId = data.itemId;
            drag.itemAmount = data.itemAmount;
            drag.slotId = data.slotId;

            InventoryDrop drop = obj.GetComponent<InventoryDrop>();
            drop.slotNumber = data.slotId;
            drop.partOfInventory = this;

            TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
            text.text = "x" + data.itemAmount;
        }
        
        public void SetItem(int slotId, int itemId, int amount)
        {
            localInventory.slots[slotId].itemId = itemId;
            localInventory.slots[slotId].itemAmount = amount;
        }

        public void RemoveItem(int slotId)
        {
            localInventory.slots[slotId].itemId = 0;
            localInventory.slots[slotId].itemAmount = 0;
        }
        
        public void LoadData(GameData data)
        {
            bool succes = false;
            foreach (var inv in data.inventorys)
            {
                if (inv.Key != identifier) continue;
                succes = true;
                NetworkInventoryData invData = inv.Value;
                UpdateInventoryServerRpc(invData.inventorysId, invData.inventorySlotSizes, invData.slots);
            }

            if (!succes)
                UpdateInventoryServerRpc(identifier, length, inventory.Value.GenerateSlots(length));
        }

        public void SaveData(ref GameData data)
        {
            if (data.inventorys.ContainsKey(identifier))
            {
                data.inventorys.Remove(identifier);
            }
            
            data.inventorys.Add(identifier, inventory.Value);
        }
    }