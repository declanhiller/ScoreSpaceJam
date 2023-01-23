using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour {
    
    private LootLockerManager _lootLockerManager;

    [Header("General")]
    [SerializeField] private GameObject startScreenButtons;
    [SerializeField] private GameObject loadingIcon;
    [SerializeField] private GameObject enterNameForm;
    
    [SerializeField] private float errorMessageTime = 10f;

    [Header("Name Form")]
    [SerializeField] private TMP_InputField nameInput;


    public static bool hasRun;

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start() {
        if (!hasRun) {
            Enable(loadingIcon);
            _lootLockerManager = LootLockerManager.INSTANCE;
            _lootLockerManager.OnGuestSessionStart += OnSessionStart;
            _lootLockerManager.OnPlayerNameSet += OnPlayerNameSet;
            _lootLockerManager.StartGuestSession();
            hasRun = true;
        }
    }

    void OnSessionStart(LootLockerGuestSessionResponse resp) {
        Debug.Log("Player name is " + _lootLockerManager.playerName);
        if (String.IsNullOrWhiteSpace(_lootLockerManager.playerName)) {
            Enable(enterNameForm);
            return;
        }

        Enable(startScreenButtons);
    }

    void OnPlayerNameSet(PlayerNameResponse resp) {
        Enable(startScreenButtons);
    }

    public void OnNameFieldSubmitted() {
        string playerName = nameInput.text;
        if (String.IsNullOrWhiteSpace(playerName)) {
            StartCoroutine(ThrowError("Name is Blank"));
            return;
        }
        _lootLockerManager.SetPlayerName(playerName);
    }

    IEnumerator ThrowError(string error) {
        float timer = errorMessageTime;
        while (timer > 0) {
            timer -= Time.deltaTime;
            //mouse click remove error message
            yield return new WaitForEndOfFrame();
        }
        //remove error messages
    }

    private void Enable(GameObject objectToEnable) {
        objectToEnable.SetActive(true);
        if (startScreenButtons != objectToEnable) {
            startScreenButtons.SetActive(false);
        }

        if (loadingIcon != objectToEnable) {
            loadingIcon.SetActive(false);
        }

        if (enterNameForm != objectToEnable) {
            enterNameForm.SetActive(false);
        }
    }

    private void OnDestroy() {
        _lootLockerManager.OnGuestSessionStart -= OnSessionStart;
        _lootLockerManager.OnPlayerNameSet -= OnPlayerNameSet;
    }
}
