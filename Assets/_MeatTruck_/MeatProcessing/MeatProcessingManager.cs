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
    public float nonThrowingDuration = 1;
    public Vector3 throwForceMin = new Vector3(-100f, 300f, -200f);
    public Vector3 throwForceMax = new Vector3(-1000f, 1000f, 200f);
    public List<MeatPoster> meatPosters;

    private MeatProcessingStep meatProcessingStep = MeatProcessingStep.CorpseThrowing;
    private GameObject corpseInstance;
    private int currentPosterIndex = 0;

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
                meatPosters[currentPosterIndex].SpawnMeat(corpseInstance);
                Destroy(corpseInstance);
                meatProcessingStep = MeatProcessingStep.CorpseThrowing;
                meatPosters[currentPosterIndex].SetPosterAsSelected(false);
            }
        }
    }

    public void OnSelectLeftPosterClick(CallbackContext context)
    {
        if (meatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                meatPosters[currentPosterIndex].SetPosterAsSelected(false);
                if (currentPosterIndex == 0)
                {
                    currentPosterIndex = meatPosters.Count - 1;
                }
                else
                {
                    currentPosterIndex--;
                }
                meatPosters[currentPosterIndex].SetPosterAsSelected(true);
            }
        }
    }

    public void OnSelectRightPosterClick(CallbackContext context)
    {
        if (meatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                meatPosters[currentPosterIndex].SetPosterAsSelected(false);
                if (currentPosterIndex == meatPosters.Count - 1)
                {
                    currentPosterIndex = 0;
                }
                else
                {
                    currentPosterIndex++;
                }
                meatPosters[currentPosterIndex].SetPosterAsSelected(true);
            }
        }
    }

    private IEnumerator NonThrowingTime()
    {
        yield return new WaitForSecondsRealtime(nonThrowingDuration);
        meatProcessingStep++;
        meatPosters[currentPosterIndex].SetPosterAsSelected(true);
    }
}

public enum MeatProcessingStep
{
    CorpseThrowing = 0,
    CorpseFlying = 1,
    MeatAcquire = 2,
}