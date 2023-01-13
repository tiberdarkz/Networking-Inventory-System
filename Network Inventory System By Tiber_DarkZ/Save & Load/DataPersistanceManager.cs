using System.Collections;
using System.Collections.Generic;
using GameFramework.Core;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public IEnumerator Start()
    {
        if (!StoreFileDataManager.Instance.isHost) yield break;

        yield return new WaitForSeconds(3f);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void NewGame()
    {
        this.gameData = new GameData();
        gameData.inventorys.Add(-1, new NetworkInventoryData()
        {
            inventorysId = -1,
            inventorySlotSizes = 7,
            slots = new SlotData[7]
        });
        
        gameData.inventorys.Add(-2, new NetworkInventoryData()
        {
            inventorysId = -2,
            inventorySlotSizes = 7,
            slots = new SlotData[7]
        });
    }

    private void LoadGame()
    {
        this.gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }

    private void SaveGame()
    {
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }
        
        dataHandler.Save(gameData);
    }
    
    
    private List<IDataPersistance> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistance> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistenceObjects);
    }

    public void OnSave()
    {
        if (!StoreFileDataManager.Instance.isHost) return;
        SaveGame();
    }
}
