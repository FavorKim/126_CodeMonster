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
        makeBtn.OnPoke.AddListener(InitConditionBlockInfo);
        conditionBlockInfo = Prefab_ConditionBlock;
        base.Start();

        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.CONDITIONRESET, ResetConditionContainer);
    }

    private void OnApplicationQuit()
    {
        makeBtn.OnPoke.RemoveAllListeners();// -= InitConditionBlockInfo;
    }

    // 조건 블록 위치 정렬
    public void SetConditionBlockPos(CodeBlockDrag block, bool trueorFalse)
    {
        Transform destPos = trueorFalse ? trueBlockPos : falseBlockPos;

        if (trueorFalse == true)
        {
            if (trueBlock != null)
                trueBlock.ReturnToPool();
            trueBlock = block;
            trueBlock.transform.SetParent(destPos, false);
            Vector3 newPos = trueBlock.transform.localPosition;
            newPos.z = 0;
            trueBlock.transform.localPosition = newPos;
        }
        else
        {
            if (falseBlock != null)
                falseBlock.ReturnToPool();
            falseBlock = block;
            falseBlock.transform.SetParent(destPos, false);
            Vector3 newPos = falseBlock.transform.localPosition;
            newPos.z = 0;
            falseBlock.transform.localPosition = newPos;
        }
    }

    // 내부에 제작한 컨디션 블록 정보 저장
    public void InitConditionBlockInfo()
    {
        conditionBlockInfo.InitConditionBlock(trueBlock, falseBlock, drop.GetSelectedValue() + 5);
        UIManager.Instance.MakeConditionalBlockBoxEnable();
    }
    // 내부에 저장된 컨디션 블록 정보 반환
    public ConditionBlock GetConditionBlockInfo()
    {
        return conditionBlockInfo;
    }

    private void ResetConditionContainer()
    {
        trueBlock?.ReturnToPool();
        falseBlock?.ReturnToPool();
        trueBlock = null;
        falseBlock = null;
        UIManager.Instance.MakeConditionalBlockBoxDisable();
    }
}
