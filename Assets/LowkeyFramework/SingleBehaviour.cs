using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            OnFirstInstance();   
        }
        else
        {
            OnAnotherInstance();
        }
    }

    protected virtual void OnFirstInstance()
    {
        Instance = this as T;
    }

    protected virtual void OnAnotherInstance()
    {
        Debug.LogError($"More than one SingleBehaviour of the same class is present on scene. Additional SingleBehaviour's GameObject name: {name}");
    }
}
