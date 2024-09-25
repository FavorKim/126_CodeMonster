using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
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
        if(monster == null)
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
            //CheckCanStart(); 디버깅 할 수 없는 버그로 인해 사용 불가 (예컨대 sdk쪽 오류일 가능성 높음)
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
            //CheckCanStart();
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

        if (GameManager.Instance == null || UIManager.Instance == null)
        {
            DebugBoxManager.Instance.Log("GameManager 또는 UIManager가 null입니다.");
            return;
        }

        if (DataManagerTest.Instance == null)
        {
            DebugBoxManager.Instance.Log("DataManagerTest가 null입니다.");
            return;
        }

        if (selectedMonsters == null || selectedMonsters.Count == 0)
        {
            DebugBoxManager.Instance.Log("selectedMonsters가 null이거나 Count가 0입니다.");
            return;
        }

        List<int> stageMonTypes = new List<int>();
        stageMonTypes = GameManager.Instance.GetMonsterTypeInIndex(UIManager.Instance.SelectChapterNum + UIManager.Instance.SelectStageNum);

        List<int> weaknesses = new List<int>();
        foreach (int type in stageMonTypes)
        {
            weaknesses.Add(DataManagerTest.Instance.GetWeaknessIndexByTypeIndex(type));

        }

        List<Monster> monsters = new List<Monster>();
        if (selectedMonsters.Count == 0) { DebugBoxManager.Instance.Log("셀릭티드몬스터 카운트 0");  return; }
        foreach (GameObject monster in selectedMonsters)
        {
            Monster mon = DataManagerTest.Instance.GetMonsterData(monster.name);
            if (mon == null)
            {
                DebugBoxManager.Instance.Log("Mon null");
                return;
            }
            else
            {
                monsters.Add(mon);
            }
        }

        List<int> monsterTypes = new List<int>();
        foreach(Monster mon in monsters)
        {
            monsterTypes.Add(mon.TypeIndex);
        }
        
        foreach(int weak in weaknesses)
        {
            if (monsterTypes.Contains(weak) == false)
            {
                string view = weak == 1 ? "불" : (weak == 2 ? "물" : "풀");
                DebugBoxManager.Instance.Log($"{view}타입 몬스터가 필요합니다");
                Btn_StartStage.DisablePokeBtn();
                return;
            }
        }
        

        Btn_StartStage.EnablePokeBtn();
    }
}
