using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickVisualDestroyer : MonoBehaviour
{
    [SerializeField] private float timeToDestoy = 0.5f;

    private void Update()
    {
        timeToDestoy -= Time.deltaTime;

        if (timeToDestoy < 0)
        {
            Destroy(gameObject);
        }
    }
}
