using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成图谱数据实体类
/// </summary>
public class CraftingMapItem {
    private int mapId;
    private int materialsCount;
    private string[] mapContents;
    private string mapName;
    private int mapBar;

    public int MapId { get { return mapId; } set { mapId = value; } }
    public string[] MapContents { get { return mapContents; } set { mapContents = value; } }
    public string MapName { get { return mapName; } set { mapName = value; } }
    public int MaterialsCount { get { return materialsCount; } set { materialsCount = value; } }
    public int MapBar { get { return mapBar; } set { mapBar = value; } }

    public CraftingMapItem() { mapId = -1; mapContents = null; mapName = null; }

    public CraftingMapItem(int mapId, string[] mapContents, string mapName,int materialsCount,int mapBar)
    {
        this.mapId = mapId;
        this.mapContents = mapContents;
        this.mapName = mapName;
        this.materialsCount = materialsCount;
        this.mapBar = mapBar;
    }

    public override string ToString()
    {
        return string.Format("mapId:{0}, mapContents:{1}, mapName:{2},materialsCount:{3},bar:{4}", mapId, mapContents.Length, mapName, materialsCount, mapBar);
    }
}
