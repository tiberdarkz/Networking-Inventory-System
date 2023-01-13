    using System.Collections;
    using Unity.Netcode;
    using UnityEngine;

    public class ChestController : NetworkInventory
    {
        private NetworkVariable<bool> InUse = new();
        private bool isOwner;

        public override void OnNetworkSpawn()
        {
            inventory.OnValueChanged += OnInventoryUpdated;
            
            if (StoreFileDataManager.Instance.isHost) return;
            StartCoroutine(OnFirstSpawnClient());
        }

        private IEnumerator OnFirstSpawnClient()
        {
            yield return new WaitForSeconds(3f);
            RefreshChest();
        }

        private void OpenChest()
        {
            localInventory = inventory.Value;
            UIParent.SetActive(true);
            isOwner = true;
        }

        private void CloseChest()
        {
            UpdateInventoryServerRpc(identifier, length, localInventory.slots);
            UIParent.SetActive(false);
            isOwner = false;
        }

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

        [ServerRpc(RequireOwnership = false)]
        private void SetInUseServerRpc(bool state)
        {
            InUse.Value = state;
        }

        public void OnInteract()
        {
            if (InUse.Value && !isOwner) { return; }

            if (!UIParent.activeSelf)
            {
                SetInUseServerRpc(true);
                OpenChest();
            }
            else
            {
                SetInUseServerRpc(false);
                CloseChest();
            }
        }
    }
    