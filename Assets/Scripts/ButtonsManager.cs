using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public static ButtonsManager instance;

    [SerializeField]
    private GameObject m_pauseMenu;

    [SerializeField]
    private GameObject m_backButton;

    [SerializeField]
    private GameObject m_menuButton;

    private void Awake()
    {
        instance = this;
    }

    public void Settings()
    {
        bool menu = SettingsManager.Instance.m_inMenu;
        SettingsManager.Instance.m_inMenu = !menu;
        if (menu)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
        m_menuButton.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            m_menuButton.SetActive(false);
        }
        m_pauseMenu.SetActive(!menu);
    }

    public void Menu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
        m_menuButton.SetActive(false);
    }
}
