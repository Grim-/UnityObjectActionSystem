using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActionController))]
public class ActionControllerEditor : Editor
{
    #region Inspector Settings And Variables
    private bool showDebug = true;
    private ActionController controller = null;
    private ObjectReactionData objectAction = null;
    private int[][] actionsTabIndex;
    private int currentReactionIndex;
    private int currentActionIndex;
    private bool compactMode = true;
    private bool tablessMode = false;
    #endregion

    private const string REACTION_PROP = "reaction";
    private const string REACTION_OBJECT_ACTIONS_PROP = "actions";
    private const string SHOW_DEBUG_PROP = "showDebugInScene";
    private const string COMPACT_MODE_PROP = "compactMode";

    private GUIStyle borderedBox;
    private SerializedProperty isReactionEnabled;
    private SerializedProperty currentReaction;
    private SerializedProperty reactions;

    public override void OnInspectorGUI()
    {
        controller = (ActionController)target;
        objectAction = controller.GetCurrentReaction();
        serializedObject.Update();

        if(borderedBox == null) borderedBox = new GUIStyle(EditorStyles.helpBox);
        reactions = serializedObject.FindProperty(REACTION_PROP);
       
        DrawControllerToolbar();
        DrawReaction(reactions);
        DrawDebugSection();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawReaction(SerializedProperty reaction)
    {
        serializedObject.Update();

        if (actionsTabIndex == null)
        {
            if (reaction != null && reaction.isArray == true) actionsTabIndex = new int[reaction.arraySize][];
            else actionsTabIndex = new int[1][];
        }

        if(reaction.isArray)
        {
            for (int i = 0; i < reaction.arraySize; i++)
            {
                currentReactionIndex = i;
                currentReaction = reaction.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginVertical(borderedBox);

                EditorGUILayout.BeginHorizontal();
                
                SerializedProperty actions = reaction.GetArrayElementAtIndex(i).FindPropertyRelative(REACTION_OBJECT_ACTIONS_PROP);

                EditorGUILayout.PropertyField(reaction.GetArrayElementAtIndex(i).FindPropertyRelative("reactionName"));
                DrawListRemoveButton(reaction, "Remove", i,  (list, buttonText, index) =>
                {
                    ResizeReactionsTabArray();
                }, true);

                EditorGUILayout.EndHorizontal();

                GUILayout.BeginVertical();

                DrawActions(i, actions);

                GUILayout.EndVertical();

                EditorGUILayout.EndVertical();

            }
            DrawListAddButton(reaction, "Add Reaction", (list, buttonText, index)=>
            {
                ResizeReactionsTabArray();
            });
           
        }
        else
        {
            SerializedProperty actions = reaction.FindPropertyRelative(REACTION_OBJECT_ACTIONS_PROP);
            EditorGUILayout.BeginVertical(borderedBox);
            EditorGUILayout.PropertyField(reaction.FindPropertyRelative("reactionName"));
            DrawActions(0, actions);

            EditorGUILayout.EndVertical();
        }
    }
    private void DrawActions(int index, SerializedProperty actions)
    {
        for (int x = 0; x < actions.arraySize; x++)
        {
            currentActionIndex = x;

            SerializedProperty reactionAction = actions.GetArrayElementAtIndex(x);
            SerializedProperty objectAction = reactionAction.FindPropertyRelative("action");
            SerializedProperty objectActionData = reactionAction.FindPropertyRelative("data");
            ObjectAction obAction = (ObjectAction)objectAction.objectReferenceValue;

            if (actionsTabIndex[index] == null)
                actionsTabIndex[index] = new int[actions.arraySize];

            Color old = GUI.backgroundColor;

            if (!compactMode)
            {
                if (!tablessMode)
                {
                    DrawActionToolbar(index, x);
                }
            }

            if (controller.GetIsRunning() && controller.GetCurrentActionIndex() == x)
            {
                GUI.backgroundColor = Color.green;
            }

            EditorGUILayout.BeginHorizontal(borderedBox);
            if (compactMode)
            {
                if (!tablessMode)
                {
                    DrawActionToolbar(index, x);
                }
            }

            if(!tablessMode)
            {
                switch (actionsTabIndex[index][x])
                {
                    case 0:
                        DrawActionTab(reactionAction, obAction, x, actions);
                        break;
                    case 1:
                        DrawDataTab(reactionAction, obAction, actions);
                        break;
                }
            }
            else
            {
                EditorGUILayout.BeginVertical();
                DrawActionTab(reactionAction, obAction, x, actions);
                DrawDataTab(reactionAction, obAction, actions);

                EditorGUILayout.EndVertical();
            }


            
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = old;
        }
        
            
        DrawListAddButton(actions, "Add Action", (list, buttonText, ind) =>
        {
            ResizeActionsTabArray();
        });
    }
    private void DrawActionToolbar(int reactionIndex, int currentActionIndex)
    {
        EditorGUI.BeginChangeCheck();

        actionsTabIndex[reactionIndex][currentActionIndex] = GUILayout.Toolbar(actionsTabIndex[reactionIndex][currentActionIndex], new string[] { "Action", "Data" });

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void DrawActionTab(SerializedProperty objectReactionData, ObjectAction obAction, int actionIndex, SerializedProperty actions)
    {
        SerializedProperty objectAction = objectReactionData.FindPropertyRelative("action");
        SerializedProperty objectActionData = objectReactionData.FindPropertyRelative("data");
        SerializedProperty dataFloatValues = objectActionData.FindPropertyRelative("floatValues");
        SerializedProperty dataStringValues = objectActionData.FindPropertyRelative("stringValues");
        SerializedProperty dataBoolValues = objectActionData.FindPropertyRelative("boolValues");
        SerializedProperty dataIntValues = objectActionData.FindPropertyRelative("intValues");
        SerializedProperty dataVectorValues = objectActionData.FindPropertyRelative("vectorValues");
        SerializedProperty dataPrefabValues = objectActionData.FindPropertyRelative("prefabValues");
        SerializedProperty dataActionValues = objectActionData.FindPropertyRelative("actionValues");


        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        DrawToggleForActionStatus(objectReactionData);
     
        EditorGUI.BeginChangeCheck();

        ObjectAction oa = objectAction.objectReferenceValue as ObjectAction;

        objectAction.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none, objectAction.objectReferenceValue, typeof(ObjectAction), false, GUILayout.MinWidth(10f), GUILayout.MaxWidth(200f));

        if (EditorGUI.EndChangeCheck())
        {
            obAction = (ObjectAction)objectAction.objectReferenceValue;

            if (obAction != null)
            {
                PopulateFloatValues(dataFloatValues, obAction);
                PopulateStringValues(dataStringValues, obAction);
                PopulateBoolValues(dataBoolValues, obAction);
                PopulateIntValues(dataIntValues, obAction);
                PopulatePrefabValues(dataPrefabValues, obAction);
                PopulateVectorValues(dataVectorValues, obAction);
            }
            else
            {
                dataFloatValues.ClearArray();
                dataBoolValues.ClearArray();
                dataPrefabValues.ClearArray();
                dataIntValues.ClearArray();
                dataVectorValues.ClearArray();
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        DrawListRemoveButton(actions, "Remove", currentActionIndex, true);


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    private void DrawDataTab(SerializedProperty action, ObjectAction obAction, SerializedProperty actions)
    {
        SerializedProperty data = action.FindPropertyRelative("data");
        SerializedProperty floatValues = data.FindPropertyRelative("floatValues");
        SerializedProperty stringValues = data.FindPropertyRelative("stringValues");
        SerializedProperty boolValues = data.FindPropertyRelative("boolValues");
        SerializedProperty prefabValues = data.FindPropertyRelative("prefabValues");
        SerializedProperty vectorValues = data.FindPropertyRelative("vectorValues");
        SerializedProperty intValues = data.FindPropertyRelative("intValues");
        SerializedProperty actionValues = data.FindPropertyRelative("actionValues");
        EditorGUILayout.BeginVertical();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(data.FindPropertyRelative("targetType"));
        EditorGUILayout.EndHorizontal();
        ////Lists of Each Value Type      
        DrawFloatValues(floatValues);
        DrawStringValues(stringValues);
        DrawBoolValues(boolValues);
        DrawPrefabValues(prefabValues);
        DrawIntValues(intValues);
        DrawVectorValues(vectorValues);
        DrawActionValues(actionValues);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndVertical();
    }
    private void DrawDebugSection()
    {
        EditorGUILayout.Separator();
        GUIStyle centeredStyle = new GUIStyle();
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Options", centeredStyle);
        EditorGUILayout.BeginHorizontal();
        
        showDebug = serializedObject.FindProperty(SHOW_DEBUG_PROP).boolValue;
        compactMode = serializedObject.FindProperty(COMPACT_MODE_PROP).boolValue;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty(SHOW_DEBUG_PROP));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(COMPACT_MODE_PROP));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndHorizontal();
    }
    #region Data Values
    private void PopulateFloatValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.floatValues.Count;
            for (int i = 0; i < obAction.floatValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.floatValues[i].name;
                soo.FindPropertyRelative("value").floatValue = obAction.floatValues[i].value;
            }
        }
    }
    private void PopulateStringValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.stringValues.Count;
            for (int i = 0; i < obAction.stringValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.stringValues[i].name;
                soo.FindPropertyRelative("value").stringValue = obAction.stringValues[i].value;
            }
        }
    }
    private void PopulateBoolValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.boolValues.Count;
            for (int i = 0; i < obAction.boolValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.boolValues[i].name;
                soo.FindPropertyRelative("value").boolValue = obAction.boolValues[i].value;
            }
        }
    }
    private void PopulateIntValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.intValues.Count;
            for (int i = 0; i < obAction.intValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.intValues[i].name;
                soo.FindPropertyRelative("value").intValue = obAction.intValues[i].value;
            }
        }
    }
    private void PopulateVectorValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.vectorValues.Count;
            for (int i = 0; i < obAction.vectorValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.vectorValues[i].name;
                soo.FindPropertyRelative("value").vector3Value = obAction.vectorValues[i].value;
            }
        }
    }
    private void PopulatePrefabValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.prefabValues.Count;
            for (int i = 0; i < obAction.prefabValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.prefabValues[i].name;
                soo.FindPropertyRelative("value").objectReferenceValue = obAction.prefabValues[i].value;
            }
        }
    }
    private void PopulateActionValues(SerializedProperty dataValue, ObjectAction obAction)
    {
        if (dataValue != null)
        {
            dataValue.arraySize = obAction.actionValues.Count;
            for (int i = 0; i < obAction.actionValues.Count; i++)
            {
                SerializedProperty soo = dataValue.GetArrayElementAtIndex(i);
                soo.FindPropertyRelative("name").stringValue = obAction.actionValues[i].name;
                soo.FindPropertyRelative("value").objectReferenceValue = obAction.actionValues[i].value;
            }
        }
    }

    private void DrawFloatValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty floatValue = dataValue.GetArrayElementAtIndex(i);
                DrawDataValueLabel(floatValue);
                DrawFloatDataValue(floatValue);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();
        }
    }
    private void DrawStringValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty stringValue = dataValue.GetArrayElementAtIndex(i);
                DrawDataValueLabel(stringValue);
                DrawStringDataValue(stringValue);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();
        }
    }
    private void DrawBoolValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty boolValue = dataValue.GetArrayElementAtIndex(i);
                DrawDataValueLabel(boolValue);
                DrawBoolDataValue(boolValue);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();
        }
    }
    private void DrawIntValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty intValue = dataValue.GetArrayElementAtIndex(i);
                DrawDataValueLabel(intValue);
                DrawDataValueLabel(intValue);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    private void DrawVectorValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty vectorValue = dataValue.GetArrayElementAtIndex(i);
                DrawDataValueLabel(vectorValue);
                DrawVectorDataValue(vectorValue);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    private void DrawPrefabValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                if (i > dataValue.arraySize) break;
                EditorGUILayout.BeginHorizontal(borderedBox);
                SerializedProperty prefabValue = dataValue.GetArrayElementAtIndex(i);

                DrawDataValueLabel(prefabValue);
                DrawPrefabDataValuePreview(prefabValue);

                DrawListRemoveButton(dataValue, "Remove", i);

             
                EditorGUILayout.EndHorizontal();
            }

            DrawListAddButton(dataValue, "Add");
            EditorGUILayout.EndVertical();
        }      
    }
    private void DrawActionValues(SerializedProperty dataValue)
    {
        if (dataValue.arraySize > 0)
        {
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty actionValue = dataValue.GetArrayElementAtIndex(i);
                ObjectAction obAction = (ObjectAction)actionValue.FindPropertyRelative("value").objectReferenceValue;
                DrawDataValueLabel(actionValue);

                actionValue.FindPropertyRelative("value").objectReferenceValue = EditorGUILayout.ObjectField(actionValue.FindPropertyRelative("value").objectReferenceValue, typeof(ObjectAction), false);
                //Debug.Log(actionValue.FindPropertyRelative("value").FindPropertyRelative("scriptObjectPath").stringValue);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < dataValue.arraySize; i++)
            {
                SerializedProperty actionValue = dataValue.GetArrayElementAtIndex(i);
                ObjectAction obAction = (ObjectAction)actionValue.FindPropertyRelative("value").objectReferenceValue;

                if (obAction.floatValues.Count > 0)
                {
                    for (int x = 0; x < obAction.floatValues.Count; x++)
                    {
                        obAction.floatValues[x].name = EditorGUILayout.TextField(obAction.floatValues[x].name);
                    }
                }

                if (obAction.prefabValues.Count > 0)
                {
                    for (int x = 0; x < obAction.prefabValues.Count; x++)
                    {
                        obAction.prefabValues[x].value = (GameObject)EditorGUILayout.ObjectField(obAction.prefabValues[x].value, typeof(GameObject), false);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

    }
    #endregion

    #region UI
    private void OnSceneGUI()
    {
        DrawCurrentObjectActionLabel();
    }
    private void ToggleActionEnabled(SerializedProperty serializedProperty)
    {
        serializedProperty.FindPropertyRelative("enabled").boolValue = !serializedProperty.FindPropertyRelative("enabled").boolValue;
        serializedObject.ApplyModifiedProperties();    
    }
    private void DrawCurrentObjectActionLabel()
    {
        if (objectAction != null && showDebug)
        {
            if (controller.GetCurrentAction() != null && controller.GetIsRunning())
            {
                Handles.color = Color.white;
                Handles.Label(controller.transform.position + new Vector3(0, 2, 0), controller.GetCurrentAction().ToString());
            }
            else
            {
                Handles.Label(controller.transform.position + new Vector3(0, 2, 0), "No Actions Running");
            }

        }
    }
    private void DrawControllerToolbar()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.EndHorizontal();
    }
    private void DrawToggleForActionStatus(SerializedProperty action)
    {
        string buttonText;
        if (action.FindPropertyRelative("enabled").boolValue)
        {
            buttonText = "Enabled";
        }
        else
        {
            buttonText = "Disabled";
        }

        if (GUILayout.Button(buttonText, EditorStyles.miniButton, GUILayout.MaxWidth(80f)))
        {
            ToggleActionEnabled(action);
        }
    }
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
        if(!allowRemovalOfLastItem) if (list.arraySize == 1) return;

        if (GUILayout.Button(buttonText))
        {
            list.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
        }
    }
    private void DrawListRemoveButton(SerializedProperty list, string buttonText, int index, Action<SerializedProperty,string,int> callback, bool allowRemovalOfLastItem = false)
    {
        if(!allowRemovalOfLastItem) if (list.arraySize == 1) return;

        if (GUILayout.Button(buttonText))
        {
            list.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();

            callback.Invoke(list, buttonText, index);
        }
    }
    private void ResizeActionsTabArray()
    {
        if (currentReaction != null)
        {
            int[] temp = actionsTabIndex[currentReactionIndex];

            if (currentReaction.FindPropertyRelative("actions") != null)
            {
                actionsTabIndex[currentReactionIndex] = new int[currentReaction.FindPropertyRelative("actions").arraySize];
            }


            if (temp != null)
            {
                Array.Copy(temp, actionsTabIndex[currentReactionIndex], currentReaction.FindPropertyRelative("actions").arraySize - 1);
            }
        }
    }
    private void ResizeReactionsTabArray()
    {
        int[][] temp = actionsTabIndex;

        actionsTabIndex = new int[reactions.arraySize][];

        if (temp != null)
        {
            Array.Copy(temp, actionsTabIndex, reactions.arraySize - 1);
        }
    }

    private void DrawDataValueLabel(SerializedProperty nameProperty)
    {
        GUIContent label = new GUIContent(nameProperty.FindPropertyRelative("name").stringValue);
        Vector2 size = GUI.skin.box.CalcSize(label);
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(size.x));
    }
    private void DrawFloatDataValue(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").floatValue = EditorGUILayout.FloatField(dataValue.FindPropertyRelative("value").floatValue);
    }
    private void DrawStringDataValue(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").stringValue = EditorGUILayout.TextField(dataValue.FindPropertyRelative("value").stringValue);
    }
    private void DrawBoolDataValue(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").boolValue = EditorGUILayout.Toggle(dataValue.FindPropertyRelative("value").boolValue);
    }
    private void DrawIntDataValue(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").intValue = EditorGUILayout.IntField(dataValue.FindPropertyRelative("value").intValue);
    }
    private void DrawVectorDataValue(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, dataValue.FindPropertyRelative("value").vector3Value);
    }
    private void DrawPrefabDataValuePreview(SerializedProperty dataValue)
    {
        dataValue.FindPropertyRelative("value").objectReferenceValue = EditorGUILayout.ObjectField(dataValue.FindPropertyRelative("value").objectReferenceValue, typeof(GameObject), false);

        if (dataValue != null)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(dataValue.FindPropertyRelative("value").objectReferenceValue);
            //TODO : Move to own method for generating preview textures in labels
            if (texture != null) GUILayout.Label(texture, GUILayout.MaxWidth(90f), GUILayout.MaxHeight(90f));
        }
    }
    #endregion
}