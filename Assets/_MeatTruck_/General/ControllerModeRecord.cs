using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ControllerModeRecord
{
    private enum ControllerModeEnum {
        SideCameraControllerMode = 0,
        BackCameraControllerMode = 1,
    }

    [SerializeField]
    private ControllerModeEnum controllerModeEnum;
    public CinemachineVirtualCamera virtualCamera;

    public ControllerMode GetControllerMode()
    {
        switch(controllerModeEnum)
        {
            case ControllerModeEnum.SideCameraControllerMode:
                return new SideCameraControllerMode(virtualCamera);
            case ControllerModeEnum.BackCameraControllerMode:
                return new BackCameraControllerMode(virtualCamera);
            default:
                Debug.LogError("Unknown ControllerModeEnum!");
                return null;
        }
    }
}
