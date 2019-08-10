using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包control层
/// </summary>
public class InventoryPanelController : MonoBehaviour,IUIPanelShowHide {

    public static InventoryPanelController Instance;

    private InventoryPanelModel inventoryPanelModel;
    private InventoryPanelView inventoryPanelView;

    private int slotNumber = 27; //背包物品槽数量
    private List<GameObject> slotLists = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }
    void Start () {
        inventoryPanelView = gameObject.GetComponent<InventoryPanelView>();
        inventoryPanelModel = gameObject.GetComponent<InventoryPanelModel>();
        CreateAllSlot();
        CreateALLItem();
        gameObject.SetActive(false);
    }
	
	void Update () {
		
	}

    /// <summary>
    /// 生成背包物品槽。
    /// </summary>
    private void CreateAllSlot()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            GameObject tempSlot= GameObject.Instantiate(inventoryPanelView.PrefabSlot, inventoryPanelView.GridTransform);
            tempSlot.name = "BagSlot" + i;
            slotLists.Add(tempSlot);    
        }
    }
    /// <summary>
    /// 创建人物背包物品
    /// </summary>
    private void CreateALLItem()
    {
        //测试数据
        /*List<InventoryItem> tempList = new List<InventoryItem>
        {
            new InventoryItem("Torch", 50),
            new InventoryItem("Axe", 79),
            new InventoryItem("Arrow", 99)
        };
        */
        List<InventoryItem> tempList = inventoryPanelModel.GetJsonList("InventoryJsonData");
        for (int i = 0; i < tempList.Count; i++)
        {
            GameObject tempItem = GameObject.Instantiate(inventoryPanelView.PrefabItem, slotLists[i].GetComponent<Transform>());
            tempItem.GetComponent<InventoryItemController>().InitItem(tempList[i].ItemName, tempList[i].ItemNum,tempList[i].ItemId,tempList[i].ItemBar);
        }
    }
    /// <summary>
    /// 从合成槽将物品放回背包
    /// </summary>
    public void AddItems(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            RectTransform tempTransform = list[i].GetComponent<RectTransform>();
            for (int j = 0; j < slotLists.Count; j++)
            {
                Transform bagTemp = slotLists[j].GetComponent<Transform>().Find("InVentoryItem");
                if (bagTemp != null)
                {
                    InventoryItemController bagCtrl = bagTemp.gameObject.GetComponent<InventoryItemController>();
                    InventoryItemController craftCtrl = list[i].GetComponent<InventoryItemController>();
                    if (bagCtrl.Id == craftCtrl.Id)
                    {
                        bagCtrl.InitNum(craftCtrl.Num + bagCtrl.Num);
                        GameObject.Destroy(list[i]);
                        break;
                    }
                }
                else
                {
                    tempTransform.SetParent(slotLists[j].GetComponent<Transform>());
                    tempTransform.localPosition = Vector3.zero;
                    tempTransform.GetComponent<InventoryItemController>().ResetSpriteSize(tempTransform, 85, 85);
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 返回背包列表
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetInventorySlotList()
    {
        return slotLists;
    }

    /// <summary>
    /// 过渡方法，调用CraftingPanelController单例脚本的方法
    /// </summary>
    public void SendDargMaterialItem(GameObject gameObject)
    {
        CraftingPanelController.Instance.DargMaterial(gameObject);
    }
    /// <summary>
    /// 过渡方法，给CraftingPanelController单例脚本调用
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="num"></param>
    /// <param name="id"></param>
    public void SendInitItemMethod(GameObject go, string name, int num, int id,int bar)
    {
        go.GetComponent<InventoryItemController>().InitItem(name,num,id,bar);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    public bool SetItemNum(GameObject go)
    {
        go.GetComponent<InventoryItemController>().InitNum(go.GetComponent<InventoryItemController>().Num-1);
        if (go.GetComponent<InventoryItemController>().Num==0)
        {
            return false;
        }
        return true;
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
