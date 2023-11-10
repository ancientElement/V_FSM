using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using AE_FSM;

namespace AE_FSMGV
{
    public class AE_FSMGVWindow : EditorWindow
    {
        public Context Context { get; private set; } = new Context();

        public ParamterView ParamterView { get; private set; }
        public StateAreaGraphView StateAreaView { get; private set; }

        [MenuItem("Tools/AE_FSMGV/EditorWindow")]
        public static void ShowExample()
        {
            AE_FSMGVWindow wnd = GetWindow<AE_FSMGVWindow>();
            wnd.titleContent = new GUIContent("AE状态机");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/AE_FSMGV/Editor/Window/AE_FSMGVWindow.uxml");
            visualTree.CloneTree(root);

            //style
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AE_FSMGV/Editor/Window/AE_FSMGVWindow.uss");
            root.styleSheets.Add(styleSheet);

            ParamterView = root.Q<ParamterView>();
            StateAreaView = root.Q<StateAreaGraphView>();

            ParamterView.Init(this);
            StateAreaView.Init(this);
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject as RunTimeFSMController != null || Selection.activeObject as GameObject != null)
            {
                ParamterView.Init(this);
                StateAreaView.Init(this);
            }
        }
    }
}