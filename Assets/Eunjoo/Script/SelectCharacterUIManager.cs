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
    public List<int> GetSelectedTypeIndexes()
    {
        List<int> indexes = new List<int>();
        foreach (GameObject monster in selectedMonsters)
        {
            Monster mon = DataManagerTest.Instance.GetMonsterData(monster.name);
            indexes.Add(mon.TypeIndex);
        }
        if (indexes.Count > 0)
            return indexes;
        else
        {
            Debug.LogError("선택된 몬스터가 없거나 타입을 불러올 수 없습니다.");
            return null;
        }
    }
    public void AddMonster(GameObject monster)
    {
        if (monster == null)
        {
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
            CheckCanStart();
            //디버깅 할 수 없는 버그로 인해 사용 불가 (예컨대 sdk쪽 오류일 가능성 높음)
        }
        else
        {
        }

    }
    public void RemoveMonster(GameObject monster)
    {
        if (monster == null)
        {
            return;
        }
        if (selectedMonsters.Contains(monster))
        {
            monster.transform.SetParent(null);
            SetMonsterList();
            //FieldManager.Instance.TeleportMonstersToTargetPositions();
            //monster.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            CheckCanStart();
        }
        else
        {
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
        List<Monster> selectedMonsters = new List<Monster>();
        foreach (GameObject mon in this.selectedMonsters)
        {
            Monster selectedMon = DataManagerTest.Instance.GetMonsterData(mon.name);
            selectedMonsters.Add(selectedMon);
        }
        int stageIndex = UIManager.Instance.SelectChapterNum + UIManager.Instance.SelectStageNum;
        List<int> indexesofThisStage = GameManager.Instance.GetMonsterTypeInIndex(stageIndex);

        List<int> weaksofThisStageMonsters = new List<int>();
        foreach (int monIndex in indexesofThisStage)
        {
            int weakIndex = DataManagerTest.Instance.GetWeaknessIndexByMonsterTypeIndex(monIndex);
            if (weakIndex != 0)
                weaksofThisStageMonsters.Add(weakIndex);
        }

        List<int> typeIndexesOfSelectedMonsters = new List<int>();
        foreach (Monster mon in selectedMonsters)
        {
            int index = mon.TypeIndex;
            typeIndexesOfSelectedMonsters.Add(index);
        }
        int temp = 0;

        int fire = 0;
        int grass = 0;
        int water = 0;
        foreach (int type in typeIndexesOfSelectedMonsters)
        {
            switch (type)
            {
                case 1: fire++; break;
                case 2: water++; break;
                case 3: grass++; break;
            }
            if(weaksofThisStageMonsters.Contains(type) == false)
            {
                temp++;
            }
        }
        if (fire > 1 || water > 1 || grass > 1)
        {
            temp++;
        }
        if (temp == 0)
            Btn_StartStage.EnablePokeBtn();
    }
}
