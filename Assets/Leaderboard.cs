using System;
using System.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;

namespace DefaultNamespace {
    public class Leaderboard : MonoBehaviour
    {
        private GameObject leaderboard;
        
        private void Start() {
            //Where I would set the leaderboard as loading without any players
            
            //Get the leaderboard and actually create
            GetLeaderboard();
        }

        public async void GetLeaderboard() {
            Task<LootLockerGetScoreListResponse> getLbTask = LootLockerManager.INSTANCE.GetScoreListAsync();
            await getLbTask;
            LootLockerGetScoreListResponse resp = getLbTask.Result;
            CreateLeaderboard(resp);
        }

        private void CreateLeaderboard(LootLockerGetScoreListResponse resp)
        {
            Debug.Log("Instantiate Leaderboard Spots");
        }
    }
}