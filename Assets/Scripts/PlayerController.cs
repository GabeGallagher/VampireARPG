using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mouseTracker;

    private InputController inputController;

    private NavMeshAgent navMeshAgent;

    

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
        Vector3 moveToPosition = inputController.MousePosition;

        navMeshAgent.destination = moveToPosition;
    }
}
