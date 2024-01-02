using AE_FSM;
using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] FSMController controller;
    [SerializeField,Range(0.001f,10f)] float test;

    private void Update()
    {
        controller.SetFloat("New Float_0", test);
    }

    private IEnumerator WaitForAttack(float v)
    {
        yield return new WaitForSeconds(v);
        print("攻击完毕");
        yield break;
    }
}
