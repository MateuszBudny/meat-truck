using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private int cash = 0;
    [SerializeField]
    private List<Meat> meats = new List<Meat>();
    [SerializeField]
    private List<NpcCharacter> corpses = new List<NpcCharacter>();

    public int Cash { get => cash; set => cash = value; }
    public List<Meat> Meats { get => meats; private set => meats = value; }
    public List<NpcCharacter> Corpses { get => corpses; private set => corpses = value; }

    public bool HasMeatType(MeatData meatData) => Meats.Any(meat => meat.Data == meatData);

    public Inventory Copy()
    {
        Inventory newInventory = new Inventory
        {
            Cash = Cash,
            Meats = new List<Meat>(Meats),
            Corpses = new List<NpcCharacter>(Corpses)
        };

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
