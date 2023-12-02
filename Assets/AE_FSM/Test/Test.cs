using AE_FSM;
using System;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] float attackTime = 0.5f;
    [SerializeField] FSMController controller;

    private void Update()
    {
      
    }

    private IEnumerator WaitForAttack(float v)
    {
        yield return new WaitForSeconds(v);
        print("攻击完毕");
        yield break;
    }
}
