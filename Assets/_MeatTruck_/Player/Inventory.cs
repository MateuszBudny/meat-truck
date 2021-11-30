using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private List<Meat> meats = new List<Meat>();
    [SerializeField]
    private List<NpcCharacter> corpses = new List<NpcCharacter>();

    public List<Meat> Meats { get => meats; private set => meats = value; }
    public List<NpcCharacter> Corpses { get => corpses; private set => corpses = value; }

    public Inventory Copy()
    {
        Inventory newInventory = new Inventory();
        newInventory.Meats = new List<Meat>(Meats);
        newInventory.Corpses = new List<NpcCharacter>(Corpses);

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
