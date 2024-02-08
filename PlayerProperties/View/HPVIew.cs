using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPVIew : MonoBehaviour
{

    public Image show_hp;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeHP(float percent) {
        show_hp.fillAmount = percent;
    }
}
