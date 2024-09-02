using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeConditionBlockUIManager : Singleton<MakeConditionBlockUIManager>
{
    [SerializeField] Transform trueBlockPos;
    [SerializeField] Transform falseBlockPos;
    [SerializeField] PokeDropdown drop;
    [SerializeField] ConditionBlock Prefab_ConditionBlock;
    [SerializeField] CustomPokedObject makeBtn;

    private ConditionBlock conditionBlockInfo;

    public CodeBlockDrag trueBlock
    {
        get; private set;
    }

    public CodeBlockDrag falseBlock
    {
        get;
        private set;
    }

    protected override void Start()
    {
        base.Start();
        makeBtn.OnPoke += InitConditionBlockInfo;
    }

    private void OnApplicationQuit()
    {
        makeBtn.OnPoke -= InitConditionBlockInfo;
    }

    // 조건 블록 위치 정렬
    public void SetConditionBlockPos(CodeBlockDrag block, bool trueorFalse)
    {
        CodeBlockDrag dest = trueorFalse ? trueBlock : falseBlock;
        Transform destPos = trueorFalse ? trueBlockPos : falseBlockPos;
        if (dest != null)
            dest.ReturnToPool();
        dest = block;
        dest.transform.SetParent(destPos, false);
        Vector3 newPos = dest.transform.localPosition;
        newPos.z = 0;
        dest.transform.localPosition = newPos;
    }

    // 내부에 제작한 컨디션 블록 정보 저장
    public void InitConditionBlockInfo()
    {
        conditionBlockInfo.InitConditionBlock(trueBlock, falseBlock, drop.GetSelectedValue());
        UIManager.Instance.MakeConditionalBlockBoxEnable();
    }
    // 내부에 저장된 컨디션 블록 정보 반환
    public ConditionBlock GetConditionBlockInfo()
    {
        return conditionBlockInfo;
    }
}
