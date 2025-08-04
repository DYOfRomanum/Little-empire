using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_manager : MonoBehaviour
{
    // [Header("UI References")]
    public GameObject languagePanel;
    public GameObject accountPanel;
    public GameObject settingPanel;
    public Button openUiButton;
    public Button applyButton;
    public Button cancelButton;
    // [Header("Setting Components")]
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Open language ui
    public void OpenEmptyUi()
    {
        languagePanel.SetActive(true);
    }
    public void CloseEmptyUi()
    {
        languagePanel.SetActive(false);
    }
        // Open account ui
    public void OpenAccUi()
    {
        accountPanel.SetActive(true);
    }
    public void CloseAccUi()
    {
        accountPanel.SetActive(false);
    }
        // Open language ui
    public void OpenSetUi()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSetUi()
    {
        settingPanel.SetActive(false);
    }
}
