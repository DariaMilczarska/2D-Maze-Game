using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeChooser : MonoBehaviour
{     
    public void FullScreen()
    {
        Screen.fullScreen = true;
        LoadMenu();
    }

    public void Windowed()
    {
        Screen.SetResolution(1280, 720, false);
        Screen.fullScreen = false;
        LoadMenu();
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
