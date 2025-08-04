using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_swap : MonoBehaviour
{
    public string newScene = "Primary_empire";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneSwap()
    {
        Debug.Log($"正在加载场景: {newScene}");
        SceneManager.LoadScene(newScene);
    }
}
