using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mouseTracker;

    private InputController inputController;

    private NavMeshAgent navMeshAgent;

    private Vector3 moveToPosition;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        inputController = mouseTracker.GetComponent<InputController>();
    }

    private void Start()
    {
        inputController.OnMove += InputController_OnMove;
    }

    private void InputController_OnMove(object sender, System.EventArgs e)
    {
        Transform lastChildInMouseTracker = mouseTracker.transform.GetChild(mouseTracker.transform.childCount - 1);

        moveToPosition = lastChildInMouseTracker.position;
    }

    private void Update()
    {
        if (moveToPosition != null)
        { 
            navMeshAgent.destination = moveToPosition;
        }
    }
}
