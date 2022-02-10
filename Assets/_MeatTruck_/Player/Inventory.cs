using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
[Serializable]
public class Inventory
{
    [JsonProperty]
    [SerializeField]
    private int cash;
    [JsonProperty]
    [SerializeField]
    private List<Meat> meats = new List<Meat>();
    [JsonProperty]
    [SerializeField]
    private List<NpcCharacter> corpses = new List<NpcCharacter>();

    public int Cash { get => cash; set => cash = value; }
    public List<Meat> Meats { get => meats; private set => meats = value; }
    public List<NpcCharacter> Corpses { get => corpses; private set => corpses = value; }

    public Inventory() {}

    public Inventory(int cash, List<Meat> meats, List<NpcCharacter> corpses)
    {
        Cash = cash;
        Meats = meats;
        Corpses = corpses;
    }

    public bool HasMeatType(MeatData meatData) => Meats.Any(meat => meat.Data == meatData);

    public Inventory Copy()
    {
        Inventory newInventory = new Inventory(Cash, Meats, Corpses);
        return newInventory;
    }

    public void DebugLogMeats()
    {
        Debug.Log("Inventory:");
        for (int i = 0; i < Meats.Count; i++)
        {
            Debug.Log(Meats[i].Data);
        }
    }
}