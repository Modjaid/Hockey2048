using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckGenerator : MonoBehaviour
{
    public bool GenIsActive { get 
        { 
            if (autoGenThread != null) 
            {
                return true;
            }
            return false;
        } 
    }

    [SerializeField] private Transform pucksParent;
    [SerializeField] private GameObject puckPrefab;
    [SerializeField] private Transform start;
    [SerializeField] private Transform finish;

    private int firstPuckBall;
    private int secondPuckBall;
    private int thirdPuckBall;
    private float genInterval;

    private Coroutine autoGenThread;


    void Start()
    {
        
    }

    public void ProduceNewPuck(int point)
    {
        var newPuck = Instantiate(puckPrefab, pucksParent);
        newPuck.GetComponent<Rigidbody>().isKinematic = true;
        newPuck.GetComponent<Collider>().enabled = false;
        newPuck.transform.GetChild(0).GetComponent<Collider>().enabled = false;
        newPuck.GetComponent<PuckController>().point = point;
        newPuck.transform.position = start.position;

        FieldManager.OnPuckChanged?.Invoke();

        StartCoroutine(Producing());

        IEnumerator Producing()
        {
            var distance = Vector3.Distance(newPuck.transform.position, finish.position);
            while (distance > 1f)
            {
                if(distance < 10f)
                {
                    newPuck.GetComponent<Collider>().enabled = true;
                }
                newPuck.transform.position = Vector3.Lerp(newPuck.transform.position, finish.transform.position, Time.deltaTime * 3);
                distance = Vector3.Distance(newPuck.transform.position, finish.position);
                yield return null;
            }
            newPuck.GetComponent<Rigidbody>().isKinematic = false;
            newPuck.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        }
    }

    public void StartAutoGenerator()
    {
        autoGenThread = StartCoroutine(InstantiateWithRandom());
        IEnumerator InstantiateWithRandom()
        {
            while (true)
            {
                yield return new WaitForSeconds(genInterval);
                int numVictory;

                int sum = firstPuckBall + secondPuckBall + thirdPuckBall;
                int random = Random.Range(0, sum);
                if (random < firstPuckBall)
                {
                    
                    numVictory = 2;
                }else if(random < firstPuckBall + secondPuckBall)
                {
                    numVictory = 4;
                }
                else
                {
                    numVictory = 8;
                }
                Debug.Log("ЦИФРА " + random + "sum " + sum);
                ProduceNewPuck(numVictory);
            }
        }
    }

    public void StopAutoGenerator()
    {
        StopCoroutine(autoGenThread);
        autoGenThread = null;
    }

    public void Test_GeneratorUpdate(int first,int second,int third, float interval)
    {
        firstPuckBall = first;
        secondPuckBall = second;
        thirdPuckBall = third;
        genInterval = interval;
    }

}
