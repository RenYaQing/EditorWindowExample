using UnityEngine;
using UnityEditor;


public enum SelectType
{
    Select1 = 1,
    Select2 = 2,
}
public class MyWindow : EditorWindow
{
    string myString = "Hello World";
    string textAreaStr = "Hello World \n Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    int autoType = 1;
    SelectType selectType = SelectType.Select1;
    int selectIndex = 0;

    GUIStyle boxStyle;
    Texture selectTexture;
    string texturePath = "Assets/Textures/";
    string selectTexturePath = "Assets/Textures/rate_us.png";

    int selGridInt = 0;
    string[] selStrings = { "select1", "select2", "select3" ,"select4"};
    
    int toolBarInt = 0;
    string[] toolBarStrings = { "tool1", "tool2", "tool3" ,"tool4"};
    float vSbarValue;
    float scrollPos = 0.5f;
    
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }

    void OnGUI()
    {
        //Base Settings
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
        //TextArea 多行显示
        GUILayout.Label("TextArea",EditorStyles.boldLabel);
        textAreaStr = EditorGUILayout.TextArea(textAreaStr);
        //Optional Settings
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        //Optional Settings 2
        bool needAuto = EditorGUILayout.ToggleLeft("Optional Settings 2", autoType != 0, EditorStyles.boldLabel);
        if (needAuto)
        {
            EditorGUI.indentLevel += 1;//缩进
            if (autoType == 0)
            {
                autoType = 1;
            }
            if (EditorGUILayout.ToggleLeft("full", autoType == 1)) autoType = 1;
            if (EditorGUILayout.ToggleLeft("empty", autoType == 2)) autoType = 2;
            EditorGUI.indentLevel -= 1;//缩进
        }
        else
        {
            autoType = 0;
        }

        //Selection
        selectType = (SelectType)EditorGUILayout.EnumPopup("Select Type", selectType);

        selectIndex = EditorGUILayout.Popup("Select Index", selectIndex, new string[] { "selectStr1", "selectStr2", "selectStr3" });

        if (boxStyle == null)
        {
            GUIStyle tempStyle = GUI.skin.GetStyle("box");
            boxStyle = new GUIStyle(tempStyle);
            boxStyle.normal.background = null;
            // boxStyle.contentOffset = new Vector2(0, -7);
        }
        //Box ShowTexture
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinHeight(30));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Texture Show", EditorStyles.boldLabel, GUILayout.Width(130));
        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/btn_store.png");
        // GUILayout.Box(texture,GUILayout.Width(50), GUILayout.Height(50),GUILayout.Width(50));
        GUILayout.Box(texture, boxStyle, GUILayout.Width(50), GUILayout.Height(50));
        EditorGUI.indentLevel += 1;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        //TextureSelect Button
        GUILayout.BeginHorizontal();
        GUILayout.Label("Texture Select", EditorStyles.boldLabel, GUILayout.Width(140));
        selectTexture = AssetDatabase.LoadAssetAtPath<Texture>(selectTexturePath);
        if (GUILayout.Button(selectTexture, GUILayout.Width(60f), GUILayout.Height(60f)))
        {
            TextureSelectEditWindow.OpenWindow(texturePath, OnTexturePicker);
        }
        GUILayout.EndHorizontal();
        //RepeatBtn 按下和抬起鼠标发送事件
        if (GUILayout.RepeatButton("RepeatBtn"))
        {
            Debug.Log("repeatBtn");
        }

        //SelectionGrid
        GUILayout.Label("SelectionGrid",EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 2);
        if (GUILayout.Button("selectFinish"))
        {
            Debug.Log("You chose " + selStrings[selGridInt]);
        }
        GUILayout.EndVertical();

        //ToolBar
        GUILayout.Label("ToolBar",EditorStyles.boldLabel);
        toolBarInt = GUILayout.Toolbar(toolBarInt,toolBarStrings);

        //Scrollbar
        GUILayout.Label("Scrollbar",EditorStyles.boldLabel);
        vSbarValue = GUILayout.VerticalScrollbar(vSbarValue, 1.0f, 100.0f, 0.0f,GUILayout.Height(100));
        scrollPos = GUILayout.HorizontalScrollbar(scrollPos, 10, 0, 100);
    }
    private void OnTexturePicker(string iconName)
    {
        selectTexturePath = texturePath + iconName;
    }
}
