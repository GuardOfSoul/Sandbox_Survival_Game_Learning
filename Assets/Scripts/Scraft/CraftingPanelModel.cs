using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// 合成面板model层
/// </summary>
public class CraftingPanelModel : MonoBehaviour {

    private Dictionary<int, CraftingMapItem> mapItemDic;

    void Awake () {
        mapItemDic = LoadJsonDataForMapItem("CraftingMapJsonData");

    }

    public string[] GetTabsItemName()
    {
        return  new string[] { "Icon_House", "Icon_Weapon" };
    }

    /// <summary>
    /// 获取Item的json数据并转换
    /// </summary>
    /// <param name="name">json文件名称</param>
    /// <returns>返回含有实体数据类的list的list</returns>
    public List<List<CraftingContentItem>> GetJsonDataByName(string name)
    {
        List<List<CraftingContentItem>> temp = new List<List<CraftingContentItem>>();
        string jsonStr= Resources.Load<TextAsset>("JsonData/"+ name).text;
        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            List<CraftingContentItem> tempList = new List<CraftingContentItem>();
            JsonData jd = jsonData[i]["Type"];
            for (int j = 0; j < jd.Count; j++)
            {
                tempList.Add(JsonMapper.ToObject<CraftingContentItem>(jd[j].ToJson()));
            }
            temp.Add(tempList);
        }

        return temp;
    }

    /// <summary>
    /// 加载合成图谱的json数据并转换
    /// </summary>
    /// <param name="name">json数据的文件名</param>
    /// <returns>包含图谱数据的list</returns>
    private Dictionary<int,CraftingMapItem> LoadJsonDataForMapItem(string name)
    {
        Dictionary<int, CraftingMapItem> tempList = new Dictionary<int, CraftingMapItem>();
        string jsonStr = Resources.Load<TextAsset>("JsonData/" + name).text;
        JsonData json = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < json.Count; i++)
        {
            int mapId = int.Parse(json[i]["MapId"].ToString());
            int materialsCount = int.Parse(json[i]["MaterialsCount"].ToString());
            int mapBar = int.Parse(json[i]["MapBar"].ToString());
            string[] mapContens = json[i]["MapContents"].ToString().Split(','); //主要是要把这个分割成数组
            string mapName = json[i]["MapName"].ToString();
            tempList.Add(mapId,new CraftingMapItem(mapId, mapContens, mapName,materialsCount, mapBar));
        }
        return tempList;
    }

    /// <summary>
    /// 通过id获取对应的合成图谱
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CraftingMapItem GetItemById(int id)
    {
        CraftingMapItem temp = null;
        mapItemDic.TryGetValue(id, out temp);
        return temp;
    }
}
