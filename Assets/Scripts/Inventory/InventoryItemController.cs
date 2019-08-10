using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 单个背包物品控制器
/// </summary>
public class InventoryItemController : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler {

    private RectTransform transform; 
    private Image image;                    //物品图标
    private Text text;                      //物品数量文本
    private Image barImage;
    private Transform topParent;            //持有InventoryPanel，充当临时父物体
    private CanvasGroup canvasGroup;        //item子物体的canvasGroup

    private Transform curParent;            //当前背包格，物品在背包的最终父物体
    private int id;                         //物品id
    private bool isDrag=false;              //拖拽状态
    private bool isCopy = false;            //复制状态 true为已复制一次，false为未复制，因为多次复制会在同一个槽内生成
    private GameObject targetSlot;
    private int num = 0;
    private int bar = 0;                    //当前物品是否需要耐久条 0表示不需要
    public int Id { get { return id; } }
    public int Num { get { return num; } }
    public Transform CurParent { get { return curParent; } set { curParent = value; } }

    void Awake () {
        FindInit();
    }

    void Update()
    {
        //拖拽并按下tab拆分一半
        if (isDrag && Input.GetKeyDown(KeyCode.Tab) && isCopy == false) 
        {
            BreakMaterials();
            //已复制，不能再复制
            isCopy = true;
        }
        //拖拽按下鼠标右键放下一个物品
        if (isDrag && Input.GetMouseButtonDown(1)) 
        {

            PutOneInSlot(targetSlot);
        }
        if (num==0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// 所有查找相关的初始化
    /// </summary>
    private void FindInit()
    {
        transform = gameObject.GetComponent<RectTransform>();
        image = gameObject.GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
        barImage = transform.Find("Bar").GetComponent<Image>();
        topParent = GameObject.Find("Canvas").GetComponent<Transform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        CurParent = transform.parent;
    }
    /// <summary>
    /// 初始化item
    /// </summary>
    /// <param name="name"></param>
    /// <param name="num"></param>
    public void InitItem(string name, int num,int id,int bar)
    {
        image.sprite = Resources.Load<Sprite>("Item/" + name);
        this.bar = bar;
        InitNum(num);
        InitId(id);
        InitName();
        BarOrNum();
    }
    /// <summary>
    /// 初始化id
    /// </summary>
    /// <param name="id"></param>
    public void InitId(int id)
    {
        this.id = id;
    }
    /// <summary>
    /// 初始化num
    /// </summary>
    /// <param name="num"></param>
    public void InitNum(int num)
    {
        this.num = num;
        text.text = num.ToString();
    }
    /// <summary>
    /// 初始化name
    /// </summary>
    /// <param name="name"></param>
    public void InitName()
    {
        gameObject.name = "InVentoryItem";
    }
    private void BarOrNum()
    {
        if (bar==0)
        {
            barImage.gameObject.SetActive(false);
        }
        else if (bar==1)
        {
            text.gameObject.SetActive(false);
        }
    }
    public void UpDateUI(float value)
    {
        barImage.fillAmount = value;
        if (value<=0)
        {
            transform.parent.GetComponent<ToolBarSlotController>().SwitchButtonState();
            ToolBarPanelController.Instance.CurWeapen = null;
            ToolBarPanelController.Instance.CurSlot = null;
            GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// 重置物品尺寸
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void ResetSpriteSize(RectTransform transform, float width, float height)
    {
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    //实现拖拽功能
    /// <summary>
    /// 拖拽开始
    /// </summary>
    /// <param name="eventData"></param>
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("being Drag");
        transform.SetParent(topParent);
        canvasGroup.blocksRaycasts = false;
        isDrag = true;
    }
    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        targetSlot = eventData.pointerEnter;
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform, eventData.position, eventData.enterEventCamera, out pos);
        transform.position = pos;

    }
    /// <summary>
    /// 拖拽结束，进行安全判断
    /// </summary>
    /// <param name="eventData"></param>
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;
        ItemDrag(target);
    }

    //拖拽子功能
    /// <summary>
    /// 组合物品
    /// </summary>
    private void GroupMaterials(InventoryItemController ctrl)
    {
        ctrl.InitNum(ctrl.Num+num);
        if (ctrl.Num>64)
        {
            Debug.Log("hhh");
            CreateAndSettleNewItem(ctrl.Num - 64);
            ctrl.InitNum(64);
        }
        GameObject.Destroy(gameObject);
    }
    /// <summary>
    /// 拆分物品
    /// </summary>
    private void BreakMaterials()
    {
        if (num >= 2)
        {
            int tempNum = num / 2;
            GameObject temp = GameObject.Instantiate<GameObject>(gameObject, CurParent);
            //复制物品
            InitNewItem(temp, tempNum, curParent.tag); //复制数量为tempnum的物体
            InitNum(tempNum);
        }
    }
    /// <summary>
    /// 向槽里放置一个物品
    /// </summary>
    private void PutOneInSlot(GameObject targetSlot)
    {
        //槽里有物品
        if (targetSlot.tag== "InVentoryItem")
        {
            InventoryItemController tempCtrl = targetSlot.GetComponent<InventoryItemController>();
            //槽里物品id与当前拖拽物品id一致
            if (tempCtrl.Id==id)
            {
                tempCtrl.InitNum(tempCtrl.Num+1);       //槽内物品数量加一
                InitNum(num - 1);                       //拖拽中物品数量减一
                if (num == 0)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }
        //槽里没有物品且可放置（合成槽有放置规则）
        else if (targetSlot.tag== "InVentorySlot" ||(targetSlot.tag == "CraftingSlot" && targetSlot.GetComponent<CraftingSlotController>().IsOpen && id == targetSlot.GetComponent<CraftingSlotController>().Id))
        {
            if (targetSlot.GetComponent<Transform>().Find("InVentoryItem") ==null)
            {
                GameObject go = GameObject.Instantiate(gameObject, targetSlot.GetComponent<RectTransform>());
                //复制物品
                InitNewItem(go, 1, targetSlot.tag);         //创建新的数量为1的物品放进槽中
                InitNum(num - 1);                           //拖拽中物品数量减一
                InventoryPanelController.Instance.SendDargMaterialItem(go);

            }
            else if(targetSlot.GetComponent<Transform>().Find("InVentoryItem") !=null)
            {
                InventoryItemController tempCtrl = targetSlot.GetComponent<Transform>().Find("InVentoryItem").GetComponent<InventoryItemController>();
                //槽里物品id与当前拖拽物品id一致
                if (tempCtrl.Id == id)
                {
                    tempCtrl.InitNum(tempCtrl.Num + 1);       //槽内物品数量加一
                    InitNum(num - 1);                       //拖拽中物品数量减一
                    if (num == 0)
                    {
                        GameObject.Destroy(gameObject);
                    }
                }
            }
            
        }
    }
    /// <summary>
    /// 物品拖拽处理
    /// </summary>
    /// <param name="target"></param>
    private void ItemDrag(GameObject target)
    {
        //非UI区域
        if (target == null)
        {
            transform.SetParent(CurParent);

        }
        //合法UI区域
        else
        {
            #region 目标区域为物品
            //检测到物品
            if (target.tag == "InVentoryItem")
            {
                //检测到为同类物品
                if (target.GetComponent<InventoryItemController>().Id == id)
                {
                    //组合物品
                    GroupMaterials(target.GetComponent<InventoryItemController>());
                }
                //非同类物品但是都在背包区域
                else if (target.GetComponent<Transform>().parent.tag == "InVentorySlot" && CurParent.tag == "InVentorySlot")
                {
                    ItemExchange(target);
                }
                //既不是同类物品也不是都在背包区域
                else
                {
                    transform.SetParent(CurParent);
                }
            }
            #endregion
            #region 目标区域为空背包格
            else if (target.tag == "InVentorySlot")
            {
                //保险起见还是看看有没有子物体，但是一般只要做UI的时候子物体的大小与背包格子大小一致就没有问题的
                if (target.GetComponent<Transform>().Find("InVentoryItem") == null)
                {
                    //调整sprite大小
                    ResetSpriteSize(transform, 85, 85);
                    transform.SetParent(target.transform);
                }
                else
                {
                    transform.SetParent(CurParent);
                }
            }
            #endregion
            #region 目标区域为合成物品槽
            //当目标区域的可放置标识符为真 且 当前物品id与目标区域id相同时 才可放置 否则放回原位
            else if (target.tag == "CraftingSlot" && target.GetComponent<CraftingSlotController>().IsOpen && id == target.GetComponent<CraftingSlotController>().Id)
            {
                ResetSpriteSize(transform, 70, 62);
                transform.SetParent(target.transform);
                CraftingPanelController.Instance.DargMaterial(gameObject);
            }
            #endregion
            else
            {
                transform.SetParent(CurParent);
            }
        }
        transform.localPosition = new Vector3(0, 0, 0); //使用localposition重置当前子物体坐标为父物体中心
                     
        CurParent = transform.parent;                   //当前物品所在的槽

        //重置拖拽状态
        isDrag = false;
        canvasGroup.blocksRaycasts = true;
        isCopy = false;                                 //拖拽已结束,下次拖拽可以再次复制
    }
    /// <summary>
    /// 当前物品和目标物品交换处理
    /// </summary>
    private void ItemExchange(GameObject target)
    {
        Transform targetTransform = target.GetComponent<Transform>();
        transform.SetParent(targetTransform.parent);
        targetTransform.SetParent(CurParent);
        target.GetComponent<InventoryItemController>().CurParent = CurParent;//这里有点难看，目标物体的最终背包格也要重置，不然会出问题
        targetTransform.localPosition = new Vector3(0, 0, 0); //使用localposition重置目标子物体坐标为父物体中心
    }
    /// <summary>
    /// 初始化复制的物品
    /// </summary>
    /// <param name="go"></param>
    private void InitNewItem(GameObject go,int num,string tag)
    {
        InventoryItemController tempCtrl = go.GetComponent<InventoryItemController>();
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;      //位置初始化
        tempCtrl.InitId(id);                    //id初始化
        tempCtrl.InitName();                //名称初始化，去掉clone
        tempCtrl.InitNum(num);                  //数量文本初始化
        tempCtrl.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (tag == "InVentorySlot")
        {
            tempCtrl.ResetSpriteSize(go.GetComponent<RectTransform>(), 85, 85);
        }
        else
        {
            tempCtrl.ResetSpriteSize(go.GetComponent<RectTransform>(), 70, 62);
        }
    }
    /// <summary>
    /// 安放多出来的物品
    /// </summary>
    private void CreateAndSettleNewItem(int num)
    {
        //寻找空物品栏
        bool find = false;
        List<GameObject> slotsList = InventoryPanelController.Instance.GetInventorySlotList();
        for (int i = 0; i < slotsList.Count; i++)
        {
            Transform temp = slotsList[i].GetComponent<Transform>().Find("InVentoryItem");
            if (temp == null)
            {
                GameObject go = GameObject.Instantiate(gameObject, slotsList[i].GetComponent<Transform>());
                //初始化新物品
                InitNewItem(go, num, "InVentorySlot");
                find = true;
                break;
            }
        }
        if (!find)
        {
            Debug.Log("背包已满，溢出物品已销毁");
        }

    }
}
