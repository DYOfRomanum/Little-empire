using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_login : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
// using UnityEngine;

// public class RotateOnZAxis : MonoBehaviour
{
    [Tooltip("旋转速度 (度/秒)")]
    public float rotationSpeed = 5f;

    void Update()
    {
        // 每帧绕Z轴旋转（世界坐标系）
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}