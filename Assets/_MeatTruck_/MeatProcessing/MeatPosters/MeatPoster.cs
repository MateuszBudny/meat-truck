using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPoster : MonoBehaviour
{
    public GameObject highlight;
    public Meat meat;

    public void SetPosterAsSelected(bool activate)
    {
        highlight.SetActive(activate);
    }
}
