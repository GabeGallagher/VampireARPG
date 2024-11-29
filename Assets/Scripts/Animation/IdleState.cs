using UnityEngine;

public class IdleState : AnimationState
{
    public IdleState(AnimationController animationController, PlayerController player, Animator animator) : base(animationController, player, animator)
    {
    }

    public override void EnterState()
    {
        animator.SetBool("isIdle", true);
    }

    public override void ExitState()
    {
        animator.SetBool("isIdle", false);
    }

    public override void UpdateState()
    {
        if (player.IsRunning)
        {
            animationController.SwitchState(new RunningState(animationController, player, animator));
        }
    }
}
