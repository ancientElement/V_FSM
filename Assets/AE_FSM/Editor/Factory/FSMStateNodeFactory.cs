﻿using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AE_FSM
{
    public class FSMStateNodeFactory
    {
        public static FSMStateNodeData CreateFSMNode(RunTimeFSMController contorller, string stateName, bool defaultState, Rect rect, string scriptName = "")
        {
            if (contorller.states.Where(x => x.name.Equals(stateName)).FirstOrDefault() != null)
            {
                Debug.LogError($"创建状态{stateName}节点失败名称重复!!!");
                return null;
            }

            FSMStateNodeData stateNodeData = new FSMStateNodeData();
            stateNodeData.name = stateName;
            if (scriptName == "")
                stateNodeData.scriptName = string.Empty;
            else
                stateNodeData.scriptName = scriptName;

            stateNodeData.rect = rect;

            if (defaultState)
            {
                foreach (var item in contorller.states)
                {
                    item.defualtState = false;
                }
            }

            stateNodeData.defualtState = defaultState;

            contorller.states.Add(stateNodeData);

            EditorUtility.SetDirty(contorller);

            AssetDatabase.SaveAssets();

            return stateNodeData;
        }

        public static FSMStateNodeData CreateFSMNode(RunTimeFSMController contorller, bool defaultState, Rect rect)
        {
            return CreateFSMNode(contorller, GetName(contorller), defaultState, rect);
        }

        private static string GetName(RunTimeFSMController contorller)
        {
            int i = 0;
            string name;
            FSMStateNodeData state;

            do
            {
                name = $"New State_{i}";
                state = contorller.states.Where(x => x.name == name).FirstOrDefault();
                i += 1;
            } while (state != null);

            return name;
        }

        public static bool DeleteFSMNode(RunTimeFSMController contorller, FSMStateNodeData nodeData)
        {
            if (!contorller.states.Contains(nodeData))
                return false;

            if (nodeData.name.Equals(FSMConst.anyState) || nodeData.name.Equals(FSMConst.enterState))
            {
                Debug.LogError(" 不能删除 anyState 或者 enterState");
                return false;
            }

            //过渡相关
            for (int i = contorller.trasitions.Count - 1; i >= 0; i--)
            {
                if (contorller.trasitions[i].fromState == nodeData.name || contorller.trasitions[i].toState == nodeData.name)
                {
                    contorller.trasitions.RemoveAt(i);
                }
            }

            contorller.states.Remove(nodeData);
            EditorUtility.SetDirty(contorller);

            //默认状态
            if (nodeData.defualtState == true)
                foreach (FSMStateNodeData item in contorller.states)
                {
                    if (item.name != FSMConst.anyState && item.name != FSMConst.enterState)
                    {
                        item.defualtState = true;
                        break;
                    }
                }

            return true;
        }

        public static void ReNameFSMNode(RunTimeFSMController contorller, FSMStateNodeData nodeData, string newName)
        {
            if (!contorller.states.Contains(nodeData))
                return;

            if (nodeData.name.Equals(FSMConst.anyState) || nodeData.name.Equals(FSMConst.enterState))
            {
                Debug.LogError(" 不能修改 anyState 或者 enterState");
                return;
            }

            if (contorller.states.Where(x => x.name == newName).FirstOrDefault() != null)
                return;

            Debug.Log("**************");
            //相关过渡
            foreach (FSMTranslationData item in contorller.trasitions)
            {
                if (item.toState == nodeData.name)
                {
                    item.toState = newName;
                }
                if (item.fromState == nodeData.name)
                {
                    item.fromState = newName;
                }
            }

            nodeData.name = newName;

            EditorUtility.SetDirty(contorller);
            AssetDatabase.SaveAssets();
        }
    }
}