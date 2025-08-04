using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class Create_building : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public BuildingCursor buildingCursor;
    public RenderMatrixMap renderMatrixMap;

    public void CreateBuilding(string buildingName)
    {
        GameObject prefab = null;
        
        switch (buildingName)
        {
            case "House":
                prefab = buildingPrefabs[0];
                break;
            case "Gold_mine":
                prefab = buildingPrefabs[1];
                break;
            case "Crystal_mine":
                prefab = buildingPrefabs[2];
                break;
            case "Cage":
                prefab = buildingPrefabs[3];
                break;
        }
        
        if (prefab != null)
        {
            // 实例化建筑但暂时不显示
            GameObject building = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            building.SetActive(false);
            
            // 添加建筑控制器
            BuildingController controller = building.AddComponent<BuildingController>();
            controller.Initialize(renderMatrixMap);
            
            // 开始放置流程
            buildingCursor.StartPlacement(building);
        }
    }
    // public GameObject[] buildings;
    // private GameObject building;
    // public Cursor _Cursor;

    // // Start is called before the first frame update
    // public void createBuilding(string name)
    // {
    //     // select building prefab according to the name of the button
    //     switch (name)
    //     {
    //         case "House":
    //             // Debug.Log(buildings[0].transform.localScale);
    //             building = Instantiate(buildings[0], new Vector3(0,0,0),Quaternion.identity) as GameObject;
    //             break;
    //         case "Gold_mine":
    //             // Debug.Log(buildings[0].transform.localScale);
    //             building = Instantiate(buildings[1], new Vector3(0,0,0),Quaternion.identity) as GameObject;
    //             break;
    //         case "Crystal_mine":
    //             // Debug.Log(buildings[0].transform.localScale);
    //             building = Instantiate(buildings[2], new Vector3(0,0,0),Quaternion.identity) as GameObject;
    //             break;
    //         case "Cage":
    //             // Debug.Log(buildings[0].transform.localScale);
    //             building = Instantiate(buildings[3], new Vector3(0,0,0),Quaternion.identity) as GameObject;
    //             break;
                

    //     }
    //     if (building != null)
    //     {
    //         // create initial regions
    //         Vector3 buildingScale = building.transform.localScale;
    //         Vector2[] buildingMatrix = new Vector2[(int)buildingScale.x * (int)buildingScale.z];
    //         double x0 = -Ceiling((buildingScale.x - 1)/2);
    //         double y0 = -Ceiling((buildingScale.z - 1)/2);
    //         for (int x=0; x<(int)buildingScale.x; x++)
    //         {
    //             for (int y=0; y<(int)buildingScale.z; y++)
    //             {
    //                 buildingMatrix[x * y + y] = new Vector2((int)x0 + x, (int)y0 + y);
    //                 // Debug.Log(new Vector2((int)x0 + x, (int)y0 + y));
    //             }
    //         }
    //         // 把初始化的占地区域提供给Render
    //         _Cursor.RenderMatrixMap.shemeRadius = buildingMatrix;
    //         _Cursor.createBuilding = building;
    //         _Cursor.RenderMatrixMap.refreshMatrixCursor();
    //     }
    // }
}
