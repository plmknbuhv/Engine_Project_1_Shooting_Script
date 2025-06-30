using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ShipData
{
    public Sprite EngineSprite;
    public Sprite WeaponSprite;
}

[CreateAssetMenu(menuName = "SO/ShipDataSO")]
public class ShipDataListSO : ScriptableObject
{
    public List<ShipData> dataList;
}
