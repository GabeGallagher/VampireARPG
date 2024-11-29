using UnityEngine;

public abstract class AnimationState
{
    protected AnimationController animationController;
    protected Animator animator;
    protected PlayerController player;

    public enum EAnimationState
    {
        Idle,
        Running,
        Attacking
    }

    public AnimationState(AnimationController animationController, PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
        this.animationController = animationController;

        if (animator == null)
        {
            Debug.LogError($"Animator component not found on {player.name}");
        }
    }

    public virtual EAnimationState AnimationStateName => EAnimationState.Idle;
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
