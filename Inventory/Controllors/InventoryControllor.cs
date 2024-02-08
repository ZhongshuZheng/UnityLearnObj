using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
// using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class InventoryControllor : Singleton<InventoryControllor>, ISaveable
{

    InventoryModel imodel; 
    public List<InventoryItemView> item_views;


    void Start()
    {
        imodel = InventoryModel.Instance;   
    }

    public void refreshInventoryView(GameObject inventoryPannel, Canvas m_canvas, GameObject inventoryGrid) {
        for (int i = 0; i < imodel.items.Count; i++) {
            if (item_views.Count <= i) {
                GameObject item_i = Instantiate(imodel.ItemPrefab, inventoryGrid.transform);
                InventoryItemView item_view_i = item_i.GetComponent<InventoryItemView>();
                item_view_i.index = i;
                item_view_i.item_type = imodel.items[i].itemtype;
                item_view_i.m_canvas = m_canvas;

                if (imodel.items[i].itemtype != ITEMTYPE.EMPTY) {
                    item_view_i.setItemView(imodel.items[i]);
                }
                item_views.Add(item_view_i);

            } else if (item_views[i].item_type != imodel.items[i].itemtype) {
                item_views[i].setItemView(imodel.items[i]); 
            }
        }
    }

    public int addItem(ItemBase item) {
        if (imodel.size + 1 > imodel.maxSize) {
            return -1;
        }
        for (int i = 0; i < imodel.maxSize && i < imodel.items.Count; i++) {
            print(i);
            if (imodel.items[i].itemtype == ITEMTYPE.EMPTY) {
                // Update model
                imodel.items[i] = item;
                imodel.size += 1;
                return 0;
            } 
        }
        return -1;
    }

    public void swapItems(Canvas m_canvas, int index) {
        GameObject target = Utils.GetMousePositionUI(m_canvas);
        if (target == null) {
            return;
        }

        InventoryItemView target_view = target.GetComponent<InventoryItemView>();
        if (target_view == null) {
            return;
        }

        int target_index = target_view.index;

        // Swap the model
        ItemBase tmp_item = imodel.items[index];
        imodel.items[index] = imodel.items[target_index];
        imodel.items[target_index] = tmp_item;

        // refresh the view
        item_views[index].setItemView(imodel.items[index]);
        item_views[target_index].setItemView(imodel.items[target_index]);

        // for (int i = 0; i < imodel.items.Count; i++) {
        //     print(imodel.items[i].itemname);
        //     print(item_views[i].item_type);
        // }
    }

    public void useItem(Canvas m_canvas, int index) {
        if (imodel.items[index].itemtype != ITEMTYPE.EMPTY) {
            imodel.items[index].fun(null);
        }
        imodel.items[index] = new ItemEmpty();
        imodel.size -= 1;
        item_views[index].setItemView(imodel.items[index]);
    }

    public void openInventory(GameObject inventoryPannel, Canvas m_canvas, GameObject inventoryGrid) {
        refreshInventoryView(inventoryPannel, m_canvas, inventoryGrid);
        // show pannel
        inventoryPannel.SetActive(true);
    }

    internal void ClosePannel(GameObject inventoryPannel, GameObject inventoryGrid) {
        inventoryPannel.SetActive(false);
        // print("close inventory");
    }

    public string Save()
    {
        InventoryModelSaveData idata = new InventoryModelSaveData();
        idata.maxSize = imodel.maxSize;
        idata.size = imodel.size;
        idata.items = new List<ITEMTYPE>();
        foreach (ItemBase item in imodel.items) {
            idata.items.Add(item.itemtype);
        }
        return JsonConvert.SerializeObject(idata);
    }

    public void Load(string json_string)
    {
        InventoryModelSaveData idata = JsonConvert.DeserializeObject<InventoryModelSaveData>(json_string);
        imodel.maxSize = idata.maxSize;
        imodel.size = 0;    // idata.size;
        imodel.items.Clear();
        foreach (ITEMTYPE item in idata.items) {
            if (item == ITEMTYPE.EMPTY)
                imodel.items.Add(new ItemEmpty());
            else if (item == ITEMTYPE.Strawberry) {
                imodel.items.Add(new ItemStrawberry());
                imodel.size += 1;
            }
            else {
                imodel.items.Add(new ItemEmpty());
            }
        }
    }

}
