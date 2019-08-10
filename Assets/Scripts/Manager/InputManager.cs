using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class InputManager : MonoBehaviour {

    public static InputManager Instance;
    private bool inventoryState = false;
    private bool buildState = false;
    private bool craftingState = false;
    private FirstPersonController firstPersonController;
    private GameObject buildPanel;
    private GunControllerBase gunControllerBase;
    private GameObject gunStar;

    public bool BuildState
    {
        get { return buildState; }
        set
        {
            if (buildPanel != null)
            {
                buildPanel.SetActive(value);
                if (!value)
                {
                    buildPanel.GetComponent<BuildPanelController>().ResetUI();
                }
            } buildState = value; } }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        FindInit();
    }
    void Update ()
    {
        if (!buildState)
        {
            InventoryKey();
            CraftingKey();
        }
        
        if (!inventoryState&& !craftingState)
        {
            ToolBarKeyControl();
        }
    }

    private void FindInit()
    {
        firstPersonController = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        buildPanel = GameObject.Find("Canvas/BuildPanel");
        buildPanel.SetActive(false);
    }
    private void SetPersonActive()
    {
        if (ToolBarPanelController.Instance.CurWeapen != null) ToolBarPanelController.Instance.CurWeapen.SetActive(true);
        firstPersonController.enabled = true;
    }
    private void SetPersonNormal()
    {
        if (ToolBarPanelController.Instance.CurWeapen != null) ToolBarPanelController.Instance.CurWeapen.SetActive(false);
        firstPersonController.enabled=false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// 合成面板按键控制
    /// </summary>
    private void InventoryKey()
    {
        if (Input.GetKeyDown(GameConst.InventoryKey))
        {
            if (inventoryState)
            {
                inventoryState = false;
                craftingState = false;
                InventoryPanelController.Instance.UIPanelHide();
                CraftingPanelController.Instance.UIPanelHide();
                SetPersonActive();
            }
            else
            {
                inventoryState = true;
                InventoryPanelController.Instance.UIPanelShow();
                SetPersonNormal();
            }
        }
    }
    private void CraftingKey()
    {
        if (Input.GetKeyDown(GameConst.CraftKey))
        {
            if (craftingState)
            {
                craftingState = false;
                CraftingPanelController.Instance.UIPanelHide();
                if (!inventoryState)
                {
                    SetPersonActive();
                }
            }
            else
            {
                craftingState = true;
                inventoryState = true;
                CraftingPanelController.Instance.UIPanelShow();
                InventoryPanelController.Instance.UIPanelShow();
                SetPersonNormal();
            }
        }
    }
    private void ToolBarKeyControl()
    {
        ToolBarKey(GameConst.ToolBar1,0);
        ToolBarKey(GameConst.ToolBar2,1);
        ToolBarKey(GameConst.ToolBar3,2);
        ToolBarKey(GameConst.ToolBar4,3);
        ToolBarKey(GameConst.ToolBar5,4);
        ToolBarKey(GameConst.ToolBar6,5);
        ToolBarKey(GameConst.ToolBar7,6);
        ToolBarKey(GameConst.ToolBar8,7);
    }
    private void ToolBarKey(KeyCode keyCode,int keynum)
    {
        if (Input.GetKeyDown(keyCode))
        {
            ToolBarPanelController.Instance.SlotChangeByKey(keynum);
        }
    }
}
