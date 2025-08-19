using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Login, Primary_empire, Secondary_empire, Union}
    private void Awake()
    {
        // if there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // set the static instance to this instance
            Instance = this;
        }
        //Make the gameobject persistent across scenes
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        SceneManager.LoadScene(locationToSwitch.ToString());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CleanUpDuplicateEventSystems();
    }

    // 清理重复事件系统
    private void CleanUpDuplicateEventSystems()
    {
        // 获取所有事件系统
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        
        // 如果存在多个事件系统
        if (eventSystems.Length > 1)
        {
            // 保留第一个激活的事件系统
            for (int i = 1; i < eventSystems.Length; i++)
            {
                // 只销毁未标记为Don't Destroy的对象
                if (eventSystems[i].gameObject.scene.name != "DontDestroyOnLoad")
                {
                    Destroy(eventSystems[i].gameObject);
                }
            }
        }
        
        // 确保有新输入模块
        if (FindObjectOfType<StandaloneInputModule>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

}
