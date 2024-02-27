using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;


    void Start()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
        //SceneManager.LoadScene("1");

    }
    public void BackMenu()
    {
        SceneManager.LoadScene("Menu");
        //SceneManager.LoadScene("0");

    }

    public void Settings()
    {
        if(OptionsMenu.activeSelf==false) OptionsMenu.SetActive(true); 
        else if (OptionsMenu.activeSelf==true) OptionsMenu.SetActive(false);
        if (MainMenu.activeSelf == false) MainMenu.SetActive(true);
        else if (MainMenu.activeSelf == true) MainMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
    /*void Update()
    {
        
    }*/
}
