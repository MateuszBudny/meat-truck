using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class DrivingGameplayManager : SingleBehaviour<DrivingGameplayManager>
{
    [SerializeField]
    private List<ControllerModeRecord> controllerModesRecords;
    public Queue<ControllerMode> ControllerModes { get; private set; }

    [SerializeField]
    private GameObject mobileVirtualControllers;

    public VehicleController PlayerVehicleController { get; set; }
    public ControllerMode CurrentControllerMode => ControllerModes.Peek();

    protected override void Awake()
    {
        base.Awake();

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

    public void OnRestartInput(CallbackContext context)
    {
        if(context.performed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnQuitInput(CallbackContext context)
    {
        if(context.performed)
        {
            Application.Quit();
        }
    }
}
