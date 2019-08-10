using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 合成模块左侧单个tab控制脚本
/// </summary>
public class CraftingTabItemController : MonoBehaviour {

    private Transform transform;
    private Button button;
    private GameObject buttonBG;
    private Image icon;
    private int index = -1;

	void Awake() {
        transform = gameObject.GetComponent<Transform>();
        button = gameObject.GetComponent<Button>();
        buttonBG = transform.Find("CenterBG").gameObject;
        icon = transform.Find("Icon").GetComponent<Image>();
        button.onClick.AddListener(ButtonOnClick);
	}
	

	void Update () {
		
	}

    /// <summary>
    /// 初始化Item
    /// </summary>
    /// <param name="index"></param>
    public void InitItem(int index,Sprite sprite)
    {
        this.index = index;
        gameObject.name = "Tab" + index;
        icon.sprite = sprite;
    }

    /// <summary>
    /// 未选中状态、默认状态
    /// </summary>
    public void NormalTab()
    {
        if (!buttonBG.activeSelf)
        {
            buttonBG.SetActive(true);
        }
    }

    /// <summary>
    /// 选中状态
    /// </summary>
    public void ActiveTab()
    {
        if (buttonBG.activeSelf)
        {
            buttonBG.SetActive(false);
        }
    }

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    private void ButtonOnClick()
    {
        SendMessageUpwards("ResetTabsAndContent", index);
    }
}
