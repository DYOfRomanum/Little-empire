using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMatrixMap : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 mapXRange = new Vector2(-17, 3);
    public Vector2 mapZRange = new Vector2(-17, 3);
    public Vector2 cellSize = new Vector2(1.0f, 1.0f); // 网格单元大小
    public Vector2Int gridSize = new Vector2Int(20, 20); // 网格尺寸
    
    [Header("Visualization")]
    public GameObject gridCellPrefab;
    public Material validMaterial;
    public Material invalidMaterial;
    
    private GridCell[,] gridCells;
    private List<GameObject> activeIndicators = new List<GameObject>();
    private bool isPlacementValid;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        gridCells = new GridCell[gridSize.x, gridSize.y];
        
        // 计算网格原点（左下角）
        Vector3 gridOrigin = new Vector3(mapXRange.x, 0, mapZRange.x);
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // 使用地图范围作为基准点
                Vector3 position = gridOrigin + new Vector3(
                    x * cellSize.x, 
                    0,
                    y * cellSize.y
                );
                
                gridCells[x, y] = new GridCell {
                    position = position,
                    isOccupied = false,
                    gridPosition = new Vector2Int(x, y)
                };
            }
        }
    }


    public Vector3 GetNearestGridPoint(Vector3 worldPosition)
    {
        Vector2Int gridPos = WorldToGridPosition(worldPosition);
        Vector3 gridOrigin = new Vector3(mapXRange.x, 0, mapZRange.x);
        
        return gridOrigin + new Vector3(
            gridPos.x * cellSize.x,
            0,
            gridPos.y * cellSize.y
        );
    }

    public bool IsPositionValid(Vector3 position, Vector2 buildingSize)
    {
        Vector2Int gridPos = WorldToGridPosition(position);
        int width = Mathf.RoundToInt(buildingSize.x);
        int height = Mathf.RoundToInt(buildingSize.y);
        
        // 检查整个建筑区域是否有效
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int cellX = gridPos.x + x;
                int cellY = gridPos.y + y;
                
                // 检查是否在网格范围内（考虑建筑尺寸）
                if (cellX < 0 || cellX >= gridSize.x || 
                    cellY < 0 || cellY >= gridSize.y)
                {
                    return false;
                }
                
                // 检查是否被占用
                if (gridCells[cellX, cellY].isOccupied)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        // 计算相对于地图左下角的偏移
        float offsetX = worldPosition.x - mapXRange.x;
        float offsetZ = worldPosition.z - mapZRange.x;
        
        return new Vector2Int(
            Mathf.RoundToInt(offsetX / cellSize.x),
            Mathf.RoundToInt(offsetZ / cellSize.y)
        );
    }


    // 在ShowPlacementPreview方法中添加范围检查
    public void ShowPlacementPreview(Vector3 position, Vector2 buildingSize)
    {
        ClearIndicators();
        
        Vector3 gridPosition = GetNearestGridPoint(position);
        Vector2Int gridPos = WorldToGridPosition(gridPosition);
        int width = Mathf.RoundToInt(buildingSize.x);
        int height = Mathf.RoundToInt(buildingSize.y);
        
        isPlacementValid = true;
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int checkX = gridPos.x + x;
                int checkY = gridPos.y + y;
                
                bool isValid = true;
                
                // 检查边界
                if (checkX < 0 || checkX >= gridSize.x || 
                    checkY < 0 || checkY >= gridSize.y)
                {
                    isValid = false;
                    isPlacementValid = false;
                }
                // 检查是否被占用
                else if (gridCells[checkX, checkY].isOccupied)
                {
                    isValid = false;
                    isPlacementValid = false;
                }
                
                // 新增：检查地图范围
                Vector3 cellWorldPos = new Vector3(
                    (gridPos.x + x) * cellSize.x + mapXRange.x,
                    0.1f,
                    (gridPos.y + y) * cellSize.y + mapZRange.x
                );
                
                if (cellWorldPos.x < mapXRange.x || cellWorldPos.x > mapXRange.y ||
                    cellWorldPos.z < mapZRange.x || cellWorldPos.z > mapZRange.y)
                {
                    isValid = false;
                    isPlacementValid = false;
                }
                
                // 创建预览指示器
                GameObject indicator = Instantiate(
                    gridCellPrefab, 
                    cellWorldPos, 
                    Quaternion.identity
                );
                
                indicator.GetComponent<Renderer>().material = isValid ? validMaterial : invalidMaterial;
                activeIndicators.Add(indicator);
            }
        }
    }

    public bool GetPlacementValidity()
    {
        return isPlacementValid;
    }

    public void UpdateMap()
    {
        ClearIndicators();
    }

    void ClearIndicators()
    {
        foreach (GameObject indicator in activeIndicators)
        {
            if (indicator != null) Destroy(indicator);
        }
        activeIndicators.Clear();
    }

    public void OccupyCells(Vector3 position, Vector2 buildingSize)
    {
        Vector2Int gridPos = WorldToGridPosition(position);
        int width = Mathf.RoundToInt(buildingSize.x);
        int height = Mathf.RoundToInt(buildingSize.y);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int cellX = gridPos.x + x;
                int cellY = gridPos.y + y;
                
                // 增加范围检查防止越界
                if (cellX >= 0 && cellX < gridSize.x && 
                    cellY >= 0 && cellY < gridSize.y)
                {
                    gridCells[cellX, cellY].isOccupied = true;
                }
            }
        }
    }

    private class GridCell
    {
        public Vector3 position;
        public bool isOccupied;
        public Vector2Int gridPosition;
    }
}
// public class RenderMatrixMap : MonoBehaviour
// {
//     public static Vector2 SizeCube = new Vector2(1.0f, 1.0f);
//     public Vector2 WHnums = new Vector2(20,20);
//     public Vector2[] shemeRadius;
//     // region prefab
//     public GameObject cubeMatrix; 
//     public GameObject[] radiusObjects;
//     // simulated map
//     public List<pointMap> points = new List<pointMap>();

