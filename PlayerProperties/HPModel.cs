using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPModel
{
    public int max_health;
    public int health;

    public HPModel(int max_health=100, int health=100) {
        this.max_health = max_health;
        this.health = health;
    }

}
