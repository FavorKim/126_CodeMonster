using Oculus.Interaction.HandGrab;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject MonsterAttributeBox;
    [SerializeField] private List<GameObject> selectedMonsters = new List<GameObject>();
    [SerializeField] GameObject CharacterContainer;
    [SerializeField] CustomPokedObject Btn_StartStage;



    private void OnEnable()
    {
        Btn_StartStage.DisablePokeBtn();
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

        if (monsterTypeIndices == null)
        {
            Debug.LogError("monsterTypeIndices is NULL!");
            return;
        }
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

    public List<GameObject> GetSelectedMonsters()
    {
        return selectedMonsters;
    }
    public void AddMonster(GameObject monster)
    {
        if (monster == null)
        {
            DebugBoxManager.Instance.Log("AddmOnster NULL");
            return;
        }
        if (!selectedMonsters.Contains(monster))
        {
            //FieldManager.Instance.TeleportMonstersToTargetPositions();
            monster.transform.SetParent(CharacterContainer.transform, false);
            SetMonsterList();
            //monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = new Vector3(70, 70, 70);
            monster.transform.localRotation = Quaternion.Euler(new Vector3(0, 120, 0));
            DebugBoxManager.Instance.Log("캐릭터 등록");
            CheckCanStart();
            //디버깅 할 수 없는 버그로 인해 사용 불가 (예컨대 sdk쪽 오류일 가능성 높음)
        }
        else
        {
            DebugBoxManager.Instance.Log("추가하려는 몬스터가 이미 리스트 내에 존재합니다.");
        }

    }
    public void RemoveMonster(GameObject monster)
    {
        if (monster == null)
        {
            DebugBoxManager.Instance.Log("RemoveMonster NULL");
            return;
        }
        if (selectedMonsters.Contains(monster))
        {
            monster.transform.SetParent(null);
            SetMonsterList();
            //FieldManager.Instance.TeleportMonstersToTargetPositions();
            //monster.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            DebugBoxManager.Instance.Log("캐릭터 등록 해제");
            CheckCanStart();
        }
        else
        {
            DebugBoxManager.Instance.Log("삭제하려는 몬스터가 리스트내에 없습니다.");
        }
    }

    public void RemoveAllMonsters()
    {
        foreach (Transform t in CharacterContainer.transform)
        {
            t.SetParent(null);
            t.localScale = Vector3.one;
        }
    }
    private void SetMonsterList()
    {
        selectedMonsters.Clear();
        for (int i = 0; i < CharacterContainer.transform.childCount; i++)
        {
            selectedMonsters.Add(CharacterContainer.transform.GetChild(i).gameObject);
        }
    }

    private void CheckCanStart()
    {
        List<Monster> monsters = new List<Monster>();
        foreach (GameObject mon in selectedMonsters)
        {
            Monster monster = DataManagerTest.Instance.GetMonsterData(mon.name);
            monsters.Add(monster);
        }
        int stageIndex = UIManager.Instance.SelectChapterNum + UIManager.Instance.SelectStageNum;
        List<int> stageMonsterIndexes = GameManager.Instance.GetMonsterTypeInIndex(stageIndex);

        List<int> weaks = new List<int>();
        foreach(int monIndex in stageMonsterIndexes)
        {
            int weakIndex = DataManagerTest.Instance.GetWeaknessIndexByMonsterTypeIndex(monIndex);
            if (weakIndex != 0)
                weaks.Add(weakIndex);
        }

        List<int> typeIndexes = new List<int>();
        foreach (Monster mon in monsters)
        {
            int index = mon.TypeIndex;
            typeIndexes.Add(index);
        }
        int temp = 0;
        foreach(int weak in weaks)
        {
            if (typeIndexes.Contains(weak) == false)
            {
                DebugBoxManager.Instance.Log($"{weak}번 타입 인덱스 없음");
                Btn_StartStage.DisablePokeBtn();
                temp++;
            }
        }
        if(temp == 0)
        Btn_StartStage.EnablePokeBtn();
    }
}