//     public Material noActiveRegion;
//     public Material ActiveRegion;

//     public bool CreateBuilding;

//     // 
//     void Start()
//     {
//         for (int x = 0; x < WHnums.x; x++)
//         {
//             for (int y = 0; y < WHnums.y; y++)
//             {
//                 pointMap p = new pointMap(x * SizeCube.x, y * SizeCube.y, (x * SizeCube.x)+SizeCube.x, (y*SizeCube.y)+SizeCube.y);
//                 points.Add(p);
//             }
//         }
//     }
//     // refresh range object
//     public void refreshMatrixCursor()
//     {
//         radiusObjects = new GameObject[shemeRadius.Length];
//         Debug.Log(shemeRadius.Length);
//         for (int i=0; i<shemeRadius.Length; i++)
//         {
//             radiusObjects[i] = Instantiate(cubeMatrix, new Vector3(0,0,0), Quaternion.identity) as GameObject;
//         }
//     }

//     public Vector2 getPositionMatrix(float x, float y)
//     {
//         var returnVector = new Vector2();
//         for (int i=0; i<points.Count; i++)
//         {
//             var point = points[i];
//             float Ycalc = (point.Y - point.sizeY)/2;
//             float Xcalc = (point.X - point.sizeX)/2;

//             if ((point.X + Xcalc)<x)
//             {
//                 if (((point.X + point.sizeX)+Xcalc)>x)
//                 {
//                     if ((point.Y + Ycalc)<y)
//                     {
//                         if (((point.Y + point.sizeY)+Ycalc)>y)
//                         {
//                             returnVector.x = point.X;
//                             returnVector.y = point.Y;

//                             CreateBuilding = true;
//                             renderMatrixToMap(i);
//                         }
//                     }
//                 }
//             }
//         }
//         return returnVector;
//     }
//     // render border object
//     public void renderMatrixToMap(int index)
//     {
//         int indexEnd = 0;
//         try 
//         {
//             for (int i=0; i<shemeRadius.Length; i++)
//             {
//                 Vector2 sRadius = shemeRadius[i];
//                 indexEnd = index;
//                 //Y
//                 if (sRadius.y<0) {
//                     indexEnd = index+(Mathf.Abs((int)sRadius.y)*(int)WHnums.y);
//                 } else if (sRadius.y>0) {
//                     indexEnd = index-(Mathf.Abs((int)sRadius.y)*(int)WHnums.y);
//                 }
//                 //X
//                 if (sRadius.x<0) {
//                     indexEnd += Mathf.Abs((int)sRadius.x);
//                 } else if (sRadius.x>0) {
//                     indexEnd -= Mathf.Abs((int)sRadius.x);
//                 }
//                 // if empty region
//                 if (points[indexEnd].create) {
//                     radiusObjects[i].transform.GetComponent<Renderer>().material = ActiveRegion;
//                 } else {
//                     radiusObjects[i].transform.GetComponent<Renderer>().material = noActiveRegion;
//                     CreateBuilding = false;
//                 }

//                 radiusObjects[i].transform.position = new Vector3(points[indexEnd].X, 0.35f, points[indexEnd].Y);
//                 // radiusObjects[i].GetComponent<BuildingInfo>().Index = indexEnd;
//             }
//         } catch(UnityException ex) {
//             Debug.Log("Error : "+ex.Message);
//         }
//     }

//     // 
//     public class pointMap
//     {
//         public float X;
//         public float Y;
//         public float sizeX;
//         public float sizeY;
//         public bool create;

//         public pointMap(float _X, float _Y, float _sizeX, float _sizeY)
//         {
//             X = _X;
//             Y = _Y;
//             sizeX = _sizeX;
//             sizeY = _sizeY;
//             create = true;
//         }
//     }

// }
