using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpenButton : MonoBehaviour
{
    public GameObject inventory_pannel;
    public Canvas m_canvas;

    public void openInventory() {
        InventoryControllor.Instance.openInventory(inventory_pannel, 
            m_canvas,
            inventory_pannel.GetComponent<InventoryPannelView>().contentGrid
        );
    }

}
