using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuckController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Joystick playerJoystick;
    [SerializeField] private TrajectoryRendererAdvanced trajectory;
    public float forceImpulse { get; set; }

    private void Start()
    {
        StartCoroutine(AutoSwitchPlayerJoystick());
    }

    public void PushByEvent(float force, Vector2 direction)
    {
        var worldDir = new Vector3(direction.x, 0, direction.y);
        trajectory.ShowTrajectory(this.transform.position, Vector3.zero);
        rigidbody.AddForce(worldDir.normalized * forceImpulse * force, ForceMode.Impulse);
       // trajectory.ShowTrajectory(this.transform.position, worldDir.normalized * forceImpulse * force);
    }

    public void ShowTrajectory(float force, Vector2 direction)
    {
        var worldDir = new Vector3(direction.x, 0, direction.y);
        trajectory.ShowTrajectory(this.transform.position, worldDir.normalized * forceImpulse * force);
    }

    private IEnumerator AutoSwitchPlayerJoystick()
    {
        bool IsChanged = false;
        while (true)
        {
            if (rigidbody.velocity.magnitude < 1f && IsChanged)
            {
                playerJoystick.enabled = true;
                playerJoystick.EnableActiveAnimation();
                IsChanged = false;
            }
            else if(rigidbody.velocity.magnitude > 1f)
            {
                playerJoystick.enabled = false;
                IsChanged = true;
            }
            Debug.Log("IsChanged = " + IsChanged);
            yield return null;
        }
    }
}
