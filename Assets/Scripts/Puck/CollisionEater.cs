using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollisionEater : MonoBehaviour
{
    [SerializeField] private PuckController puck;
    [SerializeField] private GameObject collisionEffect;

    public void OnTriggerEnter(Collider other)
    {
        PuckController collisionPuck = other.gameObject.GetComponent<PuckController>();
        if (collisionPuck)
        {
            // id == -1 - Отключенно во время мерджа с другой шайбой
            if (puck.Id == collisionPuck.Id)
            {
                return;
            }
            else
            {
                collisionPuck.ChangeIdWithDelay(.1f);
            }

            if (puck.point == collisionPuck.point)
            {
                Instantiate(collisionEffect, transform.parent);
                Instantiate(collisionEffect, collisionPuck.transform);

                puck.MergePuck(collisionPuck, 40);
                FieldManager.OnPuckChanged?.Invoke();
            }


        }
    }
   // public void OnTriggerStay(Collider other)
   // {
   //     PuckController collisionPuck = other.gameObject.GetComponent<PuckController>();
   //     if (collisionPuck)
   //     {
   //         Debug.Log("COLLISION " + other.gameObject.name);
   //         collisionPuck.PushPuck();
   //     }
   // }


}
