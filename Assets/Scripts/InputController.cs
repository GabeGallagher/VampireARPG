using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public event EventHandler OnMove;
    public event EventHandler<OnDamageableClickedEventArgs> OnEnemyClicked;
    public event EventHandler OnOpenInventory;
    public event EventHandler OnEquipPerformed;
    public event EventHandler<OnDamageableClickedEventArgs> OnHarvestableClicked;
    public event EventHandler OnToggleBuildMode;
    public event EventHandler OnOpenSkillTab;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject mouseTrackerVisual;
    [SerializeField] private PlayerController player;
    [SerializeField] private SkillSlotController leftClickSkill, rightClickSkill;

    private InputActions inputActions;
    private Vector3 mousePosition;
    private bool isHolding = false;
    private bool canMove = true;

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

        inputActions.UI.EquipFromInventory.performed += EquipFromInventory_performed;

        inputActions.UI.OpenSkillTab.performed += OpenSkillTab_performed;

        inputActions.UI.Enable();

        inputActions.PlayerActions.ToggleBuildMode.performed += ToggleBuildMode_performed;

        inputActions.PlayerActions.Enable();
    }

    private void OpenSkillTab_performed(InputAction.CallbackContext obj)
    {
        OnOpenSkillTab?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleBuildMode_performed(InputAction.CallbackContext obj)
    {
        OnToggleBuildMode?.Invoke(this, EventArgs.Empty);
    }

    private void EquipFromInventory_performed(InputAction.CallbackContext obj)
    {
        OnEquipPerformed?.Invoke(this, EventArgs.Empty);
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
        if (!IsPointerOverUIElement()) // prevents movement clicks when click ui
        {
            GameObject mouseClickAnimation = Instantiate(mouseTrackerVisual, transform);

            mouseClickAnimation.transform.position = mousePosition;

            if (canMove)
            {
                OnMove?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void CheckForMovementHold()
    {
        if (!IsPointerOverUIElement())
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
    }
    // TODO: Update this method to properly cast spell or throw missile
    private void CheckForAttackHold()
    {
        if (!IsPointerOverUIElement())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                mousePosition = hitInfo.point;

                if (isHolding && canMove)
                {
                    Debug.Log("Attacking with ranged missile or spell");
                }
            }
        }
    }

    private void CheckForEnemyClick(SkillSO skill)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out EnemyController enemy))
            {
                float playerDistanceToEnemy = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (player.GetAttackingRange() < playerDistanceToEnemy)
                {
                    mousePosition = enemy.transform.position;
                }
                else
                {
                    canMove = false;
                }
                OnEnemyClicked?.Invoke(this, new OnDamageableClickedEventArgs(enemy.transform, skill));
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
                float playerDistanceToItem = Vector3.Distance(player.transform.position, item.transform.position);

                if (player.PickupRange < playerDistanceToItem)
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

    private void CheckForHarvestableClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.TryGetComponent(out Harvestable harvestable))
            {
                float playerDistanceToHarvestable = Vector3.Distance(player.transform.position, harvestable.transform.position);
                if (player.PickupRange < playerDistanceToHarvestable)
                {
                    mousePosition = harvestable.transform.position;
                }
                else
                {
                    canMove = false;
                }
                OnHarvestableClicked?.Invoke(this, new OnDamageableClickedEventArgs(harvestable.transform, player.BasicAttackSkill));
            }
        }
    }

    public bool IsPointerOverUIElement()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void Update()
    {
        CheckForMovementHold();

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            CheckForEnemyClick(leftClickSkill.skill);

            CheckForItemClick();

            CheckForHarvestableClick();
        }
        // Get right click but prioritize left click if both are held
        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            SkillSO.ESkillType skillType = rightClickSkill.skill.SkillType;
            bool isRanged = skillType == SkillSO.ESkillType.Ranged || skillType == SkillSO.ESkillType.Magical;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out RaycastHit hitInfo);
            if (hitInfo.transform.TryGetComponent(out Harvestable harvestable))
            {
                CheckForHarvestableClick();
            }
            else if (skillType == SkillSO.ESkillType.Ranged || skillType == SkillSO.ESkillType.Magical)
            {
                canMove = false; // Will need to be set to true in animation
                Debug.Log($"Casting {rightClickSkill.skill.SkillType}");
            }
            else if (skillType == SkillSO.ESkillType.Physical)
            {
                CheckForEnemyClick(rightClickSkill.skill);
            }
            else
            {
                GameObject mouseClickAnimation = Instantiate(mouseTrackerVisual, transform);
                mouseClickAnimation.transform.position = mousePosition;
                if (canMove)
                {
                    OnMove?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}

public class OnDamageableClickedEventArgs : EventArgs
{
    public Transform TargetTransform { get; }
    public SkillSO Skill { get; }

    public OnDamageableClickedEventArgs(Transform targetTransform, SkillSO skill)
    {
        TargetTransform = targetTransform;
        Skill = skill;
    }
}