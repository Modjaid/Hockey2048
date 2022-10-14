using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    public int point;

    [SerializeField] private TextMeshProUGUI pointText;

    public void Start()
    {
        pointText.text = point.ToString();
    }
}
