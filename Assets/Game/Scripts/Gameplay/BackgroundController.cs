using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Serializable]
    public struct Boundaries
    {
        public float min, max;
    }

    [SerializeField]
    private List<Transform> backgrounds = new List<Transform>();

    [SerializeField]
    private Boundaries bounds;

    [SerializeField]
    private float speed = 5f;

    private void FixedUpdate()
    {
        foreach (Transform background in backgrounds)
        {
            background.Translate(0, -speed/100, 0);

            if (background.position.y < bounds.min)
            {
                Vector3 currentPos = background.position;
                background.position = new Vector3(currentPos.x, bounds.max, currentPos.z);
            }
        }
    }
}
