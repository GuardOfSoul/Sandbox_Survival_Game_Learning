using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarPanelController : MonoBehaviour {

    public static ToolBarPanelController Instance;
    private ToolBarPanelModel toolBarPanelModel;
    private ToolBarPanelView toolBarPanelView;
    [SerializeField]private GameObject curSlot;//当前激活的工具栏格子
    [SerializeField]private GameObject curWeapen;//当前激活的武器模型
    private List<GameObject> toolBarSlotsList = new List<GameObject>();
    private Dictionary<GameObject, GameObject> toolBarSlotItemDic = new Dictionary<GameObject, GameObject>();

    public GameObject CurWeapen { get { return curWeapen; } set { curWeapen = value; } }

    public GameObject CurSlot { get { return curSlot; } set { curSlot = value; } }

    void Awake()
    {
        Instance = this;
    }
    void Start () {
        toolBarPanelView = gameObject.GetComponent<ToolBarPanelView>();
        toolBarPanelModel = gameObject.GetComponent<ToolBarPanelModel>();
        InitToolBar();

    }
	
	void Update () {
		
	}

    private void InitToolBar()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject go= GameObject.Instantiate(toolBarPanelView.PrefabToolBarSlot, toolBarPanelView.GridTransform);
            go.name = toolBarPanelView.PrefabToolBarSlot.name + i;
            go.GetComponent<ToolBarSlotController>().InitSlot(toolBarPanelView.PrefabToolBarSlot.name + i, i + 1);
            toolBarSlotsList.Add(go);
        }
    }

    private void SlotChange(GameObject go)
    {
        if (curSlot != null && curSlot.name != go.name) 
        {
            curSlot.GetComponent<ToolBarSlotController>().SwitchButtonState();
        }
        curSlot = go;
    }

    public void SlotChangeByKey(int keynum)
    {
        //已经有工具被激活
        if (curSlot != null)
        {
            
            if (curSlot.name != toolBarSlotsList[keynum].name)
            {
                curSlot.GetComponent<ToolBarSlotController>().SwitchButtonState();
                toolBarSlotsList[keynum].GetComponent<ToolBarSlotController>().SwitchButtonState();
                curSlot = toolBarSlotsList[keynum];
            }
            //已激活的与按键按的是同一个，将当前工具消活
            else
            {
                curSlot.GetComponent<ToolBarSlotController>().SwitchButtonState();
                curSlot = null;
            }
        }
        //当前没有工具已激活
        else if (curSlot==null)
        {
            toolBarSlotsList[keynum].GetComponent<ToolBarSlotController>().SwitchButtonState();
            curSlot = toolBarSlotsList[keynum];
        }
        StartGunFactory(toolBarSlotsList[keynum]);
    }

    private void StartGunFactory(GameObject nextSlot)
    {
        StartCoroutine("CallGunFactory", nextSlot);
    }

    private IEnumerator CallGunFactory(GameObject nextSlot)
    {
        GameObject go = null;
        Transform temp = null;
        temp = nextSlot.GetComponent<Transform>().Find("InVentoryItem");//查找目标工具栏是否有武器
        //toolBarSlotItemDic.TryGetValue(nextSlot, out go);//查找目标工具栏武器是否已创建
        //当前已有工具被激活
        if (curWeapen != null)
        {
            //无论如何都要先放下当前武器
            if (curWeapen.GetComponent<GunControllerBase>()!=null)
            {
                curWeapen.GetComponent<GunControllerBase>().Holster();
            }
            else
            {
                curWeapen.GetComponent<Animator>().SetTrigger("Holster");
            }
            yield return new WaitForSeconds(0.5f);//等完这0.5秒curWeapen是有可能为空的
            if (curWeapen != null)
            {
                curWeapen.SetActive(false);
            }
        }
        //当前有工具栏被激活且工具栏有武器
        if (temp != null && curSlot != null)
        {
            //目标工具栏的武器未被创建
            toolBarSlotItemDic.TryGetValue(nextSlot, out go);
            if (go == null)
            {
                go = GunFactory.Instance.CreateGun(temp.GetComponent<Image>().sprite.name, temp.gameObject);
                toolBarSlotItemDic.Remove(nextSlot);
                toolBarSlotItemDic.Add(nextSlot, go);
            }
            //目标工具栏的武器已被创建
            //被创建的与现在在工具栏中的武器名字相同，显示武器
            else if (temp.GetComponent<Image>().sprite.name == go.name)
            {
                go.SetActive(true);
            }
            //名称不相同，显示当前栏中的武器，之前的删除而且列表要更新
            else
            {
                toolBarSlotItemDic.Remove(nextSlot);
                GameObject.Destroy(go);
                go = GunFactory.Instance.CreateGun(temp.GetComponent<Image>().sprite.name, temp.gameObject);
                toolBarSlotItemDic.Add(nextSlot, go);
            }
            curWeapen = go;
        }
        //当前没有选中任何格子，所以现在已经没有持有武器了
        else if (curSlot == null)
        {
            curWeapen = null;
        }
    }
}
