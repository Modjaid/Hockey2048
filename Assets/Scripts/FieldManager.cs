using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldManager : MonoBehaviour
{
    public static UnityEvent OnPuckChanged;
    [Space(5)]
    [TextArea(2, 4)]
    [SerializeField] private string INFO_1;
    [Header("Интервал времени между генерацией новых шайб (В секундах)")]
    [Range(0,40)]
    [SerializeField] private float generatorInterval;
    [Header("балл вероятности инстацирования определенной шайбы!")]
    [Tooltip("Балл шайбы с 2 очками")]
    [Range(0,100)]
    [SerializeField] private int firstPuckBall;
    [Tooltip("Балл шайбы с 4 очками")]
    [Range(0, 100)]
    [SerializeField] private int secondPuckBall;
    [Tooltip("Балл шайбы с 8 очками")]
    [Range(0, 100)]
    [SerializeField] private int thirdPuckBall;

    #region Game TEST
    [Space(5)]
    [TextArea(2, 4)]
    [SerializeField]private string INFO_2;
    [Header("Параметры шайбы игрока")]
    [Tooltip("Сила толчка контроллера")]
    [SerializeField] private float playerPuckForceImpulse;
    [Tooltip("Масса шайбы игрока")]
    [SerializeField] private float playerPuckMass;
    [Tooltip("Трение шайбы игрока")]
    [SerializeField] private float playerPuckDrag;
    [Tooltip("Трение шайбы игрока")]
    [SerializeField] private float playerPuckAngularDrag;

    [Space(5)]
    [Header("Параметры остальных шайб")]
    [Tooltip("Масса шайбы")]
    [SerializeField] private float puckMass;
    [Tooltip("Трение шайбы")]
    [SerializeField] private float puckDrag;
    [Tooltip("Трение шайбы")]
    [SerializeField] private float puckAngularDrag;

    private PlayerPuckController playerController;
    private Rigidbody playerRB;
    #endregion

    [Space(5)]
    [Header("References")]
    [Tooltip("Массив Шайб (Первый Puck игрока)")]
    [SerializeField] private Transform PucksParent;
    [SerializeField] private PuckGenerator puckGenerator;
    [SerializeField] private ColorManager colorManager;
    [Tooltip("Ворота")]
    [SerializeField] private GoalKeeper[] goalKeepers;
    [SerializeField] private GameObject[] gates;
    [SerializeField] private GameObject[] madeWalls;

    private Rigidbody[] rbPucks;
    private PuckController[] pucks;

    private void Awake()
    {
        OnPuckChanged = new UnityEvent();
    }

    private void Start()
    {
        Test_InitPucks();
        UpdatePucks();
        Test_UpdatePucks();
        ControlHallForSuitPuck();
        OnPuckChanged.AddListener(UpdatePucks);
        OnPuckChanged.AddListener(ControlHallForSuitPuck);
    }

    private void Update()
    {
        puckGenerator.Test_GeneratorUpdate(firstPuckBall,secondPuckBall,thirdPuckBall,generatorInterval);
      //  Test_UpdatePucks();

      //  int[] ids = new int[pucks.Length];
      //  for(int i = 0; i < pucks.Length; i++)
      //  {
      //      
      //  }
    }

    private void Test_InitPucks()
    {
        playerController = PucksParent.GetChild(0).GetComponent<PlayerPuckController>();
        playerRB = PucksParent.GetChild(0).GetComponent<Rigidbody>();
    }

    private void Test_UpdatePucks()
    {
        playerController.forceImpulse = playerPuckForceImpulse;
        playerRB.mass = playerPuckMass;
        playerRB.drag = playerPuckDrag;
        playerRB.angularDrag = playerPuckAngularDrag;

        for (int i = 0; i < pucks.Length; i++)
        {
            rbPucks[i].mass = puckMass;
            rbPucks[i].drag = puckDrag;
            rbPucks[i].angularDrag = puckAngularDrag;
        }
    }

    


    /// <summary>
    /// Сравнивает очки каждой шайбы с воротами, и если ок, дает проходку шайбе передавая свой слой
    /// </summary>
    private void ControlHallForSuitPuck()
    {
        foreach (PuckController puck in pucks)
        {
            puck.gameObject.layer = (int)Layer.Default;
        }

        for(int i = 0; i < pucks.Length; i++)
        {
            int puckPoint = pucks[i].point;

            for (int j = 0; j < goalKeepers.Length; j++)
            {
                if(puckPoint == goalKeepers[j].point)
                {
                    pucks[i].gameObject.layer = goalKeepers[j].gameObject.layer;
                    Debug.Log($"Ворота открылись для {puckPoint} очки ворот: {goalKeepers[j].point} Слой ворот: {goalKeepers[j].gameObject.layer}");
                }
            }
           // Debug.Log($"PUCK POINTS = {pucks[i].point}");
            Color suitColor = colorManager.GetNeededColor(pucks[i].point);
            pucks[i].SetSuitColor(suitColor);
        }
    }

    private void UpdatePucks()
    {
        // Debug.Log("PucksCounter " + PucksParent.childCount);
        if (PucksParent.childCount <= 1)
        {
            pucks = new PuckController[0];
            rbPucks = new Rigidbody[0];
            return;
        }

        rbPucks = new Rigidbody[PucksParent.childCount - 1];
        pucks = new PuckController[PucksParent.childCount - 1];

        for(int  i = 1; i < PucksParent.childCount; i++)
        {
            rbPucks[i - 1] = PucksParent.GetChild(i).GetComponent<Rigidbody>();
            pucks[i - 1] = PucksParent.GetChild(i).GetComponent<PuckController>();
        }
    }

}
public enum Layer
{
    Default = 0,
    Gate_1 = 8,
    Gate_2 = 9,
    Gate_3 = 10,
    Gate_4 = 11
}
public enum PuckOperation
{
    Remove,
    Instance,
    Goal
}
