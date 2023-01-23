using System;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static SceneController INSTANCE;

    // private void Awake() {
    //     if (INSTANCE != null) {
    //         Destroy(gameObject);
    //     } else {
    //         DontDestroyOnLoad(gameObject);
    //         INSTANCE = this;
    //     }
    // }

    private void Start() {
        INSTANCE = this;
    }


    public void StartGame() {
        SceneManager.LoadScene("GameScreen");
    }


    public void Leaderboard()
    {
        SceneManager.LoadScene("LeaderboardScreen");
    }

    public void StartScreen() {
        SceneManager.LoadScene("StartScreen");
    }
    
    public void EndGame() {
        
    }


}