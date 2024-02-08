using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmpty : ItemBase
{
    public ItemEmpty() {
        itemtype = ITEMTYPE.EMPTY;
        itemname = "";
        description = "";
        isprite = "lil_roundbackground";
        fun = (x) => {};
    }
}
