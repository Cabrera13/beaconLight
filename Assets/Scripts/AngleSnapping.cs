using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AngleSnapping : MonoBehaviour
{
   public int angle = 45;
   
   void Update()
   {
       Vector3 vec = transform.eulerAngles;
       vec.x = Mathf.Round(vec.x / angle) * angle;
       vec.y = Mathf.Round(vec.y / angle) * angle;
       vec.z = Mathf.Round(vec.z / angle) * angle;
       transform.eulerAngles = vec;
   }
}