using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoreFileDataManager : GameFramework.Core.Singleton<StoreFileDataManager>
{
    public string filename;
    public bool isHost;
}
