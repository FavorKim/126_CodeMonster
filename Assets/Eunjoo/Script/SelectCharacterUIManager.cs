using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject MonsterAttributeBox;
    private void OnEnable()
    {
        CheckMonsterAttributes();
    }


    public void CheckMonsterAttributes()
    {
        // 몬스터 타입 인덱스 리스트를 가져옴
        List<int> monsterTypeIndices = GameManger.Instance.GetMonsterTypeInIndex(UIManager.Instance.SelectStageNum + UIManager.Instance.SelectChaterNum);

        // 리스트에서 몬스터 타입을 확인
        foreach (int monsterType in monsterTypeIndices)
        {
            switch (monsterType)
            {
                case 5:
                    SetupMonsterAttribute(BlockName.FireAttackCodeBlock);
                    break;
                case 6:
                    SetupMonsterAttribute(BlockName.WaterAttackCodeBlock);
                    break;
                case 7:
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
        attackCodeBlockHandGrab.transform.localScale = Vector3.one;
    }
}
