using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class XMLTextLoader : MonoBehaviour
{
    public Text displayText; // UI中的Text组件
    public string arrayName = "str_brick1"; // 默认显示第一个数组

    void Start()
    {
        // 加载XML文件
        TextAsset xmlFile = Resources.Load<TextAsset>("XMLres/arrays");
        if (xmlFile == null)
        {
            Debug.LogError("XML文件未找到");
            return;
        }

        // 解析XML
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);

        // 查找指定的string-array
        XmlNodeList nodes = xmlDoc.SelectNodes($"//string-array[@name='{arrayName}']/item");
        if (nodes == null || nodes.Count < 2)
        {
            Debug.LogError($"未找到数组 {arrayName} 或内容不完整");
            return;
        }

        // 组合显示文本
        string title = nodes[0].InnerText;
        string description = nodes[1].InnerText;
        displayText.text = $"<b>{title}</b>\n\n{description}";
    }

    // 可以通过这个方法动态切换显示的数组
    public void LoadArray(string newArrayName)
    {
        arrayName = newArrayName;
        Start(); // 重新加载
    }
}