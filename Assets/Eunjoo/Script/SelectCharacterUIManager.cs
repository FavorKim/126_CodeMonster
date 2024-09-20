using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject MonsterAttributeBox;
    private List<GameObject> monsters = new List<GameObject>();
    [SerializeField] GameObject CharacterContainer;

    public void AddMonster(GameObject monster)
    {
        if (!monsters.Contains(monster))
        {
            monsters.Add(monster);
            FieldManager.Instance.TeleportMonstersToTargetPositions();
            monster.transform.SetParent(CharacterContainer.transform, false);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(50, 50, 50);
        }
        else
        {
            DebugBoxManager.Instance.Log("추가하려는 몬스터가 이미 리스트 내에 존재합니다.");
        }
        
    }
    public void RemoveMonster(GameObject monster)
    {
        if (monsters.Contains(monster))
        {
            monsters.Remove(monster);
            monster.transform.SetParent(null);
            FieldManager.Instance.TeleportMonstersToTargetPositions();
            monster.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            DebugBoxManager.Instance.Log("삭제하려는 몬스터가 리스트내에 없습니다.");
        }
    }


    private void OnEnable()
    {
        CheckMonsterAttributes();
    }

    private void OnDisable()
    {
        ResetBlock();
    }

    public void CheckMonsterAttributes()
    {
        // 몬스터 타입 인덱스 리스트를 가져옴
        List<int> monsterTypeIndices = GameManager.Instance.GetMonsterTypeInIndex(UIManager.Instance.SelectChapterNum + UIManager.Instance.SelectStageNum);

        // 리스트에서 몬스터 타입을 확인
        foreach (int monsterType in monsterTypeIndices)
        {
            DebugBoxManager.Instance.Log($"{monsterType}");
            switch (monsterType)
            {
                case 1:
                    SetupMonsterAttribute(BlockName.FireAttackCodeBlock);
                    break;
                case 2:
                    SetupMonsterAttribute(BlockName.WaterAttackCodeBlock);
                    break;
                case 3:
                    SetupMonsterAttribute(BlockName.GrassAttackCodeBlock);
                    break;
                default:
                    break;
            }
        }
    }

    private void SetupMonsterAttribute(BlockName blockName)
    {
        // 공통 로직: 오브젝트 가져오기 및 설정
        GameObject attackCodeBlock = ObjectPoolManager.Instance.GetObject(blockName);
        HandGrabInteractable attackCodeBlockHandGrab = attackCodeBlock.GetComponent<HandGrabInteractable>();
        BoxCollider attackCodeBlockBoxCollider = attackCodeBlock.GetComponent<BoxCollider>();

        attackCodeBlockHandGrab.enabled = false;
        attackCodeBlockBoxCollider.enabled = false;
        attackCodeBlockHandGrab.transform.SetParent(MonsterAttributeBox.transform, false);
        attackCodeBlockHandGrab.transform.localScale = new Vector3(30, 30, 30);
    }

    public void ResetBlock()
    {
        // 현재 컨테이너 하위에 있는 모든 자식 객체를 리스트에 저장
        List<Transform> children = new List<Transform>();
        foreach (Transform child in MonsterAttributeBox.transform)
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
}
