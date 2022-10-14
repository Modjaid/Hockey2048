using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Header("Дефолтный цвет всех шайб")]
    [SerializeField] private Color defaultColor;
    [Header("Текст очков каждой воротины")]
    [SerializeField] private TextMeshProUGUI[] pointGateTexts;
    [Header("Цвета ворот")]
    [SerializeField] private Color[] gateColors;
    [SerializeField] private GoalKeeper[] goalKeepers;
    [SerializeField] private Transform[] gateModels;

    public void Start()
    {
        RenderGates();
    }

    public Color GetNeededColor(int point)
    {
        for(int i = 0; i < goalKeepers.Length; i++)
        {
            if (point == goalKeepers[i].point && goalKeepers[i].transform.parent.gameObject.activeSelf)
            {
                Debug.Log($"Возвращает цвет с индексом {i}");
                return gateColors[i];
            }
        }
        return defaultColor;
    }

    private void RenderGates()
    {
        for(int i = 0; i < gateModels.Length; i++)
        {
            Debug.Log("CurrentModels i : " + gateModels[i].childCount);
            for(int j = 0; j < gateModels[i].childCount; j++)
            {
                MeshRenderer render = gateModels[i].GetChild(j).GetComponent<MeshRenderer>();
                render.material.color = gateColors[i];
            }
        }
    }
}
