using AE_FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AE_FSMGV
{
    public class StateAreaGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<StateAreaGraphView, GraphView.UxmlTraits> { }

        public AE_FSMGVWindow FSMEditorWindowGV;
        public Context Context => FSMEditorWindowGV.Context;

        public StateAreaGraphView()
        {
            //背景
            Insert(0, new GridBackground());
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AE_FSMGV/Editor/Window/AE_FSMGVWindow.uss");

            //缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentZoomer()); //缩放
            this.AddManipulator(new ContentDragger());//拖拽
            this.AddManipulator(new SelectionDragger()); //选择拖拽
            this.AddManipulator(new RectangleSelector());//矩形选择

            styleSheets.Add(styleSheet);
        }

        public void Init(AE_FSMGVWindow FSMEditorWindowGV)
        {
            this.FSMEditorWindowGV = FSMEditorWindowGV;

            if (this.Context.RunTimeFSMContorller == null)
                return;

            nodeCreationRequest -= CreateStateNode;

            graphViewChanged -= WhenGraphViewChanged;

            DestoryAllNodeView();

            ResetView();

            nodeCreationRequest += CreateStateNode;

            graphViewChanged += WhenGraphViewChanged;

            RegisterCallback<DragExitedEvent>(ApplyDragState);
            RegisterCallback<DragUpdatedEvent>(DragingState);
        }

        bool canDragObject;
        Vector2 mousePosition;

        /// <summary>
        /// 拖拽资源成功
        /// </summary>
        /// <param name="evt"></param>
        private void ApplyDragState(DragExitedEvent evt)
        {
            UnityEngine.Object[] objs = DragAndDrop.objectReferences;
            mousePosition = Event.current.mousePosition;
            foreach (var item in objs)
            {
                CreateStateNode((item as MonoScript).GetClass().FullName);
                mousePosition.x += 30;
                mousePosition.y += 30;
            }
        }

        /// <summary>
        /// 拖拽资源到面板上
        /// </summary>
        /// <param name="evt"></param>
        private void DragingState(DragUpdatedEvent evt)
        {
            UnityEngine.Object[] objs = DragAndDrop.objectReferences;
            canDragObject = true;
            foreach (var item in objs)
            {
                if ((item as MonoScript).GetClass().GetInterfaces().Where(x => x == typeof(IFSMState)).FirstOrDefault() == null)
                {
                    canDragObject = false;
                }
            }
            if (canDragObject)
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }

        /// <summary>
        /// node 和 edge 的增删改查
        /// </summary>
        /// <param name="graphViewChange"></param>
        /// <returns></returns>
        private GraphViewChange WhenGraphViewChanged(GraphViewChange graphViewChange)
        {
            //创建连接
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge item in graphViewChange.edgesToCreate)
                {
                    string from = ((item as TraslationEdgeView).output.node as StateNodeView).nodeData.name;
                    string to = ((item as TraslationEdgeView).input.node as StateNodeView).nodeData.name;
                    (item as TraslationEdgeView).translationData = FSMTranslationFactory.CreateTransition(this.Context.RunTimeFSMContorller, from, to);
                }
            }

            //删除
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var item in graphViewChange.elementsToRemove)
                {
                    //删除创建连接
                    if ((item as TraslationEdgeView) != null)
                    {
                        FSMTranslationFactory.DeleteTransition(this.Context.RunTimeFSMContorller, (item as TraslationEdgeView).translationData);
                    }
                    //删除node
                    if ((item as StateNodeView) != null)
                    {
                        if (!FSMStateNodeFactory.DeleteFSMNode(this.Context.RunTimeFSMContorller, (item as StateNodeView).nodeData))
                        {
                            DrawNode((item as StateNodeView).nodeData);
                        };
                    }
                }
            }
            return graphViewChange;
        }

        /// <summary>
        /// 删除所有元素
        /// </summary>
        private void DestoryAllNodeView()
        {
            DeleteElements(graphElements);
        }

        /// <summary>
        /// 创建右键菜单
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
        }

        /// <summary>
        /// 创建一个Node 并且 绘制 
        /// </summary>
        /// <param name="obj"></param>
        private void CreateStateNode(NodeCreationContext obj)
        {
            var rect = new Rect(0, 0, FSMConst.stateWidth, FSMConst.stateHeight);
            rect.center = obj.screenMousePosition;
            var stateNode = FSMStateNodeFactory.CreateFSMNode(this.Context.RunTimeFSMContorller, this.Context.RunTimeFSMContorller.states.Count == 2, rect);
            DrawNode(stateNode);
        }

        /// <summary>
        /// 创建一个Node 并且 绘制 带script
        /// </summary>
        /// <param name="scriptName"></param>
        private void CreateStateNode(string scriptName)
        {
            var rect = new Rect(0, 0, FSMConst.stateWidth, FSMConst.stateHeight);
            rect.center = mousePosition;
            var stateNode = FSMStateNodeFactory.CreateFSMNode(this.Context.RunTimeFSMContorller, scriptName, this.Context.RunTimeFSMContorller.states.Count == 2, rect, scriptName);
            DrawNode(stateNode);
        }

        /// <summary>
        /// 刷新所有视图
        /// </summary>
        private void ResetView()
        {
            for (int i = 0; i < Context.RunTimeFSMContorller.states.Count; i++)
            {
                DrawNode(Context.RunTimeFSMContorller.states[i]);
            }

            //默认状态

            if (this.Context.RunTimeFSMContorller.states.Where(x => x.defualtState).FirstOrDefault() != null)
            {
                string defualtState = this.Context.RunTimeFSMContorller.states.Where(x => x.defualtState).FirstOrDefault().name;
                DrawEdge(FSMConst.enterState, defualtState, null);
            }

            foreach (var item in this.Context.RunTimeFSMContorller.trasitions)
            {
                DrawEdge(item.fromState, item.toState, item);
            }
        }

        /// <summary>
        /// 绘制一个Node
        /// </summary>
        /// <param name="nodeData"></param>
        private void DrawNode(FSMStateNodeData nodeData)
        {
            StateNodeView ndoeView = new StateNodeView(nodeData, this.FSMEditorWindowGV);
            AddElement(ndoeView);
        }

        /// <summary>
        /// 绘制连接
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        private void DrawEdge(string fromState, string toState, FSMTranslationData translationData)
        {
            StateNodeView fromNode = this.nodes.Where(x => (x as StateNodeView).nodeData.name == fromState).FirstOrDefault() as StateNodeView;

            StateNodeView toNode = this.nodes.Where(x => (x as StateNodeView).nodeData.name == toState).FirstOrDefault() as StateNodeView;

            DrawEdge(fromNode, toNode, translationData);
        }

        /// <summary>
        /// 绘制连接
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="toState"></param>
        private void DrawEdge(StateNodeView fromNode, StateNodeView toNode, FSMTranslationData translationData)
        {
            TraslationEdgeView tempEdge = new TraslationEdgeView(translationData)
            {
                output = fromNode.outPort,
                input = toNode.inputPort,
            };

            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            Add(tempEdge);
        }

        /// <summary>
        /// 限制连接
        /// </summary>
        /// <param name="startAnchor"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            List<Port> ports = new List<Port>();

            foreach (Port endPort in base.ports)
            {
                StateNodeView tempNode = endPort.node as StateNodeView;
                //enter状态不连接
                if (tempNode.nodeData.name == FSMConst.enterState && endPort.direction == Direction.Input)
                {
                    continue;
                }
                //同一个Node不连接
                if (startAnchor.node == endPort.node)
                {
                    continue;
                }

                if (startAnchor.direction == endPort.direction ||
                startAnchor.portType != endPort.portType)
                {
                    continue;
                }

                if (tempNode.nodeData.name == FSMConst.anyState && endPort.direction == Direction.Input)
                {
                    continue;
                }

                if ((startAnchor.node as StateNodeView).nodeData.name == FSMConst.enterState)
                {
                    continue;
                }

                ports.Add(endPort);
            }

            return ports;
        }
    }
}
