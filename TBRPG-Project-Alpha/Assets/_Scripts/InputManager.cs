using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existe más de un InputManager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }

    /// <summary>
    /// Gets the mouse button down.
    /// </summary>
    /// <returns>A bool.</returns>
    public bool GetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// Gets the right mouse button.
    /// </summary>
    /// <returns>A bool.</returns>
    public bool GetRightMouseButton()
    {
        return Input.GetMouseButtonDown(1);
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 moveVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveVector.y -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVector.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVector.x += 1;
        }

        return moveVector;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount -= 1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount += 1;
        }

        return rotateAmount;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount -= 1;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount += 1;
        }

        return zoomAmount;
    }

    
}
