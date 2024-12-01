using UnityEngine;

#nullable enable

public class AttackingState : AnimationState
{
    public AttackingState(AnimationController animationController, PlayerController player, Animator animator) : base(animationController, player, animator)
    {
    }

    public override void EnterState()
    {
        animator.SetBool("isAttacking", true);
    }

    public override void ExitState()
    {
        animator.SetBool("isAttacking", false);
    }

    public override void UpdateState()
    {
        if (player.IsAttacking)
        {
            animationController.SwitchState(new AttackingState(animationController, player, animator));
        }
    }
}
