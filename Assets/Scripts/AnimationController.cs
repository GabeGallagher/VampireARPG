using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private PlayerController player;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        player = transform.parent.GetComponent<PlayerController>();
    }

    private void Update()
    {
        animator.SetBool("isRunning", player.IsRunning);

        if (!isPositionRooted())
        {
            transform.position = player.transform.position;
        }

        if (!isRotationRooted())
        {
            transform.rotation = player.transform.rotation;
        }
    }

    private bool isPositionRooted()
    {
        return gameObject.transform.position == Vector3.zero;
    }

    private bool isRotationRooted()
    {
        return gameObject.transform.rotation == Quaternion.identity;
    }
}
