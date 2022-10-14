using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private Text consoleText;
    

    private static TestUI instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelGameScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelGameScene;
    }

    private void OnLevelGameScene(Scene scene, LoadSceneMode sceneMode)
    {

    }

    /// <summary>
    /// Событие кнопки
    /// </summary>
    public void clickSDKOptions()
    {
        if (OptionsPanel.activeSelf)
        {
            OptionsPanel.SetActive(false);
        }
        else
        {
            OptionsPanel.SetActive(true);
        }
    }
}
