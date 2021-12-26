using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeatsSpawning : ItemsSpawning<MeatBehaviour>
{
    [SerializeField]
    private List<MeatSpawnPoint> meatSpawnPoints;

    private Dictionary<MeatData, MeatSpawnPoint> meatSpawnPointsDict = new Dictionary<MeatData, MeatSpawnPoint>();

    private void Awake()
    {
        meatSpawnPoints.ForEach(meatSpawnPoint =>
        {
            meatSpawnPointsDict.Add(meatSpawnPoint.meatDataToSpawn, meatSpawnPoint);
        });
    }

    public List<MeatBehaviour> SpawnGivenMeats(List<Meat> meatsToSpawn) => SpawnGivenItems(meatsToSpawn.Select(meat => meat.Data.meatPrefabBehaviour).ToList());

    protected override MeatBehaviour InstantiateItem(MeatBehaviour itemBehaviourToInstantiate)
    {
        MeatBehaviour newMeat = Instantiate(itemBehaviourToInstantiate, meatSpawnPointsDict[itemBehaviourToInstantiate.meat.Data].transform.position, Quaternion.identity, transform);
        newMeat.meat = itemBehaviourToInstantiate.meat;

        return newMeat;
    }
}
