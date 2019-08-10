using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包物品数据实体类
/// </summary>
public class InventoryItem{

    private string itemName;
    private int itemNum;
    private int itemId;
    private int itemBar;

    public int ItemNum { get { return itemNum; } set { itemNum = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public int ItemId { get { return itemId; } set { itemId = value; } }
    public int ItemBar { get { return itemBar; } set { itemBar = value; } }

    public InventoryItem() { }

    public InventoryItem(string itemName, int itemNum,int itemId,int itemBar)
    {
        this.itemName = itemName;
        this.itemNum = itemNum;
        this.itemId = itemId;
        this.itemBar = itemBar;
    }

    public override string ToString()
    {
        return string.Format("物品名称：{0},数量：{1},id：{2},itemBar:{3}",this.itemName,this.itemNum,this.itemId,this.itemBar);
    }
}
