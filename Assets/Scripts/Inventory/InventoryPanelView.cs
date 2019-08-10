using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包view层
/// </summary>
public class InventoryPanelView : MonoBehaviour {

    private Transform transform;
    private Transform gridTransform;

    private GameObject prefabSlot;
    private GameObject prefabItem;

    public Transform Transform{get{return transform;}}
    public Transform GridTransform{get{return gridTransform;}}
    public GameObject PrefabSlot{get{return prefabSlot;}}
    public GameObject PrefabItem {get{return prefabItem;}}

    void Awake () {
        transform = gameObject.GetComponent<Transform>();
        gridTransform = transform.Find("Background/Grid").GetComponent<Transform>();
        prefabSlot = Resources.Load<GameObject>("Item/InVentorySlot");
        prefabItem = Resources.Load<GameObject>("Item/InVentoryItem");
	}
}
