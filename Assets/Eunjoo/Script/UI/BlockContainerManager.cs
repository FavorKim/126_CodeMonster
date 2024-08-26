using EnumTypes;
using EventLibrary;
using System.Collections.Generic;
using UnityEngine;


public class BlockContainerManager : Singleton<BlockContainerManager>
{
    [SerializeField] private RectTransform BlockContainerUIRectTransform;
    private BoxCollider BlockContainerBoxCollider;
    List<MaterialChanger> materialChangers = new();

    protected void Awake()
    {
        //base.Awake();
        BlockContainerUIRectTransform = GetComponent<RectTransform>();
        BlockContainerBoxCollider = GetComponent<BoxCollider>();
        InteractEventManager.Instance.RegistOnClickResetBtn(ResetBlockContainer);
        InteractEventManager.Instance.RegistOnClickRestartBtn(ResetBlockContainer);
        InteractEventManager.Instance.RegistOnClickPauseBtn(ResetContainerBlockMaterial);
    }

    public void SetBlockContainerUISize(int BlockContainerLength, bool PlusContainerUI)
    {
        if (PlusContainerUI == true)
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.LOOP_CON_BLOCK_SIZE, UIConstants.ATTACK_MOVE_BLOCK_SIZE);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.LOOP_CON_BLOCK_SIZE, UIConstants.ATTACK_MOVE_BLOCK_SIZE);
        }
        else
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.CONTAINER_WIDTH_SIZE, UIConstants.CONTAINER_HEIGHT_SIZE);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.CONTAINER_WIDTH_SIZE, UIConstants.CONTAINER_HEIGHT_SIZE);
        }
    }

    public void AddBlock(GameObject newBlock)
    {
        // 기존 블록들 existingBlocks에 저장
        List<Transform> existingBlocks = new List<Transform>();

        foreach (Transform child in this.transform)
        {
            existingBlocks.Add(child);
        }

        // 새로 추가될 블록의 월드 좌표
        Vector3 newBlockWorldPosition = newBlock.transform.position;

        // existingBlocks에 있는 블럭들과 새 블록의 월드 좌표를 비교해서
        // 순서대로 sortedBlocks List에 저장
        List<Transform> sortedBlocks = new List<Transform>();

        bool blockInserted = false;
        foreach (Transform block in existingBlocks)
        {
            if (!blockInserted && newBlockWorldPosition.x < block.position.x)
            {
                sortedBlocks.Add(newBlock.transform);
                blockInserted = true;
            }
            sortedBlocks.Add(block);
        }

        // 새 블록이 제일 마지막에 위치할 경우
        if (!blockInserted)
        {
            sortedBlocks.Add(newBlock.transform);
        }

        // sortedBlock 순대로 BlockContainer UI 하위로 정렬
        for (int i = 0; i < sortedBlocks.Count; i++)
        {
            sortedBlocks[i].SetParent(this.transform, false);
            // Z 좌표를 0으로 설정
            Vector3 newPosition = sortedBlocks[i].localPosition;
            newPosition.z = 0;
            sortedBlocks[i].localPosition = newPosition;
            sortedBlocks[i].SetSiblingIndex(i);
            sortedBlocks[i].rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        }
        EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCount, UIManager.Instance.BlockContainerLength - sortedBlocks.Count);
    }

    

    // 컨테이너에 있던 블럭들 리셋
    public void ResetBlockContainer()
    {
        // 현재 컨테이너 하위에 있는 모든 자식 객체를 리스트에 저장
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        // 저장한 리스트를 순회하면서 블록들을 리셋
        foreach (Transform child in children)
        {
            CodeBlockDrag codeBlockDrag = child.GetComponent<CodeBlockDrag>();


            if (codeBlockDrag != null)
            {
                codeBlockDrag.ReturnToPool(); // 블록을 풀로 반환
            }
        }
    }

    void ResetContainerBlockMaterial()
    {
        GetContatinerBlocks();
        foreach(MaterialChanger mat in materialChangers)
        {
            mat.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
        }
    }

    public List<int> GetContatinerBlocks()
    {

        if (this.transform.childCount <= 0)
        {
            return null;
        }

        List<int> list = new List<int>();
        materialChangers.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            int blockIndex = DataManagerTest.Instance.GetCodeBlockData(this.transform.GetChild(i).name).BlockIndex;
            MaterialChanger mC = transform.GetChild(i).GetComponent<MaterialChanger>();
            materialChangers.Add(mC);
            list.Add(blockIndex);
            if(i>=6)
            {
                //DebugBoxManager.Instance.Log(transform.GetChild(i).name);
            }
        }

        return list;
    }

    public void SetBlockMaterial(int index, MaterialType type)
    {
        materialChangers[index].ChangeMaterial(type);
    }
    public int CountCodeBlockDragComponents()
    {
        int count = 0;

        // 하위 객체들을 순회하면서 CodeBlockDrag 컴포넌트를 가진 객체의 수를 카운트
        foreach (Transform child in transform)
        {
            CodeBlockDrag codeBlockDrag = child.GetComponent<CodeBlockDrag>();
            if (codeBlockDrag != null)
            {
                count++;
            }
        }
        return count;
    }   
}

