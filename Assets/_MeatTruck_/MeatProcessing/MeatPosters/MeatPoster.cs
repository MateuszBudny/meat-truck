using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPoster : MonoBehaviour
{
    public GameObject highlight;
    public MeatBehaviour meatPrefabBehaviour;
    [Range(1, 20)]
    public int meatsNum = 1;

    public void SetPosterAsSelected(bool activate)
    {
        highlight.SetActive(activate);
    }

    public void SpawnMeat(NpcCharacterBehaviour corpseInstance)
    {
        Quaternion meatRotation = Quaternion.identity;
        meatRotation.eulerAngles = new Vector3(0f, corpseInstance.mainRigidbody.transform.rotation.eulerAngles.y, 0f);
        for (int i = 0; i < meatsNum; i++)
        {
            MeatBehaviour newMeatBehaviour = Instantiate(meatPrefabBehaviour, corpseInstance.mainRigidbody.transform.position, meatRotation);
            GameManager.Instance.Player.Inventory.Meats.Add(newMeatBehaviour.meat);
        }
    }
}
