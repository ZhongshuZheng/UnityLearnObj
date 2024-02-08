using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryItemOnGroundView : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            int res = InventoryControllor.Instance.addItem(new ItemStrawberry());
            if (res == 0) {
                Destroy(gameObject);
            }
        }
    }
}
