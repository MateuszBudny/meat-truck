using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MeatShopManager : SingleBehaviour<MeatShopManager>
{
    [SerializeField]
    private PlayerVehicle playerVehicle;
    [SerializeField]
    private PlayerInput mainInput;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private List<MeatSpawnPoint> meatSpawnPoints;
    [SerializeField]
    private GameObject rangeGameObject;
    public Waypoint customerEntryWaypoint;

    public List<OnRouteToMeatShopNpcCharacterState> CustomersWalkingToShop { get; private set; } = new List<OnRouteToMeatShopNpcCharacterState>();

    private Dictionary<MeatData, MeatSpawnPoint> meatSpawnPointsDict = new Dictionary<MeatData, MeatSpawnPoint>();
    private List<MeatBehaviour> spawnedMeats;

    protected override void Awake()
    {
        base.Awake();

        meatSpawnPoints.ForEach(meatSpawnPoint =>
        {
            meatSpawnPointsDict.Add(meatSpawnPoint.meatDataToSpawn, meatSpawnPoint);
        });

        rangeGameObject.SetActive(false);
    }

    public void OpenShop()
    {
        rangeGameObject.SetActive(true);

        mainInput.SwitchCurrentActionMap(PlayerInputActionMap.MeatSelling.ToString());

        DrivingGameplayManager.Instance.CurrentControllerMode.VirtualCamera.Priority = 0;
        virtualCamera.Priority++;

        spawnedMeats = new List<MeatBehaviour>();
        GameManager.Instance.Player.Inventory.Meats.ForEach(meat =>
        {
            MeatBehaviour newMeat = Instantiate(meat.Data.meatPrefabBehaviour, meatSpawnPointsDict[meat.Data].transform.position, Quaternion.identity, transform);
            spawnedMeats.Add(newMeat);
        });
    }

    public void ReturnToDriving()
    {
        mainInput.SwitchCurrentActionMap(PlayerInputActionMap.Driving.ToString());

        virtualCamera.Priority = 0;
        DrivingGameplayManager.Instance.CurrentControllerMode.VirtualCamera.Priority++;

        spawnedMeats.ForEach(meat =>
        {
            meat.CollidersHandler.CollidersSetEnabled(false);
            meat.transform.DOPunchScale(new Vector3(2f, 2f, 2f), 0.8f, 1, 1f)
                .Play();
            meat.transform.DOJump(playerVehicle.VehicleController.CenterOfMass.transform.position, 3f, 1, 1f)
                .OnComplete(() => Destroy(meat.gameObject))
                .Play();
        });

        List<OnRouteToMeatShopNpcCharacterState> customersToInformAboutShopClosing = new List<OnRouteToMeatShopNpcCharacterState>(CustomersWalkingToShop);
        customersToInformAboutShopClosing.ForEach(customer => customer.OnMeatShopClosed());
        CustomersWalkingToShop.Clear();

        rangeGameObject.SetActive(false);
    }

    public void CustomerBuy(NpcCharacterBehaviour customer)
    {
        Debug.Log("customer bought something!");
    }

    public void OnReturnToDrivingInput(CallbackContext context)
    {
        if(context.started)
        {
            ReturnToDriving();
        }
    }
}
