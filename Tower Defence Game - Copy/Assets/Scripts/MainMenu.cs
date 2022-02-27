using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingMenu;
    public GameObject helpMenu;
    public GameObject creditMenu;
    public GameObject levelSelect;
    AudioSource buttonClick;

    private void Start()
    {
        buttonClick = GetComponent<AudioSource>();
    }

    public void Quit()
    {
        PlayerPrefs.SetFloat("SliderBMGLevel", 5);
        PlayerPrefs.SetFloat("SliderEffectLevel", 5);
        Application.Quit();
    }
    public void LevelSelectActive()
    {
        buttonClick.Play();
        levelSelect.SetActive(true);
    }
    public void LevelSelectClose()
    {
        buttonClick.Play();
        levelSelect.SetActive(false);
    }
    
    public void SelectLevel(string Level)
    {
        buttonClick.Play();
        SceneManager.LoadScene(Level);
    }

    public void OpenSetting()
    {
        buttonClick.Play();
        settingMenu.SetActive(true);
    }
    public void OpenHelp()
    {
        buttonClick.Play();
        helpMenu.SetActive(true);
    }
    public void CloseSetting()
    {
        buttonClick.Play();
        settingMenu.SetActive(false);
    }
    public void CloseHelp()
    {
        buttonClick.Play();
        helpMenu.SetActive(false);
    }
    public void OpenCredits()
    {
        buttonClick.Play();
        creditMenu.SetActive(true);
    }
    public void CloseCredits()
    {
        buttonClick.Play();
        creditMenu.SetActive(false);
    }
}
