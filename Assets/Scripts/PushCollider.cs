using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCollider : MonoBehaviour
{
    public float power = 0.8f;
    public float maxSpeed = 0.1f;

    public void OnTriggerEnter(Collider other)
   {
       if(other.tag == "Puck")
       {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = -rb.velocity;
            if (rb.velocity.magnitude <= maxSpeed)
                rb.velocity *= power;
        }
   }
}
