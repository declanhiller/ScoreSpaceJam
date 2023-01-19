using System;
using System.Threading.Tasks;
using LootLocker.Requests;
using UnityEngine;

namespace DefaultNamespace {
    public class Leaderboard : MonoBehaviour {
        private void Start() {
            
        }

        public async void GetLeaderboard() {
            Task<LootLockerGetScoreListResponse> getLbTask = LootLockerManager.INSTANCE.GetScoreListAsync();
            // getLbTask.Wait();
            await getLbTask;
            LootLockerGetScoreListResponse resp = getLbTask.Result;
            //actually create leaderboard
        }
        
    }
}