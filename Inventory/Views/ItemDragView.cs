using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragView : MonoBehaviour
{

    public Canvas m_canvas;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canvas != null) {
            Utils.UGUIFollow(m_canvas, gameObject);
        }
    }

}
