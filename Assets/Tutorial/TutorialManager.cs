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
// ctrl+r+r: rename

public class TutorialManager : MonoBehaviour
{
    public GameObject cube;
    public int cubesSpawnNum = 2;
    public GameObject spawnPoint;
    public Vector3 explosionForceMin = new Vector3(-200f, 300f, -200f);
    public Vector3 explosionForceMax = new Vector3(200f, 1500f, 200f);

    [Header("Light bulb")]
    public Light lightBulb;

    private List<GameObject> cubes = new List<GameObject>();

    private LightState lightState = LightState.Full;

    private void Start()
    {
        for (int i = 0; i < cubesSpawnNum; i++)
        {
            cubes.Add(Instantiate(cube, spawnPoint.transform));
        }
    }

    public void OnCubeJumpClick(CallbackContext context)
    {
        if (context.started)
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                Vector3 explosionForce = new Vector3(
                    Random.Range(explosionForceMin.x, explosionForceMax.x),
                    Random.Range(explosionForceMin.y, explosionForceMax.y),
                    Random.Range(explosionForceMin.z, explosionForceMax.z));

                Debug.Log(i + ", force: " + explosionForce);
                cubes[i].GetComponent<Rigidbody>().AddForce(explosionForce);
            }
        }
    }
    private void SetLighParameters(bool onoff, Color lightColor, float intensity = 1f)
    {
        lightBulb.enabled = onoff;
        lightBulb.intensity = intensity;
        lightBulb.color = lightColor;
    }

    public void OnLightButtonClicked(CallbackContext context)
    {
        if(context.started)
        {
            if (lightState == LightState.Off)
            {
                lightState = LightState.Full;
            }
            else
            {
                lightState++;
            }

            if (lightState == LightState.Full)
            {
                SetLighParameters(true, Color.white, 1);
            }
            else if (lightState == LightState.Dim)
            {
                SetLighParameters(true, Color.white, 0.5f);
            }
            else if (lightState == LightState.Red)
            {
                SetLighParameters(true, Color.red, 1);
            }
            else if (lightState == LightState.Off)
            {
                SetLighParameters(false, Color.white);
            }
        }
    }
}

public enum LightState
{
    Full = 0,
    Dim = 1,
    Red = 2,
    Off = 3,
}
