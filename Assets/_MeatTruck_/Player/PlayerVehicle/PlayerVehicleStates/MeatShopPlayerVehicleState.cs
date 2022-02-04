using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MeatShopPlayerVehicleState : PlayerVehicleState
{
    private InputActionMap otherDrivingActionMap;
    private InputActionMap meatSellingActionMap;

    public MeatShopPlayerVehicleState(PlayerVehicle playerVehicle) : base(playerVehicle) 
    {
        otherDrivingActionMap = PlayerVehicle.mainInput.actions.FindActionMap(PlayerInputActionMap.OtherDriving.ToString());
        meatSellingActionMap = PlayerVehicle.mainInput.actions.FindActionMap(PlayerInputActionMap.MeatSelling.ToString());
    }

    public override float GetCurrentAcceleration() => 0f;

    public override float GetCurrentBraking() => 1f;

    public override float GetCurrentSteeringAngle() => 0f;

    public override void OnStateEnter(VehicleState previousState)
    {
        otherDrivingActionMap.Disable();
        meatSellingActionMap.Enable();

        MeatShopManager.Instance.OpenShop();
    }

    public override void ReturnToDriving(CallbackContext context)
    {
        if(context.started)
        {
            otherDrivingActionMap.Enable();
            meatSellingActionMap.Disable();

            PlayerVehicle.ChangeState(new DrivingLowVelocityPlayerVehicleState(PlayerVehicle));

            MeatShopManager.Instance.ReturnToDriving();
        }
    }
}
