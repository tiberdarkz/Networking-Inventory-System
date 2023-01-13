using System.Collections;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventoryController : NetworkInventory
    {
        private bool isOwner;

        public override void OnNetworkSpawn()
        {
            if (StoreFileDataManager.Instance.isHost) identifier = -2;
            else identifier = -1;
            
            inventory.OnValueChanged += OnInventoryUpdated;
            
            if (StoreFileDataManager.Instance.isHost) return;
            StartCoroutine(OnFirstSpawnClient());
        }

        private IEnumerator OnFirstSpawnClient()
        {
            yield return new WaitForSeconds(3f);
            RefreshChest();
        }

        //TODO - Update the inventory on the host
        //UpdateInventoryServerRpc(identifier, length, localInventory.slots);
        
        private void RefreshChest()
        {
            foreach (Transform child in UIParent.transform) { Destroy(child.gameObject); }

            for (int i = 0; i < length; i++)
            {
                AddSlot(inventory.Value.slots[i]);
            }
        }

        protected override void OnInventoryUpdated(NetworkInventoryData previousvalue, NetworkInventoryData newvalue)
        {
            RefreshChest();
        }
    }
}