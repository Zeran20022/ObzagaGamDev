using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject GameMenu;
    
    private bool isPaused =false;

    void Start()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Play()
    {
        SceneTransition.SwitchToScene("Game");
        //SceneManager.LoadScene("1");

    }
    public void BackMenu()
    {
        Time.timeScale = 1f; // ������������� ������� ��� ������ � ����
        SceneTransition.SwitchToScene("Menu");
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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

            //if(GameMenu.activeSelf == false) GameMenu.SetActive(true);
            //else if (GameMenu.activeSelf == true) GameMenu.SetActive(false);
        }

        /*if(Input.GetKeyDown(KeyCode.Escape)&&SceneManager.GetActiveScene().name =="Game") 
        {
            SceneTransition.SwitchToScene("Menu");
        }*/
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // ��������� ������� ��� �����

        Cursor.lockState = CursorLockMode.None; // ������������� �������
        Cursor.visible = true; // ����������� �������

        GameMenu.SetActive(true); // ����������� ����-����
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // ������������� �������

        Cursor.lockState = CursorLockMode.Locked; // ���������� �������
        Cursor.visible = false; // ������� �������

        GameMenu.SetActive(false); // ������� ����-����
        isPaused = false;
    }
}
