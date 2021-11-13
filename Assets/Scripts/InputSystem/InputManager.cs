using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Profiling;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class InputManager : NetworkBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance{
        get{
            return _instance;
        }
    }
    PlayerControls controls;

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

    public Vector3 GetPlayerMovement(){
        var vertical = controls.Player.VerticalMovement.ReadValue<float>();
        var horizontal = controls.Player.HorizontalMovement.ReadValue<Vector2>();
        return new Vector3(horizontal.x, vertical, horizontal.y).normalized;
    }

    public Vector2 GetMouseDelta() {
        return controls.Player.CameraMovement.ReadValue<Vector2>();
    }

    public bool GetRollMod() {
        return controls.Player.CameraRollMod.ReadValue<float>() == 1f;
    }
}
