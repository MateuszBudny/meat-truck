using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsSpawning<TBehaviour> : MonoBehaviour
    where TBehaviour : MonoBehaviour
{
    public List<TBehaviour> SpawnGivenItems(List<TBehaviour> itemsToSpawn)
    {
        List<TBehaviour> spawnedItems = new List<TBehaviour>();
        itemsToSpawn.ForEach(item =>
        {
            TBehaviour newBehaviour = InstantiateItem(item);
            spawnedItems.Add(newBehaviour);
        });

        return spawnedItems;
    }

    protected abstract TBehaviour InstantiateItem(TBehaviour itemBehaviourToInstantiate);
}
