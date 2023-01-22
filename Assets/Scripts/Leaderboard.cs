using System;
using System.Threading.Tasks;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class Leaderboard : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI[] playerNames;
        [SerializeField] private TextMeshProUGUI[] playerScores;

        [SerializeField] private TextMeshProUGUI internalPlayerRank;
        [SerializeField] private TextMeshProUGUI internalPlayerName;
        [SerializeField] private TextMeshProUGUI internalPlayerScore;

        private LootLockerManager lootLockerManager;
        
        private void Start() {
            //Where I would set the leaderboard as loading without any players

            lootLockerManager = LootLockerManager.INSTANCE;

            internalPlayerName.text = lootLockerManager.playerName;

            //Get the leaderboard and actually create
            lootLockerManager.SubmitScore(lootLockerManager.playerScore, (resp) => {
                internalPlayerRank.text = "" + resp.rank;
                internalPlayerScore.text = "" + resp.score;
            });

            GetLeaderboard();
        }

        public async void GetLeaderboard() {
            Task<LootLockerGetScoreListResponse> getLbTask = lootLockerManager.GetScoreListAsync();
            await getLbTask;
            LootLockerGetScoreListResponse resp = getLbTask.Result;
            CreateLeaderboard(resp);
        }

        private void CreateLeaderboard(LootLockerGetScoreListResponse resp)
        {

            Debug.Log(resp.items.Length);

            for (int i = 0; i < resp.items.Length; i++) {
                LootLockerLeaderboardMember member = resp.items[i];
                Debug.Log(member.rank);
                playerNames[i].text = member.player.name;
                playerScores[i].text = "" + member.score;

            }
            
            Debug.Log("Instantiate Leaderboard Spots");
        }
    }
}