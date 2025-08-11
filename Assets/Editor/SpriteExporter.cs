using UnityEditor;
using UnityEngine;
using System.IO;

public class SpriteExporter
{
    [MenuItem("Assets/Export Sprite as PNG")]
    private static void ExportSpriteAsPNG()
    {
        // 获取选中的 Sprite
        Sprite sprite = Selection.activeObject as Sprite;
        
        if (sprite == null)
        {
            Debug.LogError("Please select a Sprite first!");
            return;
        }
        
        // 获取原始纹理
        Texture2D originalTexture = sprite.texture;
        
        // 创建新纹理
        Texture2D newTexture = new Texture2D(
            (int)sprite.rect.width,
            (int)sprite.rect.height,
            TextureFormat.RGBA32,
            false
        );
        
        // 复制子图区域
        Color[] pixels = originalTexture.GetPixels(
            (int)sprite.rect.x,
            (int)sprite.rect.y,
            (int)sprite.rect.width,
            (int)sprite.rect.height
        );
        
        newTexture.SetPixels(pixels);
        newTexture.Apply();
        
        // 保存为 PNG
        string path = EditorUtility.SaveFilePanel(
            "Save Sprite as PNG",
            "",
            sprite.name + ".png",
            "png"
        );
        
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllBytes(path, newTexture.EncodeToPNG());
            Debug.Log($"Sprite exported to: {path}");
        }
        
        // 清理
        Object.DestroyImmediate(newTexture);
    }
}
