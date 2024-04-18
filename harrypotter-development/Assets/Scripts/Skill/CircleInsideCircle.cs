using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleInsideCircle : Instancable<CircleInsideCircle>
{
    public Joystick joystick;
    public Transform littleCircle, bigCircle;
    public Transform hitArea;
    public float multiplier = 5f;
    public Transform centralArea;
    public Transform bowPrimaries;
    public Transform physicalPrimaries;
    
    public bool skillUsing, skillOn;

    private void Update()
    {
        //if (skillUsing && !skillOn)
        {
            if(littleCircle != null)
            {
                littleCircle.transform.localPosition =
                new Vector3(joystick.Horizontal * multiplier, joystick.Vertical * multiplier, 0);
            }

            Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            print("Direction x: " + direction.x + "Direction y: " + direction.z);

            //centralArea.LookAt(centralArea.position + direction);
            //bowPrimaries.LookAt(bowPrimaries.position + direction);
        }
        Vector3 _direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        bowPrimaries.LookAt(bowPrimaries.position + _direction);

    }
}
