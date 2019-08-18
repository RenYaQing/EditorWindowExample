using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class TextureSelectEditWindow : EditorWindow
{
    private List<string> iconNameList = new List<string>();
    private Vector2 iconScroll = Vector2.zero;
    private string iconRelativePath;
    #region Instance
    private static TextureSelectEditWindow instance = null;
    private Action<string> SelectCallBack;
    const int ICON_COUNT_PER_ROW = 4;
    const int ICON_SIZE = 60;
    #endregion

    public static TextureSelectEditWindow OpenWindow(string iconRelativePath, Action<string> selectCallBack)
    {
        TextureSelectEditWindow window = GetWindow<TextureSelectEditWindow>("Selector");
        instance = window;
        instance.iconRelativePath = iconRelativePath;
        instance.BuildIconNameList();
        instance.SelectCallBack = selectCallBack;

        return window;
    }
    private void BuildIconNameList()
    {
        iconNameList.Clear();
        string path = instance.iconRelativePath.Substring("Assets/".Length);
        string fullPath = Path.Combine(Application.dataPath, path);
        DirectoryInfo dirInfo = new DirectoryInfo(fullPath);

        if (!dirInfo.Exists)
        {
            Debug.LogError($"Dir not exist: {fullPath}");
            return;
        }

        FileInfo[] fileArray = dirInfo.GetFiles();
        Array.ForEach<FileInfo>(fileArray, fileInfo =>
        {
            if (fileInfo.FullName.EndsWith("png"))
            {
                iconNameList.Add(fileInfo.Name);
            }
        });
    }

    void OnGUI()
    {
        OnSelectorGUI();
    }
    private void OnSelectorGUI()
    {
        GUILayout.BeginVertical();

        iconNameList.ForEach(iconName =>
        {
            int index = iconNameList.IndexOf(iconName);

            if (index % ICON_COUNT_PER_ROW == 0)
            {
                GUILayout.BeginHorizontal();
            }

            string iconRelativePath = Path.Combine(instance.iconRelativePath, iconName);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconRelativePath);

            if (sprite != null)
            {
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(iconRelativePath);

                if (GUILayout.Button(texture, GUILayout.Width(ICON_SIZE), GUILayout.Height(ICON_SIZE)))
                {
                    IconSelected(iconName);
                }
            }
            else
            {
                GUILayout.Label($"sprite: {iconName}");
            }

            if (index % ICON_COUNT_PER_ROW == ICON_COUNT_PER_ROW - 1 || index == iconNameList.Count - 1)
            {
                GUILayout.EndHorizontal();
            }
        }); ;

        GUILayout.EndVertical();
    }

    #region Callback
    private void IconSelected(string iconName)
    {
        Debug.Log($"IconSelected: {iconName}");
        if (instance.SelectCallBack != null)
        {
            instance.SelectCallBack(iconName);
        }
        this.Close();
    }

    #endregion
}