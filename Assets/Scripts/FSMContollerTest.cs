using System;
using AE_FSM;
using UnityEngine;

public class FSMContollerTest : MonoBehaviour
{
    // [SerializeField] float attackTime = 0.5f;
    [SerializeField] FSMController controller;
    [SerializeField, Range(0f, 10f)] float Paramter1_f;
    [SerializeField] bool Paramter1_b;

    [SerializeField] string Paramter1;
    [SerializeField] string Paramter2;

    private void Start()
    {
        controller.Init();
    }

    private void Update()
    {
        controller.SetFloat(Paramter1, Paramter1_f);
        controller.SetBool(Paramter2, Paramter1_b);
    }
}
