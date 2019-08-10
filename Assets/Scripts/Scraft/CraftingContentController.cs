using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制单个content区域和其所有item
/// </summary>
public class CraftingContentController : MonoBehaviour {

    private Transform transform;
    private int contentindex = -1; //content的编号
    private int itemsCount = -1;//content的items的数量

    private CraftingContentItemController curActiveContenItem = null;

    void Awake () {
        transform = gameObject.GetComponent<Transform>();
	}

	void Update () {
		
	}

    /// <summary>
    /// 初始化content
    /// </summary>
    /// <param name="index">content编号</param>
    /// <param name="prefab">content预制体</param>
    public void InitContent(int index,GameObject prefab, List<CraftingContentItem> strList)
    {
        this.contentindex = index;
        this.itemsCount = strList.Count;
        gameObject.name = "content" + index;
        CreateALLItems(strList, prefab);
    }

    /// <summary>
    /// 创建每个content里的所有items
    /// </summary>
    /// <param name="count">item数量</param>
    /// <param name="prefab">item的预制体</param>
    private void CreateALLItems(List<CraftingContentItem> strList,GameObject prefab)
    {

        for (int i = 0; i < strList.Count; i++)
        {
           GameObject temp=  GameObject.Instantiate(prefab, transform);
            temp.GetComponent<CraftingContentItemController>().InitItem(strList[i]);
        }
    }

    /// <summary>
    /// 重置正文区域
    /// </summary>
    /// <param name="item"></param>
    private void ResetItemState(CraftingContentItemController item)
    {
        if (curActiveContenItem!=item)
        {
            if (curActiveContenItem != null)
            {
                curActiveContenItem.SetNormal();
            }
            item.SetActive();
            curActiveContenItem = item;
            SendMessageUpwards("CreateSlotContents", item.Id);
        }
        
    }
}
