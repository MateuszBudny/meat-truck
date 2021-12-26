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
    private GameObject rangeGameObject;
    public Waypoint customerEntryWaypoint;
    [SerializeField]
    private CashTableData cashTable;
    [SerializeField]
    private MeatsSpawning meatsSpawning;

    public List<OnRouteToMeatShopNpcCharacterState> CustomersWalkingToShop { get; private set; } = new List<OnRouteToMeatShopNpcCharacterState>();

    private List<MeatBehaviour> spawnedMeats;
    private List<CashBehaviour> spawnedEarnedCash;
    private InputActionMap otherDrivingActionMap;
    private InputActionMap meatSellingActionMap;

    protected override void Awake()
    {
        base.Awake();
        rangeGameObject.SetActive(false);
        otherDrivingActionMap = mainInput.actions.FindActionMap(PlayerInputActionMap.OtherDriving.ToString());
        meatSellingActionMap = mainInput.actions.FindActionMap(PlayerInputActionMap.MeatSelling.ToString());
    }

    public void OpenShop()
    {
        rangeGameObject.SetActive(true);

        otherDrivingActionMap.Disable();
        meatSellingActionMap.Enable();

        DrivingGameplayManager.Instance.CurrentControllerMode.VirtualCamera.Priority = 0;
        virtualCamera.Priority++;

        spawnedMeats = meatsSpawning.SpawnGivenMeats(GameManager.Instance.Player.Inventory.Meats);

        spawnedEarnedCash = new List<CashBehaviour>();
    }

    public void ReturnToDriving()
    {
        otherDrivingActionMap.Enable();
        meatSellingActionMap.Disable();

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
