using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject mouseTrackerVisual;

    private InputActions inputActions;

    private Vector3 mousePosition;

    private bool isHolding = false;

    public event EventHandler OnMove, OnEnemyClicked;

    public Vector3 MousePosition { get => mousePosition; }

    private void Start()
    {
        inputActions = new InputActions();

        inputActions.PlayerMovement.Move.performed += Move_performed;

        inputActions.PlayerMovement.Move.started += context => OnMouseDown();

        inputActions.PlayerMovement.Move.canceled += context => OnMouseUp();

        inputActions.PlayerMovement.Enable();
    }

    private void OnMouseDown()
    {
        isHolding = true;
    }

    private void OnMouseUp()
    {
        isHolding = false;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        GameObject mouseClickAnimation = Instantiate(mouseTrackerVisual, transform);

        mouseClickAnimation.transform.position = mousePosition;

        OnMove?.Invoke(this, EventArgs.Empty);
    }

    private void CheckForMovementHold()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            mousePosition = hitInfo.point;

            if (isHolding)
            {
                OnMove?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void CheckForEnemyClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out EnemyController enemy))
            {
                mousePosition = enemy.transform.position;

                OnEnemyClicked?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void Update()
    {
        CheckForMovementHold();

        if (Input.GetMouseButtonDown(0))
        {
            CheckForEnemyClick();
        }
    }
}
