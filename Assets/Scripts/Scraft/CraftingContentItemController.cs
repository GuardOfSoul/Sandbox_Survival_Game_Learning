using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制单个item
/// </summary>
public class CraftingContentItemController : MonoBehaviour
{

    private Transform transform;
    private Text text;
    private GameObject bg;
    private Button button;
    private int id;
    private string name;

    public int Id { get { return id; } }

    void Awake()
    {
        transform = gameObject.GetComponent<Transform>();
        button = gameObject.GetComponent<Button>();
        text = transform.Find("Text").GetComponent<Text>();
        bg = transform.Find("BG").gameObject;
        bg.SetActive(false);
        button.onClick.AddListener(ItemButtonClick);    //监听点击事件
    }

    public void InitItem(CraftingContentItem item)
    {
        this.id = item.ItemId;
        this.name = item.ItemName;
        gameObject.name = name;
        text.text = name;
    }

    /// <summary>
    /// 默认状态
    /// </summary>
    public void SetNormal()
    {
        bg.SetActive(false);
    }

    /// <summary>
    /// 激活状态
    /// </summary>
    public void SetActive()
    {
        bg.SetActive(true);
        //Debug.Log(this.id);
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    private void ItemButtonClick()
    {
        transform.SendMessageUpwards("ResetItemState", this);
    }
}