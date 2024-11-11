using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] private InputController inputController;

    [SerializeField] private BasicAttackSO attack;

    [SerializeField] private int pickupRange = 4;

    [SerializeField] InventoryController inventoryController;

    private NavMeshAgent navMeshAgent;

    private Vector3 moveToPosition;

    private int experience = 0;

    private int levelUpExperience = 10;

    private int level = 1;

    private bool isRunning = false;

    private bool isAttacking = false;

    private bool canMove = true;

    public BasicAttackSO Attack { get => attack; }

    public int PickupRange { get => pickupRange; }

    public bool IsRunning { get => isRunning; }

    public bool IsAttacking { get => isAttacking; }

    public InventoryController InventoryController { get => inventoryController; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        inputController.OnMove += InputController_OnMove;
        inputController.OnEnemyClicked += InputController_OnDamageableClicked;
        inputController.OnOpenInventory += InputController_OnOpenInventory;
        inputController.OnHarvestableClicked += InputController_OnDamageableClicked;
    }

    private void InputController_OnOpenInventory(object sender, EventArgs e)
    {
        inventoryController.gameObject.SetActive(!inventoryController.gameObject.activeSelf);
    }

    private void Update()
    {
        if (!inputController.CanMove || !canMove)
        {
            navMeshAgent.isStopped = true;

            StopMoving();
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
        isRunning = SetIsRunning();
    }

    private void InputController_OnDamageableClicked(object sender, OnDamageableClickedEventArgs e)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, e.TargetTransform.position);

        if (distanceToEnemy <= (float)attack.Range)
        {
            List<Transform> targetTransformList = new List<Transform>();

            targetTransformList.Add(e.TargetTransform);

            StartCoroutine(AttackRoutine(1f, targetTransformList));
        }
    }

    private IEnumerator AttackRoutine(float attackTime, List<Transform> targetTransformList)
    {
        isAttacking = true;

        canMove = false;

        yield return new WaitForSeconds(attackTime);

        DealDamage(attack.Damage, targetTransformList);

        isAttacking = false;

        canMove = true;
    }

    private void InputController_OnMove(object sender, System.EventArgs e)
    {
        if (inputController.CanMove && canMove)
        {
            moveToPosition = inputController.MousePosition;

            navMeshAgent.destination = moveToPosition;
        }
        else
        {
            StopMoving();
        }
    }

    public void DealDamage(int damage, List<Transform> targetList)
    {
        foreach (Transform target in targetList)
        {
            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().DamageReceived(damage, gameObject);
            }
        }
    }

    public void TargetKilled(GameObject target)
    {
        if (target.GetComponent<EnemyController>())
        {
            experience += 10;

            if (experience > levelUpExperience)
            {
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        Debug.Log("Leveled Up!");
    }

    private bool SetIsRunning()
    {
        if (!isAttacking)
        {
            return !(moveToPosition.x == transform.position.x && moveToPosition.z == transform.position.z); 
        }
        return false;
    }

    // sets move to position to current transform position. Quick and dirty way to stop player run animation after certain actions
    public void StopMoving()
    {
        moveToPosition = transform.position;
    }

    public void PickUp(Item item)
    {
        inventoryController.AddItem(item.ItemSO);

        Debug.Log("Picked up " + item.ItemSO.ItemName);

        Destroy(item.gameObject);
    }
}
