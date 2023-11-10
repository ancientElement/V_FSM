using UnityEngine;

namespace AE_FSM
{
    public class FSMStateNode
    {
        public FSMStateNodeData stateNodeData;
        public FSMController controller;

        public FSMStateNode(FSMStateNodeData stateNodeData, FSMController controller)
        {
            this.stateNodeData = stateNodeData;
            this.controller = controller;
        }

        public void Enter()
        {
            controller.excuteState.Enter(this);
            controller.CheckTransfrom();
        }
        public void Update()
        {
            controller.excuteState.Update(this);
        }
        public void LateUpdate()
        {
            controller.excuteState.LaterUpdate(this);
        }
        public void FixUpdate()
        {
            controller.excuteState.FixUpdate(this);
        }
        public void Exit()
        {
            controller.excuteState.Exit(this);
        }
    }
}