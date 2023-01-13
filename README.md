# Networking Inventory System
The following is created by Tiber_DarkZ with the backbone of the project being carlboisvertdev's tutorial on Unity Netcode For GameObject series.
The project does not rely on this system, but excpects you to already have the grasps on the networking part of Unity.

I highly recommand anyway trying to use this code under the license to also check out fellow developer carlboisvertdev's Multiplayer with UGS repository and youtube tutorials:
https://github.com/carlboisvertdev/UnityMultiplayerWithUGS

# How do i setup the main system?
- Add a DataPersistanceManager to your scene and add a filename
- Create a function to update the StoreFileDataManager.Instance.IsHost whenever the host starts a game (This can be done in the main menu!)
- Call the DataPersistanceManager.Save() function whenever the host saves the game. Data is not saved INSTANTLY!
- Create a chest with a collider & the ChestController component on it.
- Create a prefab for your itemslots. This must contain the following stucture:
  * Chest (SpriteRenderer, ChestController, BoxCollider)
     * ChestUI (Canvas)
         * Inventory Parent (Grid Layout Group)

- Setup the values of the ChestController
- Load your scene with one player, and add a second player, and chests will sync over the network!

# How do i add a custom inventory?
- Just make a Controller script that inherits from NetworkInventory.
- Whenever you wanne update the inventory, call UpdateInventoryServerRpc(int identifier, int length, Slot[] slots);
- Make sure no other inventorys use the identifier you give it
- Handle your inventory however you want, use the ChestController as an example!

# Where can i find support?
https://discord.gg/vxH97zNp8N
