using UnityEditor;
using UnityEngine;

namespace AE_FSM
{
    [CustomEditor(typeof(FSMStateInspectorHelper))]
    public class FSMStateInspector : Editor
    {
        public string stateName;

        public override void OnInspectorGUI()
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;

            bool disable = EditorApplication.isPlaying || helper.stateNodeData.name == FSMConst.enterState || helper.stateNodeData.name == FSMConst.anyState;

            EditorGUI.BeginDisabledGroup(disable);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("scriptName", GUILayout.Width(80));
            helper.stateNodeData.scriptName = EditorGUILayout.DelayedTextField(helper.stateNodeData.scriptName);

            if (helper.stateNodeData.scriptName != stateName)
            {
                stateName = helper.stateNodeData.scriptName;
                EditorUtility.SetDirty(helper.contorller);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }

        protected override void OnHeaderGUI()
        {
            FSMStateInspectorHelper helper = target as FSMStateInspectorHelper;
            if (helper == null) return;

            bool disable = EditorApplication.isPlaying || helper.stateNodeData.name == FSMConst.enterState || helper.stateNodeData.name == FSMConst.anyState;

            string name = null;
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(EditorGUIUtility.IconContent("icons/processed/unityeditor/animations/animatorstate icon.asset"), GUILayout.Width(30), GUILayout.Height(30));
                EditorGUILayout.LabelField("Name", GUILayout.Width(80));

                EditorGUI.BeginDisabledGroup(disable);
                name = EditorGUILayout.DelayedTextField(helper.stateNodeData.name);

                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                FSMStateNodeFactory.ReNameFSMNode(helper.contorller, helper.stateNodeData, name);
            }

            var rect = EditorGUILayout.BeginHorizontal();

            EditorGUILayout.Space();
            Handles.color = Color.black;
            Handles.DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y));
            EditorGUILayout.Space();

            EditorGUILayout.EndHorizontal();
        }
    }
}