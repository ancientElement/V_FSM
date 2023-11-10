﻿using UnityEditor;
using UnityEngine;

namespace AE_FSM
{
    public class FSMTranslationFactory
    {
        public static FSMTranslationData CreateTransition(RunTimeFSMController contorller, string fromStateName, string toStateName)
        {
            if (toStateName == FSMConst.enterState || toStateName == FSMConst.anyState)
            {
                Debug.LogError($"无法从 <color=yellow>{fromStateName}</color>过渡到<color=yellow>{toStateName}</color>");
                return null;
            }

            if (fromStateName == toStateName)
            {
                return null;
            }

            foreach (FSMTranslationData item in contorller.trasitions)
            {
                if (item.fromState == fromStateName && item.toState == toStateName)
                {
                    Debug.LogError($"过渡<color=yellow>{fromStateName}</color>到<color=yellow>{toStateName}</color>已经存在");
                    return null;
                }
            }

            FSMTranslationData trasitionData = new FSMTranslationData();
            trasitionData.fromState = fromStateName;
            trasitionData.toState = toStateName;
            contorller.trasitions.Add(trasitionData);

            EditorUtility.SetDirty(contorller);
            AssetDatabase.SaveAssets();

            return trasitionData;
        }

        public static void DeleteTransition(RunTimeFSMController contorller, FSMTranslationData trasitionData)
        {
            contorller.trasitions.Remove(trasitionData);
            EditorUtility.SetDirty(contorller);
            AssetDatabase.SaveAssets();
        }
    }
}
