using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashSpawning : ItemsSpawning<CashBehaviour>
{
    [SerializeField]
    private Transform cashSpawningPoint;
    [SerializeField]
    private CashTableData cashTableData;

    public List<CashBehaviour> SpawnGivenCashAmount(int cashAmount)
    {
        List<CashBehaviour> cashToSpawn = new List<CashBehaviour>();
        for(int i = 0; i < cashAmount; i++)
        {
            cashToSpawn.Add(cashTableData.CashValues[1]);
        }

        return SpawnGivenItems(cashToSpawn);
    }

    protected override CashBehaviour InstantiateItem(CashBehaviour itemBehaviourToInstantiate) => Instantiate(itemBehaviourToInstantiate, cashSpawningPoint.position, Quaternion.identity, transform);
}
