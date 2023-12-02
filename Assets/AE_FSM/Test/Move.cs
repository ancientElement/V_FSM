using AE_FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IFSMState
{
    public void Enter(FSMController controller)
    {
        Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "Enter");
        Debug.Log(controller.transform.position);
    }
    public void Update(FSMController controller)
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            controller.SwitchState("Idle");
        }
        if (Input.GetMouseButton(0))
        {
            controller.SwitchState("Attack");
        }
    }
    public void LaterUpdate(FSMController controller)
    {
        //Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "LaterUpdate");
    }
    public void FixUpdate(FSMController controller)
    {
        //Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "FixUpdate");
    }
    public void Exit(FSMController controller)
    {
        Debug.Log(this.GetType().ToString() + controller.gameObject.name + "sss" + "Exit");
    }
}
