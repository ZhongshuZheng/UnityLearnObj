using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class HPController : Singleton<HPController>, ISaveable
{

    public GameObject hp_view_ob;
    HPModel hp_model;
    HPVIew hp_view;

    void Start()
    {
        hp_model = new HPModel(100, 50);
        hp_view = hp_view_ob.GetComponent<HPVIew>();
        hp_view.changeHP(1.0f * hp_model.health / hp_model.max_health);
    }

    void Update()
    {
    }

    public void changeHP(int delta_value) {
        hp_model.health += delta_value;
        hp_view.changeHP(1.0f * hp_model.health / hp_model.max_health);
    }

    public int getHP() {
        return hp_model.health;
    }

    public string Save()
    {
        string s = JsonConvert.SerializeObject(hp_model);
        return s;
    }

    public void Load(string json_string)
    {
        hp_model = JsonConvert.DeserializeObject<HPModel>(json_string);
    }
}
