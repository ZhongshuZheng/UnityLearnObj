using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour where T : class
{
    // private static readonly T instance = Activator.CreateInstance<T>();
    private static T instance; 

    public static T Instance{get{return instance;}}

    public virtual void Awake() {
        instance = this as T;
    }

}
