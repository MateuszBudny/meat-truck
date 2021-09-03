using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

// most important shortkeys
// tier0:
// alt+enter: tip
// ctrl+t: searching by fields/classes names
// right click + znajdz wszystkie odwolania: znajduje wszystkie odwolania xd
// ctrl+left click on field: jump into field declaration

// tier1:
// ctrl+l: copy and delete line 
// alt+arrow up/down: selected line up/down

public class MeatProcessingManager : MonoBehaviour
{
    public GameObject corpse;
    public GameObject corpseSpawnPoint;
    public Vector3 throwForceMin = new Vector3(-100f, 300f, -200f);
    public Vector3 throwForceMax = new Vector3(-1000f, 1000f, 200f);

    public void OnCorpseJumpClick(CallbackContext context)
    {
        if (context.started)
        {
            GameObject corpseInstance = Instantiate(corpse, corpseSpawnPoint.transform);
            Vector3 throwForce = new Vector3(
                Random.Range(throwForceMin.x, throwForceMax.x),
                Random.Range(throwForceMin.y, throwForceMax.y),
                Random.Range(throwForceMin.z, throwForceMax.z));

            NpcCharacter corpseInstanceCharacter = corpseInstance.GetComponent<NpcCharacter>();
            corpseInstanceCharacter.SetAsRagdoll();
            corpseInstanceCharacter.mainRigidbody.AddForce(throwForce);
        }
    }
}

