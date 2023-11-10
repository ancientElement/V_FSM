using AE_FSM;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AE_FSMGV
{
    public class StateNodeView : Node
    {
        public static readonly float widthScale = 0.3f;
        public static readonly float heightScale = 0.3f;

        public Port outPort;
        public Port inputPort;

        public FSMStateNodeData nodeData;
        public AE_FSMGVWindow FSMEditorWindowGV;
        public Context Context => FSMEditorWindowGV.Context;

        public StateNodeView(FSMStateNodeData nodeData, AE_FSMGVWindow FSMEditorWindowGV) : base("Assets/AE_FSMGV/Editor/Window/NodeView.uxml")
        {
            this.nodeData = nodeData;
            this.FSMEditorWindowGV = FSMEditorWindowGV;

            title = nodeData.name;

            style.left = nodeData.rect.x * widthScale;
            style.top = nodeData.rect.y * heightScale;
            style.width = nodeData.rect.width * 0.5f;
            style.width = nodeData.rect.height * 1.5f;

            if (nodeData == null) { return; }

            if (nodeData.name != FSMConst.enterState && nodeData.name != FSMConst.anyState)
                CreateInput();

            CreateOutPut();

            AddToClassList(GetSelfClass());

            RefreshExpandedState();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("添加过渡", AddEdge);
            base.BuildContextualMenu(evt);
        }

        private void AddEdge(DropdownMenuAction obj)
        {
            //this.outPort.
        }

        /// <summary>
        /// 添加自己的样式
        /// </summary>
        /// <returns></returns>
        private string GetSelfClass()
        {
            if (Application.isPlaying && this.Context.FSMController != null && this.Context.FSMController.currentState != null && this.Context.FSMController.currentState.stateNodeData.name == nodeData.name)
            {
                return Styles.Orange.ToString();
            }
            else if (!Application.isPlaying && nodeData.defualtState)
            {
                return Styles.Orange.ToString();
            }
            else if (nodeData.name == FSMConst.enterState)
            {
                return Styles.Green.ToString();
            }
            else if (nodeData.name == FSMConst.anyState)
            {
                return Styles.Miut.ToString();
            }
            else
            {
                return Styles.Normal.ToString();
            }
        }

        /// <summary>
        /// 创建output
        /// </summary>
        private void CreateOutPut()
        {
            outPort = Port.Create<TraslationEdgeView>(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outPort.portName = "";
            outPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(outPort);
        }

        /// <summary>
        /// 创建Input
        /// </summary>
        private void CreateInput()
        {
            inputPort = Port.Create<TraslationEdgeView>(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "";
            inputPort.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(inputPort);
        }

        /// <summary>
        /// 设置位置时调用
        /// </summary>
        /// <param name="newPos"></param>
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            nodeData.rect.x = newPos.x / widthScale;
            nodeData.rect.y = newPos.y / heightScale;
        }

        /// <summary>
        /// 被选中
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            FSMStateInspectorHelper.Instance.Inspector(this.Context.RunTimeFSMContorller, nodeData);
        }
    }
}
