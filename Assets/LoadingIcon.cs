using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour {
    
    [SerializeField] private float rotatationSpeed = 10f;
    private RectTransform rectTransform;
    
    private void Awake() {
        rectTransform = (RectTransform) gameObject.transform;
    }

    private void Update() {
        rectTransform.Rotate(0, 0, rotatationSpeed * Time.deltaTime);
    }
}
