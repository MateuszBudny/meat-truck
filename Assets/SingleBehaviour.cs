using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBehaviour<T> : MonoBehaviour where T : SingleBehaviour<T>
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogError($"More than one SingleBehaviour of the same class is present on scene. Additional SingleBehaviour's GameObject name: {name}");
        }

        AdditionalOnAwake();
    }

    public virtual void AdditionalOnAwake() {}
}
