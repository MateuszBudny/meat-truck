using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField]
    private Inventory defaultInventory;
    [SerializeField]
    private bool useDefaultInventory = false;

    [Header("Runtime usage ONLY")]
    [JsonProperty]
    [SerializeField]
    private Inventory inventory;
    [JsonIgnore]
    public Inventory Inventory { get => inventory; private set => inventory = value; }

    public void Init()
    {
        Inventory = useDefaultInventory ? defaultInventory.Copy() : new Inventory();
    }
}
