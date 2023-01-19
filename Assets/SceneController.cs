using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private static SceneController INSTANCE;
    
    private void Awake() {
        if (INSTANCE != null) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            INSTANCE = this;
        }
    }


    public void StartGame() {
        SceneManager.LoadScene("GameScreen");
    }


    public void Leaderboard()
    {
        SceneManager.LoadScene("LeaderboardScreen");
    }

    public void Signup() {
        
    }
    
    
}