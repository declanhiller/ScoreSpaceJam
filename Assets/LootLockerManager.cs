using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LootLocker;
using LootLocker.Requests;
using UnityEngine;

public class LootLockerManager : MonoBehaviour {

    public static LootLockerManager INSTANCE;

    private static readonly string LEADERBOARD_KEY = "10514";

    // public event Action<LootLockerSessionResponse> OnSessionStart;
    public event Action<LootLockerGuestSessionResponse> OnGuestSessionStart;
    public event Action<LootLockerSubmitScoreResponse> OnSubmittedScore;
    public event Action<PlayerNameResponse> OnPlayerNameSet;
    public event Action OnFail;

    public string playerName { get; private set; }


    private void Awake() {
        if (INSTANCE != null) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            INSTANCE = this;
        }
    }
    

    public void SetPlayerName(string name) {
        LootLockerSDKManager.SetPlayerName(name, OnPlayerNameSet);
    }


    public void StartGuestSession() {
        Debug.Log("Starting Guest Session");
        LootLockerSDKManager.StartGuestSession((resp) => {
            if (DidFail(resp, OnFail)) return;
            LootLockerSDKManager.GetPlayerName((playerNameResponse) => {
                if (!playerNameResponse.success) {
                    Debug.Log("Wasn't able to get player name");
                    playerName = "";
                    return;
                }

                playerName = playerNameResponse.name;
                OnGuestSessionStart?.Invoke(resp);
            });
        });
    }

    public void SubmitScore(int score) {
        LootLockerSDKManager.SubmitScore(playerName, score, LEADERBOARD_KEY, OnSubmittedScore);
    }

    public delegate Task LootOnScoreListResp<out T>() where T : LootLockerResponse;
    event LootOnScoreListResp<LootLockerGetScoreListResponse> MyEvent; 

    public async Task<LootLockerGetScoreListResponse> GetScoreListAsync() {
        
        TaskCompletionSource<LootLockerGetScoreListResponse> tcs1 = new TaskCompletionSource<LootLockerGetScoreListResponse>();
        
        LootLockerSDKManager.GetScoreList(LEADERBOARD_KEY, 5, 5, async (resp) => {
            if (!resp.success) {
                Debug.Log("Failed");
                return;
            }
            tcs1.SetResult(resp);
        });

        return await tcs1.Task;
    }
    
    static void MethodA(string message)
    {
        Console.WriteLine(message);
    }
    
    
    
    
    private bool DidFail(LootLockerResponse resp, Action onFail) {
        if (!resp.success) {
            onFail.Invoke();
            return true;
        }
        return false;
    }
    
}