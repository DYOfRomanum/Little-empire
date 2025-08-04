using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public GameObject buildingManager;
    public GameObject buildingPanel;
    void OnGUI()
    {
        if (GUI.Button (new Rect (0,0,0,0), "Create house"))
        {
            Debug.Log("Clicked");
            // this.SendMessage("")
        }
    }
    public void BuildingButton()
    {
        Debug.Log(this.name);
        buildingManager.SendMessage("CreateBuilding",this.name);
        CloseUi();
    }
    public void OpenUi()
    {
        buildingPanel.SetActive(true);
    }
    public void CloseUi()
    {
        buildingPanel.SetActive(false);
    }
}

