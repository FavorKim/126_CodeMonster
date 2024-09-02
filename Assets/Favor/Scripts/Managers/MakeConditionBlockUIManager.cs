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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InitConditionBlockInfo();
        }
    }

    protected override void Start()
    {
        makeBtn.OnPoke += InitConditionBlockInfo;
        conditionBlockInfo = Prefab_ConditionBlock;
        base.Start();
    }

    private void OnApplicationQuit()
    {
        makeBtn.OnPoke -= InitConditionBlockInfo;
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
        DebugBoxManager.Instance.Log("InitConditionBlockInfo");
        conditionBlockInfo.InitConditionBlock(trueBlock, falseBlock, drop.GetSelectedValue());
        UIManager.Instance.MakeConditionalBlockBoxEnable();

        if (trueBlock != null && falseBlock != null)
            DebugBoxManager.Instance.Log($"참 블록 : {trueBlock.name} \n 거짓 블록 : {falseBlock.name}");
        else
        {
            DebugBoxManager.Instance.Log("참 혹은 거짓 블록 할당 x");
        }
        
    }
    // 내부에 저장된 컨디션 블록 정보 반환
    public ConditionBlock GetConditionBlockInfo()
    {
        return conditionBlockInfo;
    }
}
