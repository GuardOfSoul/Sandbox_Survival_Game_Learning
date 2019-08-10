using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour {

    private Transform transform;
    private Image image;
    private Button craftButton;
    private Button craftAllButton;
    private GameObject craftingItemPrefab;
    private Transform goodItem;
    private string name;
    private int id;
    private int bar;

    public GameObject CraftingItemPrefab { get { return craftingItemPrefab; } set { craftingItemPrefab = value; } }

    void Awake () {
        transform = gameObject.GetComponent<Transform>();
        goodItem = transform.Find("GoodItem").GetComponent<Transform>();
        image = transform.Find("GoodItem/ItemImage").GetComponent<Image>();
        craftButton = transform.Find("Craft").GetComponent<Button>();
        craftAllButton = transform.Find("CraftAll").GetComponent<Button>();
        image.gameObject.SetActive(false);
        InitButton();
        craftButton.onClick.AddListener(CraftingItem);
    }

	void Update () {
		
	}

    /// <summary>
    /// 初始化合成物品图片
    /// </summary>
    /// <param name="sprite"></param>
    public void Init(Sprite sprite,string name,int id,int bar)
    {
        image.gameObject.SetActive(true);
        image.sprite= sprite;
        this.name = name;
        this.id = id;
        this.bar = bar;
    }
    /// <summary>
    /// 初始化按钮状态，默认为不可按
    /// </summary>
    public void InitButton()
    {
        craftButton.interactable = false;
        craftAllButton.interactable = false;
        craftButton.transform.Find("Text").GetComponent<Text>().color = Color.grey;
        craftAllButton.transform.Find("Text").GetComponent<Text>().color = Color.grey;
    }
    /// <summary>
    /// 激活按钮
    /// </summary>
    public void ActiveButton()
    {
        craftButton.interactable = true;
        craftAllButton.interactable = true;
        craftButton.transform.Find("Text").GetComponent<Text>().color = Color.white;
        craftAllButton.transform.Find("Text").GetComponent<Text>().color = Color.white;
    }
    /// <summary>
    /// 合成物品
    /// </summary>
    private void CraftingItem()
    {
        Debug.Log("合成");
        //在合成区域创建物品
        if (goodItem.Find("InVentoryItem") ==null)
        {
            GameObject go = GameObject.Instantiate(craftingItemPrefab, goodItem);
            CraftingPanelController.Instance.InitItem(go, name, 1, id,bar);
            CraftingPanelController.Instance.UseMaterial();
            //合成板有物品不可继续合成，暂时，后面有全部合成再调
            InitButton();
        }
    }
}
