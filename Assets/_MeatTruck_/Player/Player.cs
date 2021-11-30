using System;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField]
    private Inventory defaultInventory;
    [SerializeField]
    private bool useDefaultInventory = false;
    
    public Inventory Inventory { get; private set; }

    public void Init()
    {
        Inventory = useDefaultInventory ? defaultInventory.Copy() : new Inventory();
    }
}
