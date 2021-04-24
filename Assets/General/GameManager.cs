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
    public Queue<ControllerMode> ControllerModes { get; private set; }

    [SerializeField]
    private GameObject mobileVirtualControllers;

    public VehicleController Player { get; set; }
    public ControllerMode CurrentControllerMode => ControllerModes.Peek();

    public override void AdditionalOnAwake()
    {
        List<ControllerMode> tempControllerModes = new List<ControllerMode>(controllerModesRecords.Select(modeRecord => modeRecord.GetControllerMode()));

        // starting controller mode is that with highest VirtualCamera priority
        ControllerMode firstControllerMode = tempControllerModes.Aggregate(tempControllerModes.First(), (controllerModeWithHighestPriority, controllerModeRecord) =>
        {
            if(controllerModeRecord.VirtualCamera.Priority > controllerModeWithHighestPriority.VirtualCamera.Priority)
            {
                return controllerModeRecord;
            } else
            {
                return controllerModeWithHighestPriority;
            }
        });

        tempControllerModes.Remove(firstControllerMode);
        tempControllerModes.Insert(0, firstControllerMode);

        ControllerModes = new Queue<ControllerMode>(tempControllerModes);

#if UNITY_ANDROID
        mobileVirtualControllers.SetActive(true);
#else
        mobileVirtualControllers.SetActive(false);
#endif
    }

    public void OnChangeControllerModeInput(CallbackContext context)
    {
        if(context.performed)
        {
            ControllerModes.Enqueue(ControllerModes.Dequeue());
            CurrentControllerMode.OnModeChangedToThis();
        }
    }
}
