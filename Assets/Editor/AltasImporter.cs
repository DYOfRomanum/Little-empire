using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class AtlasImporter : EditorWindow
{
    private TextAsset xmlFile;
    private string textureFolder = "Assets/";
    private Dictionary<string, Texture2D> atlasTextures = new Dictionary<string, Texture2D>();
    private Dictionary<string, List<SpriteData>> spriteData = new Dictionary<string, List<SpriteData>>();
    private Vector2 scrollPosition;
    private string status = "Ready";
    private bool showParsedData = true;

    [MenuItem("Tools/Atlas Importer")]
    public static void ShowWindow()
    {
        GetWindow<AtlasImporter>("Atlas Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Atlas Importer", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        // XML文件选择
        EditorGUI.BeginChangeCheck();
        xmlFile = (TextAsset)EditorGUILayout.ObjectField("XML File", xmlFile, typeof(TextAsset), false);
        if (EditorGUI.EndChangeCheck() && xmlFile != null)
        {
            ParseXML(xmlFile.text);
        }

        // 纹理文件夹选择
        EditorGUILayout.BeginHorizontal();
        textureFolder = EditorGUILayout.TextField("Texture Folder", textureFolder);
        if (GUILayout.Button("Browse", GUILayout.Width(80)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Texture Folder", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                textureFolder = "Assets" + path.Replace(Application.dataPath, "");
            }
        }
        EditorGUILayout.EndHorizontal();

        // 状态显示
        EditorGUILayout.LabelField("Status", status);
        EditorGUILayout.Space(10);

        // 导入按钮
        if (GUILayout.Button("Import Atlas Sprites", GUILayout.Height(30)))
        {
            if (xmlFile == null)
            {
                status = "Error: Please select an XML file";
                Debug.LogError(status);
                return;
            }

            if (!Directory.Exists(textureFolder))
            {
                status = "Error: Texture folder does not exist";
                Debug.LogError(status);
                return;
            }

            ImportSprites();
        }

        // 刷新按钮
        if (GUILayout.Button("Refresh Preview", GUILayout.Height(25)))
        {
            if (xmlFile != null)
            {
                ParseXML(xmlFile.text);
            }
        }

        // 显示解析结果
        if (spriteData.Count > 0)
        {
            EditorGUILayout.Space(20);
            
            // 折叠标题
            showParsedData = EditorGUILayout.Foldout(showParsedData, "Parsed Atlas Data", true);
            
            if (showParsedData)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                
                foreach (var atlas in spriteData)
                {
                    EditorGUILayout.Space(10);
                    EditorGUILayout.BeginVertical("box");
                    
                    // 图集标题
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Atlas: {atlas.Key}", EditorStyles.boldLabel, GUILayout.Width(150));
                    
                    // 显示纹理预览
                    if (atlasTextures.ContainsKey(atlas.Key))
                    {
                        EditorGUILayout.LabelField(new GUIContent(atlasTextures[atlas.Key]), 
                            GUILayout.Width(100), GUILayout.Height(100));
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Texture not found", GUILayout.Height(100));
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    
                    // 子图信息
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < atlas.Value.Count; i++)
                    {
                        var sprite = atlas.Value[i];
                        EditorGUILayout.BeginHorizontal();
                        
                        // 交替行颜色
                        if (i % 2 == 0)
                        {
                            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
                        }
                        else
                        {
                            GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
                        }
                        
                        EditorGUILayout.LabelField(sprite.name, GUILayout.Width(200));
                        EditorGUILayout.LabelField($"X:{sprite.x}", GUILayout.Width(60));
                        EditorGUILayout.LabelField($"Y:{sprite.y}", GUILayout.Width(60));
                        EditorGUILayout.LabelField($"W:{sprite.width}", GUILayout.Width(60));
                        EditorGUILayout.LabelField($"H:{sprite.height}", GUILayout.Width(60));
                        
                        GUI.backgroundColor = Color.white;
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                    
                    EditorGUILayout.EndVertical();
                }
                
                EditorGUILayout.EndScrollView();
            }
        }
    }

    private void ParseXML(string xmlContent)
    {
        spriteData.Clear();
        atlasTextures.Clear();
        
        try
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var arrays = doc.Root.Elements("integer-array");
            
            // 先收集所有图集的基本信息
            foreach (var array in arrays)
            {
                string name = array.Attribute("name").Value;
                
                // 跳过子图信息（包含下划线的）
                if (name.Contains("_")) continue;
                
                List<int> values = new List<int>();
                foreach (var item in array.Elements("item"))
                {
                    values.Add(int.Parse(item.Value));
                }
                
                // 只处理有两个值的（宽高定义）
                if (values.Count == 2)
                {
                    spriteData[name] = new List<SpriteData>();
                    
                    // 尝试加载纹理
                    string texturePath = $"{textureFolder}/{name}.png";
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                    
                    if (texture != null)
                    {
                        atlasTextures[name] = texture;
                    }
                    else
                    {
                        Debug.LogWarning($"Texture not found: {texturePath}");
                    }
                }
            }
            
            // 收集子图信息
            foreach (var array in arrays)
            {
                string name = array.Attribute("name").Value;
                
                // 只处理包含下划线的子图
                if (!name.Contains("_")) continue;
                
                // 提取图集名称（altas0_muscle_weapon -> altas0）
                string atlasName = name;
                if (name.Contains("_"))
                {
                    atlasName = name.Substring(0, name.IndexOf('_'));
                }
                
                if (!spriteData.ContainsKey(atlasName))
                {
                    // 尝试查找更精确的匹配（例如altas0_muscle_weapon对应altas0）
                    bool found = false;
                    foreach (var key in spriteData.Keys)
                    {
                        if (name.StartsWith(key))
                        {
                            atlasName = key;
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found)
                    {
                        Debug.LogWarning($"No atlas found for sprite: {name}");
                        continue;
                    }
                }
                
                List<int> values = new List<int>();
                foreach (var item in array.Elements("item"))
                {
                    values.Add(int.Parse(item.Value));
                }
                
                // 需要4个值：x, y, width, height
                if (values.Count >= 4)
                {
                    spriteData[atlasName].Add(new SpriteData
                    {
                        name = name,
                        x = values[0],
                        y = values[1],
                        width = values[2],
                        height = values[3]
                    });
                }
                else
                {
                    Debug.LogWarning($"Insufficient data for sprite: {name} (expected 4 values, got {values.Count})");
                }
            }
            
            status = $"Parsed {spriteData.Count} atlases with {GetTotalSpriteCount()} sprites";
            Debug.Log(status);
        }
        catch (System.Exception e)
        {
            status = $"Error parsing XML: {e.Message}";
            Debug.LogError(e);
        }
    }

    private int GetTotalSpriteCount()
    {
        int count = 0;
        foreach (var atlas in spriteData)
        {
            count += atlas.Value.Count;
        }
        return count;
    }

    private void ImportSprites()
    {
        if (spriteData.Count == 0)
        {
            ParseXML(xmlFile.text);
        }
        
        int createdSprites = 0;
        int skippedSprites = 0;
        
        foreach (var atlas in spriteData)
        {
            string atlasName = atlas.Key;
            
            if (!atlasTextures.ContainsKey(atlasName))
            {
                Debug.LogWarning($"Skipping atlas {atlasName} - texture not loaded");
                skippedSprites += atlas.Value.Count;
                continue;
            }
            
            Texture2D texture = atlasTextures[atlasName];
            string folderPath = $"{textureFolder}/{atlasName}_Sprites";
            
            // 创建保存Sprite的文件夹
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string parentFolder = Path.GetDirectoryName(folderPath);
                string newFolder = Path.GetFileName(folderPath);
                
                if (!AssetDatabase.IsValidFolder(parentFolder))
                {
                    Debug.LogWarning($"Cannot create folder: {folderPath} - parent folder doesn't exist");
                    continue;
                }
                
                string guid = AssetDatabase.CreateFolder(parentFolder, newFolder);
                folderPath = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log($"Created folder: {folderPath}");
            }
            
            foreach (var spriteInfo in atlas.Value)
            {
                // 坐标转换：XML是左上原点，Unity是左下原点
                int flippedY = texture.height - spriteInfo.y - spriteInfo.height;
                
                // 验证矩形区域是否有效
                if (spriteInfo.x < 0 || spriteInfo.y < 0 || 
                    spriteInfo.width <= 0 || spriteInfo.height <= 0 ||
                    spriteInfo.x + spriteInfo.width > texture.width ||
                    flippedY + spriteInfo.height > texture.height)
                {
                    Debug.LogWarning($"Invalid sprite rect for {spriteInfo.name}: " +
                                    $"X={spriteInfo.x}, Y={spriteInfo.y}, " +
                                    $"W={spriteInfo.width}, H={spriteInfo.height}");
                    skippedSprites++;
                    continue;
                }
                
                Rect rect = new Rect(spriteInfo.x, flippedY, spriteInfo.width, spriteInfo.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                
                // 创建Sprite
                Sprite sprite = Sprite.Create(texture, rect, pivot, 100);
                sprite.name = spriteInfo.name;
                
                // 保存为独立资源
                string spritePath = $"{folderPath}/{sprite.name}.asset";
                
                // 检查是否已存在
                Sprite existing = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (existing != null)
                {
                    // 更新现有Sprite
                    existing = Sprite.Create(texture, rect, pivot, 100);
                    existing.name = spriteInfo.name;
                    EditorUtility.SetDirty(existing);
                    Debug.Log($"Updated sprite: {spritePath}");
                }
                else
                {
                    // 创建新Sprite
                    AssetDatabase.CreateAsset(sprite, spritePath);
                    Debug.Log($"Created sprite: {spritePath}");
                }
                
                createdSprites++;
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        status = $"Success! Created {createdSprites} sprites, skipped {skippedSprites} in {spriteData.Count} atlases";
        Debug.Log(status);
        
        // 显示结果对话框
        EditorUtility.DisplayDialog("Atlas Import Complete", 
            $"Successfully imported {createdSprites} sprites from {spriteData.Count} atlases.\n" +
            $"Skipped {skippedSprites} invalid sprites.", 
            "OK");
    }

    private class SpriteData
    {
        public string name;
        public int x;
        public int y;
        public int width;
        public int height;
    }
}