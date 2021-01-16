using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ActionController))]
public class ActionControllerEditor : Editor
{
    private ActionController controller = null;
    private int[][] tabIndex;
    private int currentReactionIndex = 0;
    private int currentActionIndex = 0;
    private bool compactMode = true;
    private bool showDebugInScene = true;
    #region Styles
    public GUIStyle borderedBox;
    public GUIStyle headerText;
    public GUIStyle boldText;
    public GUIStyle boldWhiteText;
    public GUIStyle actionActiveStyle;
    #endregion

    private SerializedProperty reactionsProperty;

    private void OnEnable()
    {
        Initialise();
    }

    private void Initialise()
    {
        if (borderedBox == null) borderedBox = new GUIStyle(EditorStyles.helpBox);
        if (headerText == null) headerText = new GUIStyle(); headerText.alignment = TextAnchor.MiddleCenter;
        if (boldText == null) boldText = new GUIStyle(); boldText.fontStyle = FontStyle.Bold;
        if (boldWhiteText == null)
        {
            boldWhiteText = new GUIStyle(boldText);
            boldWhiteText.normal.textColor = Color.white;
        }
    }
    public override void OnInspectorGUI()
    {
        controller = (ActionController)target;
        DrawInspector();

        DrawInspectorFooter();
    }

   
    private void OnSceneGUI()
    {
        DrawCurrentObjectActionLabel();
    }

    public virtual void DrawInspector()
    {
        
        serializedObject.Update();
        reactionsProperty = serializedObject.FindProperty("reactions");

        if (tabIndex == null)
        {
            if (reactionsProperty != null && reactionsProperty.isArray) tabIndex = new int[reactionsProperty.arraySize][];
            else tabIndex = new int[1][];
        }

        DrawRunningProgressBar();

        if (reactionsProperty.arraySize > 0)
        {
            for (int i = reactionsProperty.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty reaction = reactionsProperty.GetArrayElementAtIndex(i);
                SetCurrentReaction(i);
                DrawReaction(reaction);
            }

            DrawListAddButton(GetReactions(), "Add Reaction", (list, buttonText, index) =>
            {
                ResizeReactionsTabArray();
            });
        }
        else
        {
            DrawListAddButton(GetReactions(), "Add Reaction", (list, buttonText, index) =>
            {
                ResizeReactionsTabArray();
            });
        }
    }

