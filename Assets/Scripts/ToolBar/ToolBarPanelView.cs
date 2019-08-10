using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelView : MonoBehaviour {

    private Transform transform;
    private Transform gridTransform;
    private GameObject prefabToolBarSlot;

    public GameObject PrefabToolBarSlot { get { return prefabToolBarSlot; } }
    public Transform GridTransform{get{return gridTransform;}}
    public Transform Transform { get { return transform; } set { transform = value; } }

    void Awake (){
        InitAll();
	}
	
    private void InitAll()
    {
        transform = gameObject.GetComponent<Transform>();
        gridTransform = transform.Find("Grid");
        prefabToolBarSlot = Resources.Load<GameObject>("ToolBarPanel/ToolBarSlot");
    }
}
