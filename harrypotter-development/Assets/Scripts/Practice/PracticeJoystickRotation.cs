using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeJoystickRotation : Instancable<PracticeJoystickRotation>
{
   public Joystick joystick;
   public GameObject hitArea;
   public float rotateVertical;
   public float rotateHorizontal;

   private void FixedUpdate()
   {
      rotateVertical = joystick.Vertical * 1f;
      rotateHorizontal = joystick.Horizontal * -1f;
      hitArea.transform.Rotate(90, rotateVertical, rotateHorizontal);
   }
}