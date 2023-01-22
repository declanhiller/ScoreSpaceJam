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
    public event Action<PlayerNameResponse> OnPlayerNameSet;
    public event Action OnFail;

    public string playerName { get; private set; }
    public int playerScore { get; set; }
    public int playerRank { get; set; }
    


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
                LootLockerSDKManager.GetMemberRank(LEADERBOARD_KEY, playerName, (resp) => {
                    playerScore = resp.score;
                    playerRank = resp.rank;
                });
                OnGuestSessionStart?.Invoke(resp);
            });
        });
    }

    public void SubmitScore(int score, Action<LootLockerSubmitScoreResponse> onResp) {
        LootLockerSDKManager.SubmitScore(playerName, score, LEADERBOARD_KEY, onResp);
    }

    public void GetScore(Action<LootLockerGetMemberRankResponse> onResp) {
        LootLockerSDKManager.GetMemberRank(LEADERBOARD_KEY, playerName, onResp);
    }

    public async Task<LootLockerGetScoreListResponse> GetScoreListAsync() {
        TaskCompletionSource<LootLockerGetScoreListResponse> tcs1 = new TaskCompletionSource<LootLockerGetScoreListResponse>();
        
        LootLockerSDKManager.GetScoreList(LEADERBOARD_KEY, 5, 0, async (resp) => {
            if (!resp.success) {
                Debug.Log("Failed");
                return;
            }
            tcs1.SetResult(resp);
        });

        return await tcs1.Task;
    }
    

    private bool DidFail(LootLockerResponse resp, Action onFail) {
        if (!resp.success) {
            onFail.Invoke();
            return true;
        }
        return false;
    }
    
}