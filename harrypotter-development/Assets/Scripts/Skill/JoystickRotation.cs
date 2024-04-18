using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickRotation : Instancable<JoystickRotation>
{
   public Joystick joystick;
   public GameObject hitArea;
   public float rotateVertical;
   public float rotateHorizontal;

   private void FixedUpdate()
   {
      Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        
      hitArea.transform.LookAt(hitArea.transform.position + direction);
   }
}