using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRendererAdvanced : MonoBehaviour
{
    public GameObject playerPuck;
    [SerializeField] private Transform pucksParent;
    [SerializeField] private GameObject[] destroyers;
 //   [SerializeField] private Collider[] timeCollider;

    private LineRenderer lineRendererComponent;
    public Dictionary<Rigidbody, BodyData> savedBodies = new Dictionary<Rigidbody, BodyData>();

    private Rigidbody[] puckRBs;
    private PuckController[] puckControllers;
    

    private void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
        PuckCollectionUpdate();

        FieldManager.OnPuckChanged.AddListener(PuckCollectionUpdate);
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        // Подготовка:
        foreach (var body in savedBodies)
        {
            body.Value.position = body.Key.transform.position;
            body.Value.rotation = body.Key.transform.rotation;
            body.Value.velocity = body.Key.velocity;
            body.Value.angularVelocity = body.Key.angularVelocity;
        }

        GameObject bullet = Instantiate(playerPuck, origin, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(speed * 16, ForceMode.Force);

        Physics.autoSimulation = false;

        // Симуляция:
        Vector3[] points = new Vector3[50];
        
        points[0] = origin;
        for (int i = 1; i < points.Length; i++)
        {
            Physics.Simulate(.1f);

            points[i] = bullet.transform.position;
        }

        lineRendererComponent.SetPositions(points);

        // Зачистка:
        Physics.autoSimulation = true;

        foreach (var body in savedBodies)
        {
            body.Key.transform.position = body.Value.position;
            body.Key.transform.rotation = body.Value.rotation;
            body.Key.velocity = body.Value.velocity;
            body.Key.angularVelocity = body.Value.angularVelocity;
        }
        Destroy(bullet.gameObject);
    }

    /// <summary>
    /// Вызывается через события Joystick - OnFingerUp(true) OnFingerDown(false)
    /// </summary>
    /// <param name="isActive"></param>
    public void ActiveColliderObjects(bool isActive)
    {
        foreach (GameObject destroyer in destroyers)
        {
            destroyer.SetActive(isActive);
        }

        for(int i = 0; i < puckControllers.Length; i++)
        {
            puckControllers[i].SwitchObjectActive(isActive);
        }
    }


    public void PuckCollectionUpdate()
    {
        int puckLength = pucksParent.childCount - 1; // Минус один шайба - игрока не считается
        puckRBs = new Rigidbody[puckLength];
        puckControllers = new PuckController[puckLength];

        for (int i = 1; i < pucksParent.childCount; i++) // Первый дочерний всегда шайба игрока
        {
            Transform child = pucksParent.GetChild(i);
            puckRBs[i - 1] = child.GetComponent<Rigidbody>();
            puckControllers[i - 1] = child.GetComponent<PuckController>();
        }
        SavedBodiesUpdate();
    }

    private void SavedBodiesUpdate()
    {
        savedBodies.Clear();
        for (int i = 0; i < puckRBs.Length; i++)
        {
            savedBodies[puckRBs[i]] = new BodyData();
        }
        var playerRB = playerPuck.GetComponent<Rigidbody>();
        savedBodies[playerRB] = new BodyData();
    }
}

public class BodyData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;
}