using AE_FSM;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AE_FSMGV
{
    public class TraslationEdgeView : Edge
    {
        public FSMTranslationData translationData;

        public TraslationEdgeView()
        {

        }

        public TraslationEdgeView(FSMTranslationData translationData)
        {
            this.translationData = translationData;
        }

        /// <summary>
        /// 更新线条时调用
        /// </summary>
        /// <returns></returns>
        public override bool UpdateEdgeControl()
        {
            if (this.output == null || this.input == null) return base.UpdateEdgeControl();

            if (edgeControl.from == null || edgeControl.to == null) { return base.UpdateEdgeControl(); }
            if (edgeControl.controlPoints == null) { return base.UpdateEdgeControl(); }

            Debug.Log(edgeControl.from);
            edgeControl.controlPoints[0] = edgeControl.from;
            edgeControl.controlPoints[1] = edgeControl.from;
            edgeControl.controlPoints[2] = edgeControl.to;
            edgeControl.controlPoints[3] = edgeControl.to;

            return base.UpdateEdgeControl();
        }

        protected override EdgeControl CreateEdgeControl()
        {
            return new TranslationEdgeController();
        }

        /// <summary>
        /// 被选中
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            FSMTranslationInspectorHelper.Instance.Inspector((this.output.node as StateNodeView).Context.RunTimeFSMContorller, translationData);
        }

        /// <summary>
        /// 能否被选中
        /// </summary>
        public override bool IsSelectable()
        {
            if ((this.input != null && this.output != null))
            {
                if ((this.input.node as StateNodeView).nodeData.defualtState && (this.output.node as StateNodeView).nodeData.name == FSMConst.enterState)
                {
                    return false;
                }
            }
            return base.IsSelectable();
        }
    }
}
