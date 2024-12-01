using UnityEngine;

#nullable enable

public class AnimationEventStateBehavior : StateMachineBehaviour
{
    public string eventName;
    [Range(0f, 1f)] public float fireTime;

    private bool hasFired;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasFired = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float currentTime = stateInfo.normalizedTime % 1f;

        if (!hasFired && currentTime >= fireTime)
        {
            NotifyReceiver(animator);
            hasFired = true;
        }
    }

    void NotifyReceiver(Animator animator)
    {
        AnimationEventReceiver receiver = animator.GetComponent<AnimationEventReceiver>();
        if (receiver != null)
        {
            receiver.OnAnimationEventTriggered(eventName);
        }
    }
}
