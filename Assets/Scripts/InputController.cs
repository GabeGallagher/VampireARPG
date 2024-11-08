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

    [SerializeField] private PlayerController player;

    private InputActions inputActions;

    private Vector3 mousePosition;

    private bool isHolding = false;

    private bool canMove = true;

    public event EventHandler OnMove;

    public event EventHandler<OnEnemyClickedEventArgs> OnEnemyClicked;

    public event EventHandler OnOpenInventory;

    public Vector3 MousePosition { get => mousePosition; }

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Start()
    {
        inputActions = new InputActions();

        inputActions.PlayerMovement.Move.performed += Move_performed;

        inputActions.PlayerMovement.Move.started += context => OnMouseDown();

        inputActions.PlayerMovement.Move.canceled += context => OnMouseUp();

        inputActions.PlayerMovement.Enable();

        inputActions.UI.OpenInventory.performed += OpenInventory_performed;

        inputActions.UI.Enable();
    }

    private void OpenInventory_performed(InputAction.CallbackContext obj)
    {
        OnOpenInventory?.Invoke(this, EventArgs.Empty);
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

        if (canMove)
        {
            OnMove?.Invoke(this, EventArgs.Empty); 
        }
    }

    private void CheckForMovementHold()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            mousePosition = hitInfo.point;

            if (isHolding && canMove)
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
                float playerDistanceToEnemy = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (player.Attack.Range < playerDistanceToEnemy)
                {
                    mousePosition = enemy.transform.position;
                }
                else
                {
                    canMove = false;
                }
                OnEnemyClicked?.Invoke(this, new OnEnemyClickedEventArgs(enemy.transform));
            }
            else
            {
                canMove = true;
            }
        }
    }

    private void CheckForItemClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out Item item))
            {
                float playerDistanceToLoot = Vector3.Distance(player.transform.position, item.transform.position);

                if (player.PickupRange < playerDistanceToLoot)
                {
                    mousePosition = item.transform.position;
                }
                else
                {
                    player.PickUp(item);
                }
            }
        }
    }

    private void Update()
    {
        CheckForMovementHold();

        if (Input.GetMouseButtonDown(0))
        {
            CheckForEnemyClick();

            CheckForItemClick();
        }
    }
}

public class OnEnemyClickedEventArgs : EventArgs
{
    public Transform TargetTransform { get; }

    public OnEnemyClickedEventArgs(Transform targetTransform)
    {
        TargetTransform = targetTransform;
    }
}