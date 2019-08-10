using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
/// 背包model层
/// </summary>
public class InventoryPanelModel : MonoBehaviour {

	void Awake () {
		
	}

    /// <summary>
    /// 通过json文件名获取角色物品栏物品信息
    /// </summary>
    /// <param name="fileName">json文件名</param>
    /// <returns>包含角色物品栏信息的List对象</returns>
    public List<InventoryItem> GetJsonList(string fileName)
    {
        return JsonTools.LoadJsonFile<InventoryItem>(fileName);
    }
}
