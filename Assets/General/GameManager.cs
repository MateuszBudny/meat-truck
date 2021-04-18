using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameManager : SingleBehaviour<GameManager>
{
    [SerializeField]
    private List<ControllerModeRecord> controllerModesRecords;
    public List<ControllerMode> ControllerModes { get; private set; }

    [SerializeField]
    private GameObject mobileVirtualControllers;

    public TruckController Player { get; set; }
    public ControllerMode CurrentControllerMode { get; set; }

    public override void AdditionalOnAwake()
    {
        ControllerModes = controllerModesRecords.Select(modeRecord => modeRecord.GetControllerMode()).ToList();

        // starting controller mode is that with highest VirtualCamera priority
        CurrentControllerMode = ControllerModes.Aggregate(ControllerModes.First(), (controllerModeWithHighestPriority, controllerModeRecord) =>
        {
            if(controllerModeRecord.VirtualCamera.Priority > controllerModeWithHighestPriority.VirtualCamera.Priority)
            {
                return controllerModeRecord;
            } else
            {
                return controllerModeWithHighestPriority;
            }
        });

#if UNITY_ANDROID
        mobileVirtualController.SetActive(true);
#else
        mobileVirtualControllers.SetActive(false);
#endif
    }

    public void OnChangeControllerModeInput(CallbackContext context)
    {
        if(context.performed)
        {
            int currentIndex = ControllerModes.IndexOf(CurrentControllerMode);
            int newIndex = currentIndex + 1 == ControllerModes.Count ? 0 : currentIndex + 1;
            CurrentControllerMode = ControllerModes[newIndex];

            CurrentControllerMode.OnModeChangedToThis();
        }
    }
}
