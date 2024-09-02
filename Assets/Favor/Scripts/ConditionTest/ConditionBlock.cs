using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBlock : MonoBehaviour
{
    public CodeBlockDrag TrueBlock { get; private set; }
    public CodeBlockDrag FalseBlock { get; private set; }
    public string Condition {  get; private set; }
    CustomGrabObject grab;

    private void Start()
    {
        grab = GetComponent<CustomGrabObject>();
        grab.OnGrab += OnGrabSetData;
        grab.OnGrab += () => { DebugBoxManager.Instance.Log(Condition); };
    }

    private void OnApplicationQuit()
    {
        grab.OnGrab -= () => { DebugBoxManager.Instance.Log(Condition); };
        grab.OnGrab -= OnGrabSetData;
    }

    // 조건 블록 정보 초기화
    public void InitConditionBlock(CodeBlockDrag trueBlock, CodeBlockDrag falseBlock, string condition)
    {
        this.Condition = condition;
        this.TrueBlock = trueBlock;
        this.FalseBlock = falseBlock;
    }
    public void InitConditionBlock(ConditionBlock dest)
    {
        this.Condition = dest.Condition;
        this.TrueBlock = dest.TrueBlock;
        this.FalseBlock = dest.FalseBlock;
    }

    // 블록을 잡을 때마다 저장되어있는 정보로 본인의 정보를 초기화
    private void OnGrabSetData()
    {
        InitConditionBlock(MakeConditionBlockUIManager.Instance.GetConditionBlockInfo());
    }


}
