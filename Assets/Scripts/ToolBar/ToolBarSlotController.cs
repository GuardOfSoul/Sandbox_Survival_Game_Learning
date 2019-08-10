using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarSlotController : MonoBehaviour {

    private Transform transform;
    private Text key;
    private Button button;
    private Image image;
    private bool buttonState = false; //按钮状态，默认为未选中

    void Awake () {
        transform = gameObject.GetComponent<Transform>();
        button = gameObject.GetComponent<Button>();
        image = gameObject.GetComponent<Image>();
        key = transform.Find("Key").GetComponent<Text>();
        //鼠标点击事件暂时用不上了
        //button.onClick.AddListener(ButtonClick);
	}
	
	void Update () {
		
	}

    /// <summary>
    /// 初始化工具栏格子
    /// </summary>
    /// <param name="name"></param>
    /// <param name="num"></param>
    public void InitSlot(string name,int num)
    {
        gameObject.name = name;
        key.text = num.ToString();
    }
    
    private void ButtonClick()
    {
        if (!buttonState)
        {SwitchButtonState();
        }
        SendMessageUpwards("SlotChange", gameObject);
    }

    public void SwitchButtonState()
    {
        if (!buttonState) { image.color = Color.red; buttonState = true; }
        else { image.color = Color.white; buttonState = false; }
    }
}
