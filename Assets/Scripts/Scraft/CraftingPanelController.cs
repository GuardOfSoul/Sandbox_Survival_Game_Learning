using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成面板control层
/// </summary>
public class CraftingPanelController : MonoBehaviour,IUIPanelShowHide {

    public static CraftingPanelController Instance;
    private Transform transform;

    private CraftingPanelModel craftingPanelModel;
    private CraftingPanelView craftingPanelView;
    private CraftingController craftingController;

    private int tabsNum = -1;
    private int craftingSlotNum = 25;
    private List<GameObject> tabsList = new List<GameObject>();//合成面板.类别选择区域.GameObject列表
    private List<GameObject> contentsList = new List<GameObject>(); //合成面板.合成列表区域.GameObject列表
    private List<GameObject> slotList = new List<GameObject>(); //合成面板.中央(合成表)区域.GameObject列表
    private List<GameObject> slotItemList = new List<GameObject>();//合成面板中已经放置的材料
    private List<GameObject> materialsList = new List<GameObject>();//存放管理已经放置的材料

    private int curIndex=-1; //当前选中区域
    private int materialsCount = 0;//合成所需的素材数量
    private int dragMaterialsCount = 0;//已经放置的素材的数量

    void Awake()
    {
        Instance = this;
    }
    void Start () {
        Init();
        gameObject.SetActive(false);
    }

	void Update () {

        if (dragMaterialsCount == materialsCount&& materialsCount!=0)
        {
            craftingController.ActiveButton();
        }
        else
        {
            craftingController.InitButton();
        }
    }

    /// <summary>
    /// 初始化所有查找
    /// </summary>
    private void Init()
    {
        transform = gameObject.GetComponent<Transform>();
        craftingController = transform.Find("Right").GetComponent<CraftingController>();
        craftingPanelModel = gameObject.GetComponent<CraftingPanelModel>();
        craftingPanelView = gameObject.GetComponent<CraftingPanelView>();
        tabsNum = craftingPanelView.GetTabIconDicLength(); //根据icon的数量创建tabs
        CreatAllTabsAndContents(tabsNum);
        CreatCraftingSlots(craftingSlotNum);
        ResetTabsAndContent(0);
        craftingController.CraftingItemPrefab = craftingPanelView.CraftingItem;
    }

    /// <summary>
    /// 创建Tabs和contents
    /// </summary>
    private void CreatAllTabsAndContents(int num)
    {
        List<List<CraftingContentItem>> tempList = craftingPanelModel.GetJsonDataByName("CraftingContentsJsonData");
        string[] tempSpriteName = craftingPanelModel.GetTabsItemName();
        for (int i = 0; i < num; i++)
        {
            //实例化tab
            GameObject tempTabs= GameObject.Instantiate(craftingPanelView.CraftingTabsItem, craftingPanelView.TabTransform);
            Sprite tempIcon = craftingPanelView.GetTabIconByName(tempSpriteName[i]);
            tempTabs.GetComponent<CraftingTabItemController>().InitItem(i,tempIcon);
            tabsList.Add(tempTabs);

            //实例化content
            GameObject tempContents = GameObject.Instantiate(craftingPanelView.CraftingContent, craftingPanelView.ContentsTransform);
            tempContents.GetComponent<CraftingContentController>().InitContent(i,craftingPanelView.CraftingContentItem, tempList[i]);
            contentsList.Add(tempContents);
        }
    }

    /// <summary>
    /// 重置tabs和正文区域
    /// </summary>
    private void ResetTabsAndContent(int index)
    {
        //限制重复执行
        if (curIndex!=index)
        {
            //重置tabs与contents 
            for (int i = 0; i < tabsNum; i++)
            {
                tabsList[i].GetComponent<CraftingTabItemController>().NormalTab();
                contentsList[i].SetActive(false);
            }
            tabsList[index].GetComponent<CraftingTabItemController>().ActiveTab();
            contentsList[index].SetActive(true);
            curIndex = index;
        }
    }

    /// <summary>
    /// 创建合成表区域
    /// </summary>
    /// <param name="num"></param>
    private void CreatCraftingSlots(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject temp= GameObject.Instantiate(craftingPanelView.CraftingSlot, craftingPanelView.CenterTransform);
            temp.name = "CraftSlot" + i;
            slotList.Add(temp);
        }
    }

    /// <summary>
    /// 生成合成图谱
    /// </summary>
    /// <param name="id"></param>
    private void CreateSlotContents(int id)
    {
        //Debug.Log(id);
        CraftingMapItem tempItem = craftingPanelModel.GetItemById(id);
        //重置材料
        ResetSlotItems();
        dragMaterialsCount = 0;
        //重置按钮
        craftingController.InitButton();
        //重置合成图谱
        ResetSlotContents();
        //重新生成图谱
        if (tempItem != null)
        {
            for (int j = 0; j < tempItem.MapContents.Length; j++)
            {
                if (tempItem.MapContents[j] != "0")
                {
                    Sprite sprite = GetItemSprite(tempItem.MapContents[j]);
                    slotList[j].GetComponent<CraftingSlotController>().Init(sprite, int.Parse(tempItem.MapContents[j]));
                    slotList[j].GetComponent<CraftingSlotController>().SetSlotState(true);
                }
            }
            //初始化需要的素材数量
            materialsCount = tempItem.MaterialsCount;
            //显示最终合成物品
            craftingController.Init(craftingPanelView.GetItemIconByName(tempItem.MapName), tempItem.MapName, tempItem.MapId,tempItem.MapBar);
        }
        else materialsCount = 0;
    }

    /// <summary>
    /// 重置合成图谱
    /// </summary>
    private void ResetSlotContents()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].GetComponent<CraftingSlotController>().ResetSlot();
        }
        
    }

    /// <summary>
    /// 重置已经放上图谱的物品
    /// </summary>
    private void ResetSlotItems()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Transform transform = slotList[i].GetComponent<Transform>().Find("InVentoryItem");
            if (transform!=null)
            {
                slotItemList.Add(transform.gameObject);
            }
        }
        InventoryPanelController.Instance.AddItems(slotItemList);
        slotItemList.Clear();
    }

    /// <summary>
    /// 对拖入的材料进行管理
    /// </summary>
    /// <param name="item"></param>
    public void DargMaterial(GameObject item)
    {
        materialsList.Add(item);
        ++dragMaterialsCount;
    }

    /// <summary>
    /// 获取item文件夹里的sprite
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite GetItemSprite(string name)
    {
       return craftingPanelView.GetItemIconByName(name);
    }
    /// <summary>
    /// 过渡方法，调用InventoryPanelController里的方法
    /// </summary>
    public void InitItem(GameObject go,string name,int num,int id,int bar)
    {
        InventoryPanelController.Instance.SendInitItemMethod(go,name,num,id,bar);
    }

    public void UseMaterial()
    {
        for (int i = 0; i < materialsList.Count; i++)
        {
            if (!InventoryPanelController.Instance.SetItemNum(materialsList[i]))
            {
                materialsList.Remove(materialsList[i]);
                --i;
                --dragMaterialsCount;
            }
        }
    }

    public void UIPanelShow()
    {
        gameObject.SetActive(true);
    }

    public void UIPanelHide()
    {
        gameObject.SetActive(false);
    }
}
 
