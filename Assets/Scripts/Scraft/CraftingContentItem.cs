using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentItem {
    private int itemId;
    private string itemName;

    public int ItemId
    {
        get
        {
            return itemId;
        }

        set
        {
            itemId = value;
        }
    }
    public string ItemName
    {
        get
        {
            return itemName;
        }

        set
        {
            itemName = value;
        }
    }
}
