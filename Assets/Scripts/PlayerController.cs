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

    private List<GameObject> inventory;

    private NavMeshAgent navMeshAgent;

    private Vector3 moveToPosition;

    private bool isRunning = false;

    public BasicAttackSO Attack { get => attack; }

    public List<GameObject> Inventory { get => inventory; }

    public int PickupRange { get => pickupRange; }

    public bool IsRunning { get => isRunning; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        inventory = new List<GameObject>();
    }

    private void Start()
    {
        inputController.OnMove += InputController_OnMove;
        inputController.OnEnemyClicked += InputController_OnEnemyClicked;
    }

    private void Update()
    {
        if (!inputController.CanMove)
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

    private void InputController_OnEnemyClicked(object sender, OnEnemyClickedEventArgs e)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, e.TargetTransform.position);

        if (distanceToEnemy <= (float)attack.Range)
        {
            List<Transform> targetTransformList = new List<Transform>();

            targetTransformList.Add(e.TargetTransform);

            DealDamage(attack.Damage, targetTransformList);
        }
    }

    private void InputController_OnMove(object sender, System.EventArgs e)
    {
        moveToPosition = inputController.MousePosition;

        navMeshAgent.destination = moveToPosition;
    }

    public void DealDamage(int damage, List<Transform> targetList)
    {
        foreach (Transform target in targetList)
        {
            target.GetComponent<EnemyController>().DamageReceived(damage, gameObject);
        }
    }

    private bool SetIsRunning()
    {
        return !(moveToPosition.x == transform.position.x && moveToPosition.z == transform.position.z);
    }

    // sets move to position to current transform position. Quick and dirty way to stop player run animatio after certain actions
    public void StopMoving()
    {
        moveToPosition = transform.position;
    }
}
