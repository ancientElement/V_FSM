using AE_FSM;
using UnityEngine;

public class Idle : IFSMState
{
    public void Enter(FSMController controller)
    {
        Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "Enter");
    }

    public void Exit(FSMController controller)
    {
    }

    public void FixUpdate(FSMController controller)
    {
    }

    public void LaterUpdate(FSMController controller)
    {
    }

    public void Update(FSMController controller)
    {
    }
}
