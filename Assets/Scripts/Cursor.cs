using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCursor : MonoBehaviour
{
    public RenderMatrixMap renderMatrixMap;
    public GameObject currentBuilding;
    public LayerMask groundLayer;
    public Material validPlacementMaterial;
    public Material invalidPlacementMaterial;

    private bool isPlacing = false;
    private BuildingController buildingController;
    private Vector3 lastMousePosition;

    void Update()
    {
        if (isPlacing)
        {
            UpdateMousePosition();
            MoveBuildingWithMouse();
            CheckPlacementValidity();
            HandlePlacementInput();
        }
    }

    void UpdateMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // 只更新位置，实际移动在MoveBuildingWithMouse中处理
            lastMousePosition = hit.point;
        }
    }

    void MoveBuildingWithMouse()
    {
        if (currentBuilding == null || renderMatrixMap == null) return;
        
        // 获取最近的网格点位置
        Vector3 gridPosition = renderMatrixMap.GetNearestGridPoint(lastMousePosition);
        
        // 应用网格对齐的位置
        currentBuilding.transform.position = gridPosition;
    }

    void CheckPlacementValidity()
    {
        if (renderMatrixMap == null || currentBuilding == null || buildingController == null) return;
        
        // 获取建筑尺寸
        Vector2Int buildingSize = buildingController.GetBuildingSize();
        
        // 显示放置预览
        renderMatrixMap.ShowPlacementPreview(currentBuilding.transform.position, buildingSize);
        
        // 检查放置位置是否有效（网格占用）
        bool gridValid = renderMatrixMap.IsPositionValid(currentBuilding.transform.position, buildingSize);
        
        // 新增：检查地图范围边界
        Vector3 buildingPos = currentBuilding.transform.position;
        bool inMapRange = IsBuildingInMapRange(buildingPos, buildingSize);
        
        // 合并两个条件
        bool isValid = gridValid && inMapRange;
        
        // 更新建筑材质显示状态
        Renderer buildingRenderer = currentBuilding.GetComponent<Renderer>();
        if (buildingRenderer != null)
        {
            buildingRenderer.material = isValid ? validPlacementMaterial : invalidPlacementMaterial;
        }
        
        // 存储当前有效性状态
        currentPlacementValid = isValid;
    }

    // 新增方法：检查建筑是否完全在地图范围内
    private bool IsBuildingInMapRange(Vector3 position, Vector2Int buildingSize)
    {
        // 计算建筑左下角和右上角的世界坐标
        Vector3 bottomLeft = position;
        Vector3 topRight = position + new Vector3(
            buildingSize.x * renderMatrixMap.cellSize.x,
            0,
            buildingSize.y * renderMatrixMap.cellSize.y
        );
        
        // 检查是否在X和Z范围内
        return bottomLeft.x >= renderMatrixMap.mapXRange.x &&
            topRight.x <= renderMatrixMap.mapXRange.y &&
            bottomLeft.z >= renderMatrixMap.mapZRange.x &&
            topRight.z <= renderMatrixMap.mapZRange.y;
    }

    void HandlePlacementInput()
    {
        if (!isPlacing) return;
        
        // 左键放置 - 使用存储的有效性状态
        if (Input.GetMouseButtonDown(0))
        {
            if (currentPlacementValid) // 确保位置有效
            {
                FinalizePlacement();
            }
        }
        
        // 右键取消
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPlacement();
        }
    }
    // 添加成员变量存储当前有效性
    private bool currentPlacementValid; 

    public void StartPlacement(GameObject building)
    {
        if (isPlacing) CancelPlacement();
        
        currentBuilding = Instantiate(building, Vector3.zero, Quaternion.identity);
        buildingController = currentBuilding.GetComponent<BuildingController>();
        
        if (buildingController == null)
        {
            Debug.LogError("Building is missing BuildingController component!");
            Destroy(currentBuilding);
            return;
        }
        
        isPlacing = true;
        
        // 初始位置设为鼠标位置（对齐到网格）
        UpdateMousePosition();
        MoveBuildingWithMouse();
        
        // 启用建筑
        currentBuilding.SetActive(true);
    }

    void FinalizePlacement()
    {
        // 标记为已放置
        buildingController.SetPlaced(true);
        
        // 更新区域地图
        if (renderMatrixMap != null && buildingController != null)
        {
            Vector2 buildingSize = buildingController.GetBuildingSize();
            renderMatrixMap.OccupyCells(currentBuilding.transform.position, buildingSize);
            renderMatrixMap.UpdateMap();
        }
        
        // 清理光标状态
        currentBuilding = null;
        buildingController = null;
        isPlacing = false;
    }

    void CancelPlacement()
    {
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
        }
        currentBuilding = null;
        buildingController = null;
        isPlacing = false;
        
        // 清理预览
        if (renderMatrixMap != null)
        {
            renderMatrixMap.UpdateMap();
        }
    }
}
// public class Cursor : MonoBehaviour
// {
//     public Vector2 mouseMapPosition;
//     public RenderMatrixMap RenderMatrixMap;
//     public GameObject createBuilding;

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // moving cursor
//         Ray raycast = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
//         RaycastHit hit;
//         if (Physics.Raycast(raycast, out hit))
//         {
//             // Debug.Log(hit.point.x);
//             mouseMapPosition.x = hit.point.x;
//             mouseMapPosition.y = hit.point.z;
//             //if cursor not empty
//             if (createBuilding != null) {
//                 Vector2 position2D = RenderMatrixMap.getPositionMatrix(mouseMapPosition.x, mouseMapPosition.y);
//                 createBuilding.transform.position = new Vector3(position2D.x, 0, position2D.y);
//                 // click mouse 1
//                 if (Input.GetMouseButton(0)) {
//                     if (RenderMatrixMap.CreateBuilding)
//                     {
//                         for (int i=0; i<RenderMatrixMap.radiusObjects.Length; i++)
//                         {
//                             // RenderMatrixMap.points[RenderMatrixMap.radiusObjects[i].GetComponent<BuildingInfo>().Index].create = false;
//                             Destroy(RenderMatrixMap.radiusObjects[i], 0);
//                         }
//                         createBuilding = null;
//                     }
//                 }
//             }
//         }
//     }
// }
