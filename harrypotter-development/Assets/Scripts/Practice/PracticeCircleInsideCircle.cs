using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeCircleInsideCircle : Instancable<PracticeCircleInsideCircle>
{
    public Joystick joystick;
    public Transform littleCircle, bigCircle;
    public Transform hitArea;
    public float multiplier = 5f;
    public Transform centralArea;
    public Transform bowPrimaries;
    public Transform physicalPrimaries;

    private void Update()
    {
        PracticeManager PM = PracticeManager.Instance;
        //if(!PM.skillCasted)
            littleCircle.transform.localPosition = new Vector3(joystick.Horizontal * multiplier, joystick.Vertical * multiplier, 0);
        
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        print("Direction x: " + direction.x + "Direction y: " + direction.z);
        
        centralArea.LookAt(centralArea.position + direction);
        bowPrimaries.LookAt(bowPrimaries.position + direction);
        physicalPrimaries.LookAt(bowPrimaries.position + direction);
        
    }
}
