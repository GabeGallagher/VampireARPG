using UnityEngine;

public class RunningState : AnimationState
{
    public RunningState(AnimationController animationController, PlayerController player, Animator animator) : base(animationController, player, animator)
    {
    }

    public override EAnimationState AnimationStateName => EAnimationState.Running;

    public override void EnterState()
    {
        animator.SetBool("isRunning", true);
    }

    public override void ExitState()
    {
        animator.SetBool("isRunning", false);
    }

    public override void UpdateState()
    {
        if (player.IsIdle)
        {
            animationController.SwitchState(new IdleState(animationController, player, animator));
        }
    }
}
