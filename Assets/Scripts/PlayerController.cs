using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        inputController.OnMove += InputController_OnMove;
        inputController.OnEnemyClicked += InputController_OnEnemyClicked;
    }

    private void InputController_OnEnemyClicked(object sender, System.EventArgs e)
    {
        Debug.Log("Enemy Clicked");
    }

    private void InputController_OnMove(object sender, System.EventArgs e)
    {
        Vector3 moveToPosition = inputController.MousePosition;

        navMeshAgent.destination = moveToPosition;
    }
}
