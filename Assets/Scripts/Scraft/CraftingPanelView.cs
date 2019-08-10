using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成面板view层
/// </summary>
public class CraftingPanelView : MonoBehaviour {

    private Transform tranform;

    private Transform tabTransform;
    private GameObject craftingTabsItem;

    private Transform contentsTransform;
    private GameObject craftingContent;
    private GameObject craftingContentItem;

    private Transform centerTransform;
    private GameObject craftingSlot;

    private GameObject craftingItem;

    private Dictionary<string, Sprite> tabIconDic = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> materialDic = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> itemDic = new Dictionary<string, Sprite>();

    public Transform Tranform { get { return tranform; } }
    public Transform TabTransform { get { return tabTransform; } }
    public Transform ContentsTransform { get { return contentsTransform; } }
    public GameObject CraftingTabsItem { get { return craftingTabsItem; } }
    public GameObject CraftingContent { get { return craftingContent; } }
    public GameObject CraftingContentItem { get { return craftingContentItem; } }
    public Transform CenterTransform { get { return centerTransform; } }
    public GameObject CraftingSlot { get { return craftingSlot; } }

    public GameObject CraftingItem { get { return craftingItem; } }

    void Awake () {
        tranform = gameObject.GetComponent<Transform>();
        tabTransform = tranform.Find("Left/Tabs").GetComponent<Transform>();
        contentsTransform = tranform.Find("Left/Contents").GetComponent<Transform>();
        centerTransform = tranform.Find("Center").GetComponent<Transform>();
        craftingTabsItem = Resources.Load<GameObject>("CraftPanel/CraftingTabsItem");
        craftingContent = Resources.Load<GameObject>("CraftPanel/CraftingContent");
        craftingContentItem = Resources.Load<GameObject>("CraftPanel/CraftingContentItem");
        craftingSlot = Resources.Load<GameObject>("CraftPanel/CraftingSlot");
        craftingItem = Resources.Load<GameObject>("Item/InVentoryItem");
        //加载icon资源
        LoadAndInit();
    }

    /// <summary>
    /// 加载所有icon资源和初始化list
    /// </summary>
    private void LoadAndInit()
    {
        tabIconDic = ResourcesTools.LoadFolderAssets("TabIcon", tabIconDic);
        materialDic = ResourcesTools.LoadFolderAssets("Material", materialDic);
        itemDic = ResourcesTools.LoadFolderAssets("Item", itemDic);
    }

    /// <summary>
    /// 通过名称查找Tab文件夹里的Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetTabIconByName(string name)
    {
        return ResourcesTools.GetSpriteByName(name, tabIconDic);
    }

    /// <summary>
    /// 通过名称查找Material文件夹里的Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetMaterialIconByName(string name)
    {
        return ResourcesTools.GetSpriteByName(name, materialDic);
    }

    /// <summary>
    /// 通过名称查找Item文件夹里的Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemIconByName(string name)
    {
        return ResourcesTools.GetSpriteByName(name, itemDic);
    }

    /// <summary>
    /// 返回tabIcon的数量
    /// </summary>
    /// <returns></returns>
    public int GetTabIconDicLength()
    {
        return tabIconDic.Count;
    }

    /// <summary>
    /// 返回MeterialIcon的数量
    /// </summary>
    /// <returns></returns>
    public int GetMeterialDicLength()
    {
        return materialDic.Count;
    }
}
