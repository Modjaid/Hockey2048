using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestFieldUI : MonoBehaviour
{
    [SerializeField] private Button buttonX2;
    [SerializeField] private Button buttonX4;
    [SerializeField] private Button buttonX8;
 //   [SerializeField] private GameObject destroyer; // Отключить на время чтобы не удалял новые шайбы
    [SerializeField] private Text autoGenButtonText;
    [SerializeField] private PuckGenerator puckGenerator;

    public void Stun()
    {
        buttonX2.interactable = false;
        buttonX4.interactable = false;
        buttonX8.interactable = false;
    //    destroyer.SetActive(false);
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(0.5f);
            buttonX2.interactable = true;
            buttonX4.interactable = true;
            buttonX8.interactable = true;
      //      destroyer.SetActive(true);
        }

    }


    public void ReloadScene()
    {
        SceneManager.LoadScene("Field 1");
    }

    public void AutoGenButton()
    { 
        if (puckGenerator.GenIsActive)
        {
            puckGenerator.StopAutoGenerator();
            autoGenButtonText.text = "AutoGen On";
        }
        else
        {
            puckGenerator.StartAutoGenerator();
            autoGenButtonText.text = "AutoGen Off";
        }
    }

}
