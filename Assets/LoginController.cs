using System;
using System.Collections;
using System.Collections.Generic;
using LootLocker;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class LoginController : MonoBehaviour {
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private LootLockerManager lootLockerManager;

    private void Awake() {
        emailField.contentType = TMP_InputField.ContentType.EmailAddress;
        passwordField.contentType = TMP_InputField.ContentType.Password;
        // lootLockerManager.OnSessionStart += OnSessionStart;
    }


    // //for support if we want to do full logins
    // public void Login() {
    //     string email = emailField.text;
    //     string password = passwordField.text;
    //     lootLockerManager.Login(email, password, OnLoginResponse);
    // }
    //
    // private void OnLoginResponse(LootLockerWhiteLabelLoginResponse resp) {
    //     if (!resp.success) {
    //         Debug.Log("error while logging in");
    //         return;
    //     }
    //         
    //     Debug.Log("logged in successfully");
    //     lootLockerManager.StartSession();
    //     string token = resp.SessionToken;
    //     Debug.Log("token is " + token);
    // }
    //
    //
    // public void Signup() {
    //     string email = emailField.text;
    //     string password = passwordField.text;
    //     lootLockerManager.Signup(email, password, OnSignupResponse);
    // }
    //
    // private void OnSignupResponse(LootLockerWhiteLabelSignupResponse resp) {
    //     if (!resp.success) {
    //         Debug.Log("error while creating account");
    //         return;
    //     }
    //     Debug.Log("user created successfully");
    //     // lootLockerManager.StartSession();
    // }
    //
    // public void ResetPassword() {
    //     lootLockerManager.ResetPassword("", OnResetPasswordResponse);
    // }
    //
    // private void OnResetPasswordResponse(LootLockerResponse resp) {
    //     if (!resp.success) {
    //         Debug.Log("error requesting password reset");
    //         return;
    //     } 
    //     Debug.Log("password reset sent successfully");
    // }
    //
    // private void OnSessionStart(LootLockerSessionResponse resp) {
    //     if (!resp.success) {
    //         Debug.Log("error starting session");
    //         return;
    //     }
    //         
    //     Debug.Log("Session started");
    // }
    

    
}
