using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Settings;

    private void Start()
    {
        if (SettingsManager.Instance == null) 
        {
            Instantiate(Settings);
        }
        else
        {
            SettingsManager.Instance.gameObject.SetActive(false);
        }
        Time.timeScale = 1.0f;
    }

    public void Play()
    {
        if (!SettingsManager.Instance.m_inMenu)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Game");
        }
    }

    public void Quit()
    {
        if (!SettingsManager.Instance.m_inMenu)
        {
            Application.Quit();
        }
    }

    public void MenuSettings()
    {
        ButtonsManager.instance.Settings();
    }
}
