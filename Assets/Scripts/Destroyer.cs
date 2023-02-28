using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
   [SerializeField] private GameObject targetToDestroy;

   public void DestroyTargetGameObject()
   {
       Destroy(targetToDestroy);
   }
}
