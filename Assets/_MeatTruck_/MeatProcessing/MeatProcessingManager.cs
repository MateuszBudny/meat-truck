using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject corpseSpawnPoint;
    public float nonThrowingDuration = 1;
    public Vector3 throwForceMin = new Vector3(-100f, 300f, -200f);
    public Vector3 throwForceMax = new Vector3(-1000f, 1000f, 200f);
    public List<MeatPoster> meatPosters;
    [SerializeField]
    private MeatsSpawning meatsSpawning;
    [SerializeField]
    private CashSpawning cashSpawning;
    [SerializeField]
    private GameObject corpseThrowMobileButtonRenderer;

    private NpcCharacterBehaviour corpseInstance;
    private int currentPosterIndex = 0;

    private MeatProcessingStep meatProcessingStep = MeatProcessingStep.CorpseThrowing;
    private MeatProcessingStep MeatProcessingStep 
    { 
        get => meatProcessingStep;
        set
        {
            meatProcessingStep = value;
#if UNITY_ANDROID
            CheckCorpseThrowButton(value);
#endif
        }
    }

    private Inventory PlayerInventory => GameManager.Instance.Player.Inventory;

    private void Start()
    {
        meatsSpawning.SpawnGivenMeats(GameManager.Instance.Player.Inventory.Meats);
        cashSpawning.SpawnGivenCashAmount(GameManager.Instance.Player.Inventory.Cash);
    }

    public void OnCorpseJumpClick(CallbackContext context)
    {
        if (MeatProcessingStep == MeatProcessingStep.CorpseThrowing)
        {

            if (context.started)
            {
                if (PlayerInventory.Corpses.Count == 0)
                {
                    SceneManager.LoadScene("PLAYGROUND");
                    return;
                }
                corpseInstance = Instantiate(PlayerInventory.Corpses[PlayerInventory.Corpses.Count - 1].data.behaviour, corpseSpawnPoint.transform);
                PlayerInventory.Corpses.RemoveAt(PlayerInventory.Corpses.Count - 1);
                Vector3 throwForce = new Vector3(
                    Random.Range(throwForceMin.x, throwForceMax.x),
                    Random.Range(throwForceMin.y, throwForceMax.y),
                    Random.Range(throwForceMin.z, throwForceMax.z));

                Debug.Log(throwForce);

                corpseInstance.SetAsRagdoll();
                corpseInstance.mainRigidbody.AddForce(throwForce);
                MeatProcessingStep++;
                StartCoroutine(NonThrowingTime());
            }
        }
        if (MeatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                ProcessCurrentCorpse(meatPosters[currentPosterIndex]);
            }
        }
    }

    public void OnSelectLeftPosterClick(CallbackContext context)
    {
        if (MeatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                if (currentPosterIndex == 0)
                {
                    currentPosterIndex = meatPosters.Count - 1;
                }
                else
                {
                    currentPosterIndex--;
                }
                SelectMeatPoster(meatPosters[currentPosterIndex]);
            }
        }
    }

    public void OnSelectRightPosterClick(CallbackContext context)
    {
        if (MeatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            if (context.started)
            {
                if (currentPosterIndex == meatPosters.Count - 1)
                {
                    currentPosterIndex = 0;
                }
                else
                {
                    currentPosterIndex++;
                }
                SelectMeatPoster(meatPosters[currentPosterIndex]);
            }
        }
    }

    public void OnMeatPosterClick(MeatPoster meatPoster)
    {
        if (MeatProcessingStep == MeatProcessingStep.MeatAcquire)
        {
            SelectMeatPoster(meatPoster);
            ProcessCurrentCorpse(meatPoster);
        }
    }

    private void ProcessCurrentCorpse(MeatPoster meatPoster)
    {
        meatPoster.SpawnMeat(corpseInstance);
        Destroy(corpseInstance.gameObject);
        MeatProcessingStep = MeatProcessingStep.CorpseThrowing;
        meatPoster.SetPosterAsSelected(false);
    }

    private void SelectMeatPoster(MeatPoster meatPoster)
    {
        DeselectAllMeatPosters();
        meatPoster.SetPosterAsSelected(true);
    }

    private void DeselectAllMeatPosters()
    {
        meatPosters.ForEach(poster => poster.SetPosterAsSelected(false));
    }

#if UNITY_ANDROID
    private void CheckCorpseThrowButton(MeatProcessingStep newStep)
    {
        corpseThrowMobileButtonRenderer.SetActive(newStep == MeatProcessingStep.CorpseThrowing);
    }
#endif

    private IEnumerator NonThrowingTime()
    {
        yield return new WaitForSecondsRealtime(nonThrowingDuration);
        MeatProcessingStep++;
        meatPosters[currentPosterIndex].SetPosterAsSelected(true);
    }
}

public enum MeatProcessingStep
{
    CorpseThrowing = 0,
    CorpseFlying = 1,
    MeatAcquire = 2,
}