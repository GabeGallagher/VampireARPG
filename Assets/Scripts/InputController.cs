using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject mouseTrackerVisual;

    private InputActions inputActions;

    public event EventHandler OnMove;

    private Vector3 mousePosition;

    private void Start()
    {
        inputActions = new InputActions();

        inputActions.PlayerMovement.Move.performed += Move_performed;

        inputActions.PlayerMovement.Enable();
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        GameObject mouseClickAnimation = Instantiate(mouseTrackerVisual, transform);

        mouseClickAnimation.transform.position = mousePosition;

        OnMove?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            mousePosition = hitInfo.point;
        }
    }
}
