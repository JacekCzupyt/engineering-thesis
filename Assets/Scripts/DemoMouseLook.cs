using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMouseLook : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 5f;
    private float xRoatation = 0f;
    public Transform playerBody;
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRoatation -= mouseY;
        xRoatation = Mathf.Clamp(xRoatation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRoatation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
