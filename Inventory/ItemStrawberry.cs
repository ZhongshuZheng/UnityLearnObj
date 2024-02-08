using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStrawberry : ItemBase
{
    public ItemStrawberry() {
        itemtype = ITEMTYPE.Strawberry;
        itemname = "StrawBerry";
        description = "Recover HP";
        isprite = "CollectibleHealth";
        fun = useItem;
    }

    private void useItem(params object[] obj)
    {
        if (obj == null || obj.Length == 0 || obj[0] == null) {
            HPController.Instance.changeHP(40);
            Debug.Log("use strawberry, HP now:" + HPController.Instance.getHP());
        }
    }

}
