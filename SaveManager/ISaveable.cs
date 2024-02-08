using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    string Save();

    void Load(string json_string);
    
}