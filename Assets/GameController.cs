using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField playerField;
    [SerializeField] private TMP_InputField scoreField;

    private string playerName;
    private int score;
    
    
    // Start is called before the first frame update
    void Start() {
        scoreField.contentType = TMP_InputField.ContentType.IntegerNumber;
        PlayerPrefs.SetString("PlayerID", "356");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitScore() {
        playerName = playerField.text;
        score = Convert.ToInt32(scoreField.text);
        LootLockerSDKManager.SetPlayerName(playerName, (resp) => {
            if (resp.success) {
                Debug.Log("Success for player name");
                LootLockerSDKManager.SubmitScore(playerName, score, "10514", OnSubmitScoreResponse);
                return;
            }
            Debug.Log("Failed player name change");
        });
        
    }

    void OnSubmitScoreResponse(LootLockerSubmitScoreResponse resp) {
        if (resp.statusCode == 200) {
            Debug.Log("Score submitted");
            Debug.Log("Text: " + resp.text);
            Debug.Log("Score: " + resp.score);
            Debug.Log("MemberID: " + resp.member_id);
            LootLockerSDKManager.GetScoreList("10514", 1, (resp) => {
                string name = resp.items[0].player.name;
                Debug.Log(name);
            });
            
            return;
        }
        Debug.Log("Failed with error: " + resp.Error);
    }
}
