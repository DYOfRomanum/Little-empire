using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Rotate_loginlogo : MonoBehaviour

{
    [Tooltip("旋转速度 (度/秒)")]
    public float maxAngle = 20f;

    void Update()
    {
        // get current phase
        float cosPhase = 2 * (float)PI * Time.time / 8;
        transform.Rotate(0, maxAngle * (float)Cos(cosPhase), 0);
    }
}