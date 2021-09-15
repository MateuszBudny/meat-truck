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
    public GameObject meatPrefab;
    public float nonThrowingDuration = 1;
    public Vector3 throwForceMin = new Vector3(-100f, 300f, -200f);
    public Vector3 throwForceMax = new Vector3(-1000f, 1000f, 200f);

    private MeatProcessingStep meatProcessingStep = MeatProcessingStep.CorpseThrowing;
    private GameObject corpseInstance;
    private GameObject meatInstance;

    public void OnCorpseJumpClick(CallbackContext context)
    {
        if (meatProcessingStep == MeatProcessingStep.CorpseThrowing)
        {

            if (context.started)
            {
                corpseInstance = Instantiate(corpse, corpseSpawnPoint.transform);
                Vector3 throwForce = new Vector3(
                    Random.Range(throwForceMin.x, throwForceMax.x),
                    Random.Range(throwForceMin.y, throwForceMax.y),
                    Random.Range(throwForceMin.z, throwForceMax.z));

                Debug.Log(throwForce);

                NpcCharacter corpseInstanceCharacter = corpseInstance.GetComponent<NpcCharacter>();
                corpseInstanceCharacter.SetAsRagdoll();
                corpseInstanceCharacter.mainRigidbody.AddForce(throwForce);
                meatProcessingStep++;
                StartCoroutine(NonThrowingTime());
            }
        }
        if (meatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                Vector3 corpsePosition = corpseInstance.GetComponent<NpcCharacter>().mainRigidbody.transform.position;
                Destroy(corpseInstance);
                meatInstance = Instantiate(meatPrefab, corpsePosition, meatPrefab.transform.rotation);
                meatProcessingStep = MeatProcessingStep.CorpseThrowing;
            }

        }
    }

    private IEnumerator NonThrowingTime()
    {
        yield return new WaitForSecondsRealtime(nonThrowingDuration);
        meatProcessingStep++;
    }
}

public enum MeatProcessingStep
{
    CorpseThrowing = 0,
    CorpseFlying = 1,
    MeatAcquire = 2,
}