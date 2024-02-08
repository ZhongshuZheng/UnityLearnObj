using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSaveData
{
    public Dictionary<string, string> save_data_dict;

    public List<ISaveable> save_obj_list;

    public GameSaveData() {
        save_data_dict = new Dictionary<string, string>();
        save_obj_list = new List<ISaveable>();
    }

}
