using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private AnimationState currentState;
    private PlayerController player;
    private Dictionary<string, RuntimeAnimatorController> weaponAnimators;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = transform.parent.GetComponent<PlayerController>();
    }

    private void Start()
    {
        SwitchState(new IdleState(this, player, animator));
        player.OnPlayerStateChange += Player_OnPlayerStateChange;
        InitializeWeaponAnimators();
    }

    private void Player_OnPlayerStateChange(PlayerController.EPlayerState obj)
    {
        AnimationState? state = currentState;

        switch(obj)
        {
            case PlayerController.EPlayerState.Idle:
                state = new IdleState(this, player, animator); break;

            case PlayerController.EPlayerState.Running:
                state = new RunningState(this, player, animator); break;
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

    private void InitializeWeaponAnimators()
    {
        weaponAnimators = new Dictionary<string, RuntimeAnimatorController>
        {
            //{ "OneHanded", Resources.Load<RuntimeAnimatorController>("Animators/OneHandedAnimator") },
        };
    }
}
