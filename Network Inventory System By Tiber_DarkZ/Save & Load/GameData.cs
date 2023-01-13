using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    
    public SerializableDictionary<int, NetworkInventoryData> inventorys;

    public GameData()
    {
        inventorys = new SerializableDictionary<int, NetworkInventoryData>();
    }
}