    public virtual void DrawInspectorFooter()
    {
        EditorGUILayout.Separator();
        GUIStyle centeredStyle = new GUIStyle();
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Options", centeredStyle);
        EditorGUILayout.BeginHorizontal();

        showDebugInScene = serializedObject.FindProperty("showDebugInScene").boolValue;
        compactMode = serializedObject.FindProperty("compactMode").boolValue;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("showDebugInScene"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("compactMode"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndHorizontal();
    }

    #region Reaction
    public virtual void DrawReaction(SerializedProperty reactionToDraw)
    {
        EditorGUILayout.BeginVertical(borderedBox);
        DrawReaction_Header(reactionToDraw);

        DrawReaction_Body(reactionToDraw);
        DrawReaction_Footer(reactionToDraw);

        EditorGUILayout.EndVertical();
    }
    public virtual void DrawReaction_Header(SerializedProperty reactionToDraw)
    {
        EditorGUILayout.BeginHorizontal();
        
        DrawReaction_Name(reactionToDraw);
        DrawListRemoveButton(GetReactions(), "Remove", currentReactionIndex, (list, buttonText, index) =>
        {
            ResizeReactionsTabArray();
        }, true);

        EditorGUILayout.EndHorizontal();
    }
    public virtual void DrawReaction_Body(SerializedProperty reactionToDraw)
    {
        EditorGUILayout.BeginVertical();
       
        for (int i = 0; i < reactionToDraw.FindPropertyRelative("actions").arraySize; i++)
        {
            SetCurrentAction(i);
            SerializedProperty currentAction = reactionToDraw.FindPropertyRelative("actions").GetArrayElementAtIndex(i);

            if (tabIndex[currentReactionIndex] == null)
                tabIndex[currentReactionIndex] = new int[reactionToDraw.FindPropertyRelative("actions").arraySize];

            DrawAction(currentAction);
        }

        //DrawRunningProgressBar();

        DrawListAddButton(GetReaction(currentReactionIndex).FindPropertyRelative("actions"), "Add Action", (list, buttonText, ind) =>
        {
            ResizeActionsTabArray();
        });
        EditorGUILayout.EndVertical();
    }
    public virtual void DrawReaction_Footer(SerializedProperty reactionToDraw)
    {
        

    }

    public virtual void DrawRunningProgressBar()
    {
        if (currentActionIndex > 0 && serializedObject.FindProperty("IsRunning").boolValue)
        {
            float actionsToDo = GetReaction(currentReactionIndex).FindPropertyRelative("actions").arraySize;
            float currentActionIndex = serializedObject.FindProperty("currentActionIndex").intValue+1;
            float progress = currentActionIndex / actionsToDo;
            //Debug.Log(progress);
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(20f);
            EditorGUI.ProgressBar(rect, progress, "Running (" + currentActionIndex + " / " + actionsToDo +")");
            EditorGUILayout.EndHorizontal();
        }
    }

    public virtual void DrawReaction_Name(SerializedProperty reactionToDraw)
    {
        var rect = EditorGUILayout.BeginHorizontal();
        var pos = Event.current.mousePosition;

        if (!rect.Contains(pos))
        {
            EditorGUILayout.LabelField(reactionToDraw.FindPropertyRelative("reactionName").stringValue, boldText);          
        }
        else
        {
            EditorGUI.BeginChangeCheck();

            reactionToDraw.FindPropertyRelative("reactionName").stringValue = EditorGUILayout.TextField(reactionToDraw.FindPropertyRelative("reactionName").stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.End)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region Action
    public virtual void DrawAction(SerializedProperty action)
    {    
        var rect = EditorGUILayout.BeginVertical();
        DrawAction_Header(action);
        DrawAction_Body(action);
        DrawAction_Footer(action);
        EditorGUILayout.EndVertical();      
    }
    private void DrawAction_Header(SerializedProperty action)
    {
       if(!compactMode) DrawActionToolbar(currentReactionIndex, currentActionIndex);
    }
    private void DrawAction_Body(SerializedProperty action)
    {
        Color oldColor = GUI.backgroundColor;
        if (serializedObject.FindProperty("IsRunning").boolValue && serializedObject.FindProperty("currentActionIndex").intValue == currentActionIndex)
        {
            GUI.backgroundColor = Color.blue;
        }


        var rect = EditorGUILayout.BeginHorizontal(borderedBox);
        GUI.backgroundColor = oldColor;
        var pos = Event.current.mousePosition;

        if (rect.Contains(pos))
        {
            if (Event.current.button == 1 && Event.current.type == EventType.MouseDown)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Action Above Here"), false, OnAddActionAbove, currentActionIndex);
                menu.AddItem(new GUIContent("Add Action Below Here"), false, OnAddActionBelow, currentActionIndex);
                menu.ShowAsContext();
                Event.current.Use();
            }

        }
        if (compactMode) DrawActionToolbar(currentReactionIndex, currentActionIndex);

        switch (tabIndex[currentReactionIndex][currentActionIndex])
        {
            case 0:
                DrawAction_ActionTab(action);
            break;
            case 1:
                DrawAction_DataTab(action);
            break;
        }

        EditorGUILayout.EndHorizontal();
    }
    private void DrawAction_Footer(SerializedProperty action)
    {

    }
    private void DrawAction_ActionTab(SerializedProperty action)
    {
        SerializedProperty objectActionProp = action.FindPropertyRelative("action");
        SerializedProperty dataFloatValues = action.FindPropertyRelative("data").FindPropertyRelative("floatValues");
        SerializedProperty dataStringValues = action.FindPropertyRelative("data").FindPropertyRelative("stringValues");
        SerializedProperty dataBoolValues = action.FindPropertyRelative("data").FindPropertyRelative("boolValues");
        SerializedProperty dataIntValues = action.FindPropertyRelative("data").FindPropertyRelative("intValues");
        SerializedProperty dataSOValues = action.FindPropertyRelative("data").FindPropertyRelative("soValues");
        SerializedProperty dataVectorValues = action.FindPropertyRelative("data").FindPropertyRelative("vectorValues");
        SerializedProperty dataPrefabValues = action.FindPropertyRelative("data").FindPropertyRelative("prefabValues");

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        DrawButtonAsToggle(action, "enabled");

        EditorGUI.BeginChangeCheck();

        objectActionProp.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none, objectActionProp.objectReferenceValue, typeof(ObjectAction), false, GUILayout.MinWidth(10f), GUILayout.MaxWidth(200f));

        if (EditorGUI.EndChangeCheck())
        {
            ObjectAction objectAction = (ObjectAction)objectActionProp.objectReferenceValue;

            if (objectAction != null)
            {
                PopulateDataValue(dataFloatValues, objectAction, DataValueType.FLOAT);
                PopulateDataValue(dataStringValues, objectAction, DataValueType.STRING);
                PopulateDataValue(dataBoolValues, objectAction, DataValueType.BOOL);
                PopulateDataValue(dataIntValues, objectAction, DataValueType.INT);
                PopulateDataValue(dataVectorValues, objectAction, DataValueType.VECTOR);
                PopulateDataValue(dataPrefabValues, objectAction, DataValueType.PREFAB);
                PopulateDataValue(dataSOValues, objectAction, DataValueType.SCRIPTABLE_OBJECT);
            }
            else
            {
                ClearAllDataValues(action.FindPropertyRelative("data"));
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        DrawListRemoveButton(GetReaction(currentReactionIndex).FindPropertyRelative("actions"), "Remove", currentActionIndex, true);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    private void DrawAction_DataTab(SerializedProperty action)
    {
        SerializedProperty data = action.FindPropertyRelative("data");
        SerializedProperty objectActionProp = action.FindPropertyRelative("action");
        ObjectAction objectAction = (ObjectAction)objectActionProp.objectReferenceValue;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(data.FindPropertyRelative("targetType"));

        EditorGUILayout.EndHorizontal();

        DrawDataValues(data.FindPropertyRelative("floatValues"), DataValueType.FLOAT, objectAction);
        DrawDataValues(data.FindPropertyRelative("stringValues"), DataValueType.STRING, objectAction);
        DrawDataValues(data.FindPropertyRelative("boolValues"), DataValueType.BOOL, objectAction);
        DrawDataValues(data.FindPropertyRelative("prefabValues"), DataValueType.PREFAB, objectAction);
        DrawDataValues(data.FindPropertyRelative("intValues"), DataValueType.INT, objectAction);
        DrawDataValues(data.FindPropertyRelative("vectorValues"), DataValueType.VECTOR, objectAction);
        DrawDataValues(data.FindPropertyRelative("soValues"), DataValueType.SCRIPTABLE_OBJECT, objectAction);
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
    private void ClearAllDataValues(SerializedProperty data)
    {
        if (data == null) return;
        data.FindPropertyRelative("floatValues").ClearArray();
        data.FindPropertyRelative("stringValues").ClearArray();
        data.FindPropertyRelative("boolValues").ClearArray();
        data.FindPropertyRelative("intValues").ClearArray();
        data.FindPropertyRelative("vectorValues").ClearArray();
        data.FindPropertyRelative("prefabValues").ClearArray();
        data.FindPropertyRelative("soValues").ClearArray();
    }
    #endregion

    #region UI Events
    private void OnAddActionAbove(object currentIndex)
    {
        int insetAtIndex = (int)currentIndex - 1;

        if (insetAtIndex < 0) insetAtIndex = 0;

        GetReaction(currentReactionIndex).FindPropertyRelative("actions").InsertArrayElementAtIndex(insetAtIndex);
        GetReaction(currentReactionIndex).FindPropertyRelative("actions").GetArrayElementAtIndex(insetAtIndex).FindPropertyRelative("action").objectReferenceValue = null;
        serializedObject.ApplyModifiedProperties();
        ResizeActionsTabArray();
    }
    private void OnAddActionBelow(object currentIndex)
    {
        int insetAtIndex = (int)currentIndex + 1;

        GetReaction(currentReactionIndex).FindPropertyRelative("actions").InsertArrayElementAtIndex(insetAtIndex);
        GetReaction(currentReactionIndex).FindPropertyRelative("actions").GetArrayElementAtIndex(insetAtIndex).FindPropertyRelative("action").objectReferenceValue = null;
        serializedObject.ApplyModifiedProperties();
        ResizeActionsTabArray();
    }
    private void OnMouseOver(Rect rect, Action callback)
    {
        var pos = Event.current.mousePosition;

        if (rect.Contains(pos))
        {
            callback.Invoke();
        }
    }
    private void OnMouseLeftClickAction(Rect rect, Action callback)
    {
        var pos = Event.current.mousePosition;
        if (rect.Contains(pos))
        {
            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
            {
                callback.Invoke();
            }
           
        }
    }
    private void OnMouseRightClickAction(Rect rect, Action callback)
    {
        var pos = Event.current.mousePosition;
        if (rect.Contains(pos))
        {
            if (Event.current.button == 1 && Event.current.type == EventType.MouseDown)
            {
                callback.Invoke();
            }
        }
    }
    #endregion

    #region Helpers 
    private void DrawListAddButton(SerializedProperty list, string buttonText)
    {
        if (GUILayout.Button(buttonText))
        {
            list.arraySize++;
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void DrawListAddButton(SerializedProperty list, string buttonText, Action<SerializedProperty, string, int> callback)
    {
        if (GUILayout.Button(buttonText))
        {
            list.arraySize++;
            serializedObject.ApplyModifiedProperties();
            callback.Invoke(list, buttonText, list.arraySize);
        }
    }
    private void DrawListRemoveButton(SerializedProperty list, string buttonText, int index, bool allowRemovalOfLastItem = false)
    {
        if (!allowRemovalOfLastItem) if (list.arraySize == 1) return;

        if (GUILayout.Button(buttonText))
        {
            list.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void DrawListRemoveButton(SerializedProperty list, string buttonText, int index, Action<SerializedProperty, string, int> callback, bool allowRemovalOfLastItem = false)
    {
        if (!allowRemovalOfLastItem) if (list.arraySize == 1) return;

        if (GUILayout.Button(buttonText))
        {
            list.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
            callback.Invoke(list, buttonText, index);
        }
    }

    private void ResizeActionsTabArray()
    {
        if (GetReaction(currentReactionIndex) != null)
        {
            int[] temp = tabIndex[currentReactionIndex];

            if (GetReaction(currentReactionIndex).FindPropertyRelative("actions") != null)
            {
                tabIndex[currentReactionIndex] = new int[GetReaction(currentReactionIndex).FindPropertyRelative("actions").arraySize];
            }


            if (temp != null)
            {
                if (GetReaction(currentReactionIndex).FindPropertyRelative("actions").arraySize > 0)
                    Array.Copy(temp, tabIndex[currentReactionIndex], GetReaction(currentReactionIndex).FindPropertyRelative("actions").arraySize - 1);
            }
        }
    }
    private void ResizeReactionsTabArray()
    {
        int[][] temp = tabIndex;

        tabIndex = new int[GetReactions().arraySize][];

        if (temp != null)
        {
            if (GetReactions().arraySize > 0)
                Array.Copy(temp, tabIndex, GetReactions().arraySize - 1);
        }
    }
    private void DrawButtonAsToggle(SerializedProperty action, string propertyKey, string enabledText = "Enabled", string disabledText = "Disabled")
    {
        string buttonText;
        if (action.FindPropertyRelative(propertyKey).boolValue)
        {
            buttonText = enabledText;
        }
        else
        {
            buttonText = disabledText;
        }

        if (GUILayout.Button(buttonText, EditorStyles.miniButton, GUILayout.MaxWidth(80f)))
        {
            ToggleActionEnabled(action);
        }
    }
    private void ToggleActionEnabled(SerializedProperty serializedProperty)
    {
        serializedProperty.FindPropertyRelative("enabled").boolValue = !serializedProperty.FindPropertyRelative("enabled").boolValue;
        serializedObject.ApplyModifiedProperties();
    }
    private void DrawActionToolbar(int reactionIndex, int currentActionIndex)
    {
        EditorGUI.BeginChangeCheck();

        tabIndex[reactionIndex][currentActionIndex] = GUILayout.Toolbar(tabIndex[reactionIndex][currentActionIndex], new string[] { "Action", "Data" });

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void PopulateDataValue(SerializedProperty dataValue, ObjectAction obAction, DataValueType type)
    {
        if (dataValue != null)
        {
            switch (type)
            {
                case DataValueType.FLOAT:
                    dataValue.arraySize = obAction.floatValues.Count;
                    for (int i = 0; i < obAction.floatValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.floatValues[i].name;
                        soo.FindPropertyRelative("value").floatValue = obAction.floatValues[i].value;
                    }
                    break;
                case DataValueType.INT:
                    dataValue.arraySize = obAction.intValues.Count;
                    for (int i = 0; i < obAction.intValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.intValues[i].name;
                        soo.FindPropertyRelative("value").intValue = obAction.intValues[i].value;
                    }
                    break;
                case DataValueType.STRING:
                    dataValue.arraySize = obAction.stringValues.Count;
                    for (int i = 0; i < obAction.stringValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.stringValues[i].name;
                        soo.FindPropertyRelative("value").stringValue = obAction.stringValues[i].value;
                    }
                    break;
                case DataValueType.BOOL:
                    dataValue.arraySize = obAction.boolValues.Count;
                    for (int i = 0; i < obAction.boolValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.boolValues[i].name;
                        soo.FindPropertyRelative("value").boolValue = obAction.boolValues[i].value;
                    }
                    break;
                case DataValueType.VECTOR:
                    dataValue.arraySize = obAction.vectorValues.Count;
                    for (int i = 0; i < obAction.vectorValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.vectorValues[i].name;
                        soo.FindPropertyRelative("value").vector3Value = obAction.vectorValues[i].value;
                    }
                    break;
                case DataValueType.PREFAB:
                    dataValue.arraySize = obAction.prefabValues.Count;
                    for (int i = 0; i < obAction.prefabValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.prefabValues[i].name;
                        soo.FindPropertyRelative("value").objectReferenceValue = obAction.prefabValues[i].value;
                    }
                    break;
                case DataValueType.SCRIPTABLE_OBJECT:
                    dataValue.arraySize = obAction.soValues.Count;
                    for (int i = 0; i < obAction.soValues.Count; i++)
                    {
                        SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                        soo.FindPropertyRelative("name").stringValue = obAction.soValues[i].name;
                        soo.FindPropertyRelative("value").objectReferenceValue = obAction.soValues[i].value;
                    }
                    break;
            }
        }
    }
    private void DrawDataValues(SerializedProperty dataValue, DataValueType type, ObjectAction objectAction)
    {
        for (int i = 0; i < dataValue.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            DrawDataValueLabel(dataValue.GetArrayElementAtIndex(i));
            DrawDataValue(dataValue.GetArrayElementAtIndex(i), type);

            if(type == DataValueType.PREFAB)
            {
                DrawListRemoveButton(dataValue, "Remove", i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (objectAction != null && type == DataValueType.PREFAB && objectAction.GetType() == typeof(SpawnRandomPrefab_Action))
        {
            DrawListAddButton(dataValue, "Add");
        }
       
    }
    private void DrawDataValueLabel(SerializedProperty nameProperty)
    {
        GUIContent label = new GUIContent(nameProperty.FindPropertyRelative("name").stringValue);
        Vector2 size = GUI.skin.box.CalcSize(label);
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(size.x));
    }
    private void DrawDataValue(SerializedProperty dataValue, DataValueType type)
    {
        switch (type)
        {
            case DataValueType.FLOAT:
                dataValue.FindPropertyRelative("value").floatValue = EditorGUILayout.FloatField(dataValue.FindPropertyRelative("value").floatValue);
                break;
            case DataValueType.INT:
                dataValue.FindPropertyRelative("value").intValue = EditorGUILayout.IntField(dataValue.FindPropertyRelative("value").intValue);
                break;
            case DataValueType.STRING:
                dataValue.FindPropertyRelative("value").stringValue = EditorGUILayout.TextField(dataValue.FindPropertyRelative("value").stringValue);
                break;
            case DataValueType.BOOL:
                dataValue.FindPropertyRelative("value").boolValue = EditorGUILayout.Toggle(dataValue.FindPropertyRelative("value").boolValue);
                break;
            case DataValueType.VECTOR:
                dataValue.FindPropertyRelative("value").vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, dataValue.FindPropertyRelative("value").vector3Value);
                break;
            case DataValueType.PREFAB:
                dataValue.FindPropertyRelative("value").objectReferenceValue = EditorGUILayout.ObjectField(dataValue.FindPropertyRelative("value").objectReferenceValue, typeof(GameObject), false);
                if (dataValue != null)
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(dataValue.FindPropertyRelative("value").objectReferenceValue);
                    //TODO : Move to own method for generating preview textures in labels
                    if (texture != null) GUILayout.Label(texture, GUILayout.MaxWidth(90f), GUILayout.MaxHeight(90f));
                }
                break;
            case DataValueType.SCRIPTABLE_OBJECT:
                dataValue.FindPropertyRelative("value").objectReferenceValue = EditorGUILayout.ObjectField(dataValue.FindPropertyRelative("value").objectReferenceValue, typeof(ScriptableObject), false);
                break;
        }
    }
    private void DrawCurrentObjectActionLabel()
    {
        Color oldCOlor = Handles.color;

        Handles.color = Color.white;
        if (controller.GetCurrentAction() != null && showDebugInScene)
        {
            if (controller.GetCurrentAction() != null && controller.GetIsRunning())
            {              
                Handles.Label(controller.transform.position + new Vector3(0, 2, 0), controller.GetCurrentAction().ToString(), boldWhiteText);
            }
            else
            {
                Handles.Label(controller.transform.position + new Vector3(0, 2, 0), "No Actions Running", boldWhiteText);
            }
        }
        Handles.color = oldCOlor;
    }
    private void SetCurrentAction(int index)
    {
        currentActionIndex = index;
    }
    private void SetCurrentReaction(int index)
    {
        currentReactionIndex = index;
    }
    private SerializedProperty GetReactions()
    {
        return serializedObject.FindProperty("reactions");
    }
    private SerializedProperty GetReaction(int index)
    {
        return serializedObject.FindProperty("reactions").GetArrayElementAtIndex(index);
    }
    private SerializedProperty GetActionsForReaction(int index)
    {
        return serializedObject.FindProperty("reactions").GetArrayElementAtIndex(index).FindPropertyRelative("actions");
    }
    #endregion
}


public enum DataValueType
{
    FLOAT,
    INT,
    STRING,
    BOOL,
    VECTOR,
    PREFAB,
    SCRIPTABLE_OBJECT
}