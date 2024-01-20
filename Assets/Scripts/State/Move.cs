using AE_FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IFSMState
{
    public void Enter(FSMController controller)
    {
        Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "Enter");

    }
    public void Update(FSMController controller)
    {
       
    }
    public void LaterUpdate(FSMController controller)
    {
    }
    public void FixUpdate(FSMController controller)
    {
    }
    public void Exit(FSMController controller)
    {
    }
}
