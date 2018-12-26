
using System.Collections;
using UnityEngine;
using UnityTools;
using UnityTools.SceneManagement;

public class GameManager : AGameManager<GameManager, GameManager.State>
{
    public enum State
    {
        Menu,
        Play,
        Loading,
        Pause,  
        GameOver
    }

    private void Start()
    {
        Invoke("LoadNextLevel", 2);
    }

    private void LoadNextLevel()
    {
        SceneManager.Instance.LoadNextLevel();
    }
}

