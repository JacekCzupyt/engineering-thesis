using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : NetworkBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{
        get{
            return _instance;
        }
    }
    PlayerControls controls;

    Vector2 horizontalInput;

    private void Awake() {
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
        controls = new PlayerControls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    public Vector2 GetPlayerHorizontalMovement(){
        return controls.Player.HorizontalMovement.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerVerticalMovement(){
        return controls.Player.VerticalMovement.ReadValue<Vector2>();
    }
}
