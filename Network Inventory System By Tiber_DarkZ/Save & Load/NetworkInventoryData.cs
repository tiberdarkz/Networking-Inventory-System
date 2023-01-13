using Unity.Netcode;

[System.Serializable]
public struct NetworkInventoryData : INetworkSerializable
{
    public int inventorysId;
    public int inventorySlotSizes;
    public SlotData[] slots;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref inventorysId);
        serializer.SerializeValue(ref inventorySlotSizes);
        serializer.SerializeValue(ref slots);
    }
    
    public SlotData[] GenerateSlots(int length)
    {
        slots = new SlotData[length];
        
        for (int i = 0; i < length; i++)
        {
            slots[i].slotId = i;
            slots[i].itemAmount = 0;
            slots[i].itemId = 0;
        }

        return slots;
    }
}


[System.Serializable]
public struct SlotData : INetworkSerializable
{
    public int slotId;
    public int itemAmount;
    public int itemId;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref slotId);
        serializer.SerializeValue(ref itemAmount);
        serializer.SerializeValue(ref itemId);
    }
}
