﻿using UnityEngine;

public class Gun : MonoBehaviour
{
   // public GameObject BulletPrefab;
    public float Power = 100;

    public TrajectoryRendererAdvanced Trajectory;
    //public TrajectoryRendererAdvanced Trajectory;

    private Camera mainCamera;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
     //   float enter;
     //   Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
     //   new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
     //   Vector3 mouseInWorld = ray.GetPoint(enter);
     //
     //   Vector3 speed = (mouseInWorld - transform.position) * Power;
     //   transform.rotation = Quaternion.LookRotation(speed);
      //  Trajectory.ShowTrajectory(transform.position, speed);

    }
}
