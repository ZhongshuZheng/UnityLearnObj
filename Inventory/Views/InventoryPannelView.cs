using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPannelView : MonoBehaviour
{
    public GameObject pannel;
    public GameObject contentGrid;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosePannel() {
        InventoryControllor.Instance.ClosePannel(pannel, contentGrid);
    }
}
