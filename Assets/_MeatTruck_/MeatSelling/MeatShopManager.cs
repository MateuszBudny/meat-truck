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
    [SerializeField]
    private CashTableData cashTable;

    public List<OnRouteToMeatShopNpcCharacterState> CustomersWalkingToShop { get; private set; } = new List<OnRouteToMeatShopNpcCharacterState>();

    private Dictionary<MeatData, MeatSpawnPoint> meatSpawnPointsDict = new Dictionary<MeatData, MeatSpawnPoint>();
    private List<MeatBehaviour> spawnedMeats;
    private List<CashBehaviour> spawnedEarnedCash;

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
            newMeat.meat = meat;
            spawnedMeats.Add(newMeat);
        });

        spawnedEarnedCash = new List<CashBehaviour>();
    }

    public void ReturnToDriving()
    {
        mainInput.SwitchCurrentActionMap(PlayerInputActionMap.Driving.ToString());

        virtualCamera.Priority = 0;
        DrivingGameplayManager.Instance.CurrentControllerMode.VirtualCamera.Priority++;

        List<IItemJumpTo> itemsToGather = new List<IItemJumpTo>();
        itemsToGather.AddRange(spawnedMeats);
        itemsToGather.AddRange(spawnedEarnedCash);
        itemsToGather.ForEach(item => item.JumpTo(playerVehicle.VehicleController.CenterOfMass.transform.position, false, () => Destroy(item.GameObject), true));

        List<OnRouteToMeatShopNpcCharacterState> customersToInformAboutShopClosing = new List<OnRouteToMeatShopNpcCharacterState>(CustomersWalkingToShop);
        customersToInformAboutShopClosing.ForEach(customer => customer.OnMeatShopClosed());
        CustomersWalkingToShop.Clear();

        rangeGameObject.SetActive(false);
    }

    public void CustomerBuy(NpcCharacterBehaviour customer, MeatData meatDataBought)
    {
        if(meatDataBought)
        {
            MeatBehaviour spawnedMeatBehaviourBought = spawnedMeats.Find(meatBehaviour => meatBehaviour.meat.Data == meatDataBought);
            spawnedMeats.Remove(spawnedMeatBehaviourBought);
            GameManager.Instance.Player.Inventory.Meats.Remove(spawnedMeatBehaviourBought.meat);

            MeatPopularity boughtMeatPopularity = customer.CurrentCityRegion.meatsPopularity.List.Find(meatPopularity => meatPopularity.meatData == meatDataBought);
            boughtMeatPopularity.DecreasePopularityAfterBuy();
            
            spawnedMeatBehaviourBought.JumpTo(customer.mainRigidbody.position, true, () => Destroy(spawnedMeatBehaviourBought.gameObject), true);

            int cashValueEarned = boughtMeatPopularity.GetMeatFinalValue();
            for(int i = 0; i < cashValueEarned; i++)
            {
                spawnedEarnedCash.Add(Instantiate(cashTable.CashValues[1], customer.mainRigidbody.position, Quaternion.identity, transform));
            }
            GameManager.Instance.Player.Inventory.Cash += cashValueEarned;
        }
    }

    public void OnReturnToDrivingInput(CallbackContext context)
    {
        if(context.started)
        {
            ReturnToDriving();
        }
    }
}
