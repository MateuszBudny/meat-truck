using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Meat> Meats { get; private set; } = new List<Meat>();
    public List<NpcCharacter> Corpses { get; private set; } = new List<NpcCharacter>();

    public void DebugLogMeats()
    {
        Debug.Log("Inventory:");
        for (int i = 0; i < Meats.Count; i++)
        {
            Debug.Log(Meats[i].Data);
        }
    }
}
