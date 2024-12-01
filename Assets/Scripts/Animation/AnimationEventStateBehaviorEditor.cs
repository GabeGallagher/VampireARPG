using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

#nullable enable

[CustomEditor(typeof(AnimationEventStateBehavior))]
public class AnimationEventStateBehaviorEditor : Editor
{
    private AnimationClip? previewClip;
    private float previewTime;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationEventStateBehavior stateBehavior = (AnimationEventStateBehavior)target;
        if (isValid(stateBehavior, out string errorMessage))
        {
            GUILayout.Space(10);
            PreviewAnimationClip(stateBehavior);
            GUILayout.Label($"Previewing at {previewTime:F2}s", EditorStyles.helpBox);
        }
        else
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Info);
        }
    }

    private void PreviewAnimationClip(AnimationEventStateBehavior stateBehavior)
    {
        if (previewClip == null) return;

        previewTime = stateBehavior.fireTime * previewClip.length;

        AnimationMode.StartAnimationMode();
        AnimationMode.SampleAnimationClip(Selection.activeGameObject, previewClip, previewTime);
        AnimationMode.StopAnimationMode();
    }

    private bool isValid(AnimationEventStateBehavior stateBehavior, out string errorMessage)
    {
        AnimatorController? animatorController = GetValidAnimationController(out errorMessage);
        if (animatorController == null) return false;

        ChildAnimatorState matchingState = animatorController.layers
            .SelectMany(layer => layer.stateMachine.states)
            .FirstOrDefault(state => state.state.behaviours.Contains(stateBehavior));

        previewClip = matchingState.state?.motion as AnimationClip;
        if (previewClip == null)
        {
            errorMessage = "No valid AnimationClip found for the current state";
            return false;
        }
        return true;
    }

    private AnimatorController? GetValidAnimationController(out string errorMessage)
    {
        errorMessage = string.Empty;

        GameObject? targetGameObject = Selection.activeGameObject;
        if (targetGameObject == null )
        {
            errorMessage = "Please select a GameObject with an Animator to preview";
            return null;
        }

        Animator? animator = targetGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            errorMessage = "The selected GameObject does not have an Animator component";
            return null;
        }

        AnimatorController? animationController = animator.runtimeAnimatorController as AnimatorController;
        if (animationController == null)
        {
            errorMessage = "The selected Animator does not have a valid AnimatorController";
            return null;
        }

        return animationController;
    }
}
