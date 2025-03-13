using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private AnimationState currentState;
    private PlayerController player;

    private string triggerAttack = "Attack";
    private string triggerMoving = "IsMoving";

    private void Awake()
    {
        try
        {
            player = transform.parent.GetComponent<PlayerController>();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Player controller not found on parent of {gameObject.name}: {e.Message}");
        }

        try
        {
            animator = GetComponent<Animator>();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"Animator not found on {gameObject.name}: {e.Message}");
        }
    }

    private void Start()
    {
        SwitchState(new IdleState(this, player, animator));
        player.OnPlayerStateChange += Player_OnPlayerStateChange;
    }

    private void Player_OnPlayerStateChange(PlayerController.EPlayerState obj)
    {
        AnimationState? state = currentState;

        switch (obj)
        {
            case PlayerController.EPlayerState.Idle:
                state = new IdleState(this, player, animator); break;

            case PlayerController.EPlayerState.Running:
                state = new RunningState(this, player, animator); break;

            case PlayerController.EPlayerState.Attacking:
                state = new AttackingState(this, player, animator); break;
        }

        if (state != null) SwitchState(state);
    }

    private void Update()
    {
        if (!isPositionRooted())
        {
            transform.position = player.transform.position;
        }

        if (!isRotationRooted())
        {
            transform.rotation = player.transform.rotation;
        }
    }

    public void SwitchState(AnimationState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    private bool isPositionRooted()
    {
        return gameObject.transform.position == Vector3.zero;
    }

    private bool isRotationRooted()
    {
        return gameObject.transform.rotation == Quaternion.identity;
    }

    public void TriggerAttack()
    {
        WeaponData weaponData = (WeaponData)player.InventoryController.MainHand;
        animator.SetBool("IsTwoHanded", weaponData.IsTwoHanded);
        animator.SetTrigger(triggerAttack);
    }

    private void SpawnDamageCollider()
    {
        player.SpawnDamageCollider();
    }
}