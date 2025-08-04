using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("移动速度 (米/秒)")]
    public float moveSpeed = 5.0f;
    
    [Tooltip("平滑移动因子")]
    [Range(0, 1)] public float smoothFactor = 0.1f;

    [Header("高度控制")]
    [Tooltip("保持的初始高度")]
    public float fixedHeight = 1.7f;
    
    [Tooltip("地面检测距离")]
    public float groundCheckDistance = 0.2f;
    
    [Tooltip("地面层级")]
    public LayerMask groundLayer;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("未找到主摄像机！");
            enabled = false;
            return;
        }
        
        // 初始化目标位置为当前位置
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        MaintainHeight();
        ApplyMovement();
    }

    void HandleInput()
    {
        // 获取输入轴
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // 没有输入时直接返回
        if (horizontal == 0 && vertical == 0) return;
        
        // 获取摄像机在水平面的前向和右向（忽略Y轴）
        Vector3 cameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up).normalized;
        
        // 计算移动方向
        Vector3 moveDirection = (vertical * cameraForward) + (horizontal * cameraRight);
        moveDirection.y = 0; // 确保没有垂直分量
        
        // 防止斜向移动速度更快
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        // 更新目标位置
        targetPosition += moveDirection * moveSpeed * Time.deltaTime;
    }

    void MaintainHeight()
    {
        // 检测地面高度
        if (Physics.Raycast(targetPosition + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 
                            10f, groundLayer))
        {
            // 保持固定高度
            targetPosition.y = hit.point.y + fixedHeight;
        }
    }

    void ApplyMovement()
    {
        // 使用平滑阻尼移动
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothFactor
        );
    }

    // 可视化调试
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        Gizmos.DrawLine(transform.position, targetPosition);
        
        // 绘制方向指示器
        Gizmos.color = Color.blue;
        Vector3 camForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Gizmos.DrawRay(transform.position, camForward * 2f);
        
        Gizmos.color = Color.red;
        Vector3 camRight = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
        Gizmos.DrawRay(transform.position, camRight * 2f);
    }
}
