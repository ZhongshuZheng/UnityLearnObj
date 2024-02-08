using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryModel: Singleton<InventoryModel>
{

    public int maxSize;
    public int size;

    public GameObject ItemPrefab;

    public List<ItemBase> items;

    void Start() {
        size = -1;
        items = new List<ItemBase>();
        for (int i = -1; i < maxSize; i++) {
            items.Add(new ItemEmpty());
        }
    }

}

public class InventoryModelSaveData {
    public int maxSize;
    public int size;
    public List<ITEMTYPE> items;
}
