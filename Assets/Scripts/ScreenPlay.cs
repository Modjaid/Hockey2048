using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenPlay : MonoBehaviour
{
    [SerializeField] private PuckGenerator generator;
    [SerializeField] private TrajectoryRendererAdvanced saveBodiesHelper;
    [SerializeField] private Dictionary<Rigidbody, BodyData> saveDictionary;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            generator.ProduceNewPuck(2);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            generator.ProduceNewPuck(4);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            generator.ProduceNewPuck(8);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DownloadSaving();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("SAVING");
            SavePositions();
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            SceneManager.LoadScene("Field 1");
        }
    }

    public void SavePositions()
    {
        saveBodiesHelper.PuckCollectionUpdate();
        var dictionary = saveBodiesHelper.savedBodies;
        saveDictionary = new Dictionary<Rigidbody, BodyData>();
        foreach (var body in dictionary)
        {
            saveDictionary[body.Key] = new BodyData();
            saveDictionary[body.Key].position = body.Key.transform.position;
            saveDictionary[body.Key].rotation = body.Key.transform.rotation;
            saveDictionary[body.Key].velocity = body.Key.velocity;
            saveDictionary[body.Key].angularVelocity = body.Key.angularVelocity;
        }
    }

    public void DownloadSaving()
    {
        foreach(var body in saveDictionary)
        {
            body.Key.transform.position = body.Value.position;
            body.Key.transform.rotation = body.Value.rotation;
            body.Key.velocity = body.Value.velocity;
            body.Key.angularVelocity = body.Value.angularVelocity;
        }
    }
}
