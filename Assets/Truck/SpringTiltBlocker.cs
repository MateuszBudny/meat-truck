using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTiltBlocker : TiltBlocker
{
    [SerializeField]
    private GameObject springTiltBlockersParent;
    
    public override void OnTiltBlockerEnable(bool showInfo = true)
    {
        base.OnTiltBlockerEnable(showInfo);
        springTiltBlockersParent.SetActive(true);
    }

    public override void OnTiltBlockerDisable()
    {
        springTiltBlockersParent.SetActive(false);
    }
}
