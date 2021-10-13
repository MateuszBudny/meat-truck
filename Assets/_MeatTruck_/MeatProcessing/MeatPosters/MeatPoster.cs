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

    public void SpawnMeat(GameObject corpseInstance)
    {
        NpcCharacter corpseNpc = corpseInstance.GetComponent<NpcCharacter>();
        Quaternion meatRotation = Quaternion.identity;
        meatRotation.eulerAngles = new Vector3(0f, corpseNpc.mainRigidbody.transform.rotation.eulerAngles.y, 0f);
        for (int i = 0; i < meatsNum; i++)
        {
            Instantiate(meatPrefabBehaviour, corpseNpc.mainRigidbody.transform.position, meatRotation);
        }
    }
}
