using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个图谱槽自身管理脚本
/// </summary>
public class CraftingSlotController : MonoBehaviour {

    private Transform transform;
    private CanvasGroup canvasGroup;
    private Image image;
    private bool isOpen = false; //默认图谱槽不可以接收物品
    private int id = -1; //图谱槽物品自身id

    public bool IsOpen { get { return isOpen; } }

    public int Id { get { return id; } }

    void Awake () {
        transform = gameObject.GetComponent<Transform>();
        image = transform.Find("Item").GetComponent<Image>();
        image.gameObject.SetActive(false);
        canvasGroup = transform.Find("Item").GetComponent<CanvasGroup>();
	}
	
	void Update () {
		
	}

    public void Init(Sprite sprite,int id)
    {
        image.gameObject.SetActive(true);
        image.sprite = sprite;
        //canvasGroup组件，让参考图片自身忽略射线检测
        canvasGroup.blocksRaycasts = false;
        this.id = id;
    }

    public void ResetSlot()
    {
        image.gameObject.SetActive(false);
    }

    public void SetSlotState(bool state)
    {
        this.isOpen = state;
    }
}
