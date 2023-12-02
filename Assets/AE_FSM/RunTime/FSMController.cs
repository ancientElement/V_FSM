using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.Progress;

namespace AE_FSM
{
    public class FSMController : MonoBehaviour
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        [SerializeField] private RunTimeFSMController runTimeFSMController;
        private RunTimeFSMController _runTimeFSMController;

        /// <summary>
        /// 驱动器
        /// </summary>
        internal IExcuteState excuteState { get; private set; } = new DefualtIExcuteState();

        /// <summary>
        /// 设置驱动
        /// </summary>
        /// <param name="_excuteState"></param>
        public void SetIExcuteState(IExcuteState _excuteState)
        {
            if (excuteState != null)
            {
                excuteState = _excuteState;
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        internal Dictionary<string, FSMParameterData> parameters = new Dictionary<string, FSMParameterData>();
        /// <summary>
        /// 状态
        /// </summary>
        internal Dictionary<string, FSMStateNode> states = new Dictionary<string, FSMStateNode>();
        /// <summary>
        /// 过渡
        /// </summary>
        internal List<FSMTransition> transitions = new List<FSMTransition>();

        /// <summary>
        /// 默认状态
        /// </summary>
        internal FSMStateNode defaultState { get; private set; } = null;
        /// <summary>
        /// 当前状态
        /// </summary>
        public FSMStateNode currentState { get; private set; } = null;

        /// <summary>
        /// 退出时间
        /// </summary>
        [Range(0.05f, 10f)] public float exitTime = 0.1f;
        /// <summary>
        /// 退出状态
        /// </summary>
        public bool isExitingState;

        /// <summary>
        /// 运行时 和 _ 的配置文件
        /// </summary>
        public RunTimeFSMController RunTimeFSMController
        {
            get
            {
                if (Application.isPlaying)
                { return _runTimeFSMController; }
                return runTimeFSMController;
            }
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (currentState != null) { currentState.Update(); }
        }

        private void LateUpdate()
        {
            if (currentState != null) { currentState.LateUpdate(); }
        }

        private void FixedUpdate()
        {
            if (currentState != null) { currentState.FixUpdate(); }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            //复制数据
            if (runTimeFSMController == null) return;

            _runTimeFSMController = GameObject.Instantiate(runTimeFSMController);

            //初始化参数

            foreach (FSMParameterData item in _runTimeFSMController.paramters)
            {
                if (parameters.ContainsKey(item.name))
                {
                    Debug.LogWarning($"参数{item.name}已存在!!!");
                    continue;
                }
                item.onValueChage = null;
                parameters.Add(item.name, item);
            }

            //状态
            for (int i = 0; i < _runTimeFSMController.states.Count; i++)
            {
                FSMStateNodeData nodeData = _runTimeFSMController.states[i];
                FSMStateNode stateNode = new FSMStateNode(nodeData, this);

                if (states.ContainsKey(nodeData.name))
                {
                    Debug.LogWarning($"状态重复{nodeData.name}");
                    continue;
                }

                if (nodeData.defualtState == true)
                {
                    defaultState = stateNode;
                }
                states.Add(nodeData.name, stateNode);
            }

            //过渡

            foreach (var item in _runTimeFSMController.trasitions)
            {
                FSMTransition transition = new FSMTransition(item, this);
                transitions.Add(transition);
            }

            //切换到默认状态
            SwitchState(defaultState);
        }

        /// <summary>
        /// 判断是否需要过渡
        /// </summary>
        public void CheckTransfrom()
        {
            foreach (FSMTransition item in transitions)
            {
                if (item.translationData.fromState == currentState.stateNodeData.name || item.translationData.fromState == FSMConst.anyState)
                {
                    item.CheckAllConditionMeet();
                }
            }
        }

        #region 设置参数

        public void SetBool(string paramterName, bool value)
        {
            SetParams(paramterName, value ? 1 : 0, ParamterType.Bool);
        }

        public void SetFloat(string paramterName, float value)
        {
            SetParams(paramterName, value, ParamterType.Float);
        }

        public void SetInt(string paramterName, int value)
        {
            SetParams(paramterName, value, ParamterType.Int);
        }

        private void SetParams(string paramterName, float value, ParamterType paramterType)
        {
            if (parameters.TryGetValue(paramterName, out FSMParameterData paramterData))
            {
                if (paramterData.paramterType == paramterType)
                {
                    paramterData.Value = value;
                }
                else
                {
                    Debug.LogWarning($"参数{paramterName}类型错误:{paramterType}");
                }
            }
            else
            {
                Debug.LogWarning($"参数{paramterName}不存在!!!");
            }
        }

        #endregion

        /// <summary>
        /// 有退出时间的切换
        /// </summary>
        /// <param name="stateNode"></param>
        public void WaitSwitchState(FSMStateNode stateNode)
        {
            isExitingState = true;
            StopAllCoroutines();
            StartCoroutine(DoWaitSwitchState(stateNode));
        }
        private IEnumerator DoWaitSwitchState(FSMStateNode stateNode)
        {
            yield return new WaitForSeconds(exitTime);
            SwitchState(stateNode);
            isExitingState = false;
            yield break;
        }
        public void WaitSwitchState(string state)
        {
            WaitSwitchState(states[state]);
        }

        /// <summary>
        /// 直接切换
        /// </summary>
        /// <param name="stateNode"></param>
        public void SwitchState(FSMStateNode stateNode, bool toself = false)
        {
            if (!toself)
                if (currentState == stateNode) return;

            if (stateNode == null) return;

            if (currentState != null)
                currentState.Exit();

            currentState = stateNode;

            currentState.Enter();
        }
        public void SwitchState(string state, bool toself = false)
        {
            SwitchState(states[state], toself);
        }
    }
}