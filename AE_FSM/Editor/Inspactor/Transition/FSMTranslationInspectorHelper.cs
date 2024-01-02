using UnityEditor;

namespace AE_FSM
{
    public class FSMTranslationInspectorHelper : ScriptableObjectSingleton<FSMTranslationInspectorHelper>
    {
        public RunTimeFSMController contorller;
        public FSMTranslationData translationData;

        public void Inspector(RunTimeFSMController contorller, FSMTranslationData translationData)
        {
            this.contorller = contorller;
            this.translationData = translationData;
            Selection.activeObject = this;
        }
    }
}