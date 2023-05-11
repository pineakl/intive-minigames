using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResolutionMaintainer : MonoBehaviour
{
    private const int DESIRED_WIDTH = 1366;
    private const int DESIRED_HEIGHT = 768;

    private int _savedWidth;
    private int _savedHeight;
    private FullScreenMode _fsMode;

    private void Start() 
    {
        _savedWidth = Screen.width;
        _savedHeight = Screen.height;
        _fsMode = Screen.fullScreenMode;

        Screen.SetResolution(DESIRED_WIDTH, DESIRED_HEIGHT, FullScreenMode.FullScreenWindow);
    }

    public void ExitMinigame()
    {
        Screen.SetResolution(_savedWidth, _savedHeight, _fsMode);

        //  Script Ganti Scene
        
        //  SceneManager.LoadScene("Nama Scene", LoadSceneMode.Single);
    }
}
