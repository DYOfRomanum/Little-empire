using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private RenderMatrixMap renderMatrixMap;
    private Vector2Int buildingSize;
    private bool isPlaced = false;

    public void Initialize(RenderMatrixMap map)
    {
        renderMatrixMap = map;
        
        // 计算建筑占用的网格单元数量
        buildingSize = new Vector2Int(
            Mathf.RoundToInt(transform.localScale.x),
            Mathf.RoundToInt(transform.localScale.z)
        );
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
        
        // 放置后锁定位置
        if (placed)
        {
            // 确保位置对齐到网格
            if (renderMatrixMap != null)
            {
                transform.position = renderMatrixMap.GetNearestGridPoint(transform.position);
            }
            
            // 禁用移动
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    public Vector2Int GetBuildingSize()
    {
        return buildingSize;
    }
}