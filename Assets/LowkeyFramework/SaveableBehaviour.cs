using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a part of this code has been stolen from: https://github.com/UnityTechnologies/open-project-1

public abstract class SaveableBehaviour : MonoBehaviour
{
	[SerializeField, HideInInspector] private string guid;
	public string Guid => guid;

#if UNITY_EDITOR
	void OnValidate()
	{
		if(string.IsNullOrEmpty(guid))
        {
			guid = System.Guid.NewGuid().ToString();
        }
	}
#endif
}
