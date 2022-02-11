using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageEntrance : MonoBehaviour
{
    [SerializeField]
    private string garageSceneName;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag(Tags.PlayerVehicle.ToString()))
        {
            SaveManager.Instance.Save();
            SceneManager.LoadScene(garageSceneName);
        }
    }
}