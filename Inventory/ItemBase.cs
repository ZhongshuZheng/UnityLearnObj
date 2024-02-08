using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ITEMTYPE {
    EMPTY = 0,
    Strawberry = 1

}


public class ItemBase
{
    public ITEMTYPE itemtype;
    public string itemname;
    public string description;



    public string isprite;

    public Action<object[]> fun = new Action<object[]>((x) => {});

}
