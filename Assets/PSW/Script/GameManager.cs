using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    private List<string> _playerMonsterNameList = new List<string>();
    [SerializeField] Transform CollectingPos;
    [SerializeField] public GameObject PlayerPrefab;
    [SerializeField] GameObject ScreenBlocker;
    [SerializeField] GameObject LoadingBar;
    Vector3 PlayerOriginPos;

    protected override void Start()
    {
        base.Start();
        _playerMonsterNameList = DataManagerTest.instance.GetPlayerData("Player").StartMonsterNameList;
        PlayerOriginPos = PlayerPrefab.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetPlayerPosToCollectingZone();
        }
    }

    public void AddMonsterInPlayerList(string monsterName)//플레이어 보유 몬스터에 몬스터를 추가
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);

        if (CheckMonsterInPlayerList(name) == false)
        {
            if (_playerMonsterNameList == null)
                _playerMonsterNameList = DataManagerTest.instance.GetPlayerData("Player").StartMonsterNameList;

            _playerMonsterNameList.Add(name);
        }
        else
        {
            return;
        }
    }

    public bool CheckMonsterInPlayerList(string monsterName)//플레이어 보유 몬스터에 몬스터가 있는지 검사
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);
        if (_playerMonsterNameList.Count == 0)
            _playerMonsterNameList = DataManagerTest.instance.GetPlayerData("Player").StartMonsterNameList;
        if (_playerMonsterNameList.Contains(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetMonsterNameInPlayerList(string monsterName)//플레이어 보유 목록에서 보유한 몬스터의 이름을 반환 단 ( 가 있으면 ( 뒤는 삭제후 반환
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);

        if (CheckMonsterInPlayerList(name))
        {
            return name;
        }
        else
        {
            return string.Empty;
        }
    }

    public List<int> GetMonsterTypeInIndex(int stageIndex)
    {
        StageMap stageInfo = DataManagerTest.instance.GetStageMapData(stageIndex);
        if (stageInfo == null)
        {
            Debug.LogError("StageInfo is NULL!");
            return null;
        }
        else
        {
            List<int> list = stageInfo.MonsterIDList;

            if (DataManagerTest.instance.GetStageMapData(stageIndex).BushMonsterIDList != null)
            {
                foreach (var item in DataManagerTest.instance.GetStageMapData(stageIndex).BushMonsterIDList)
                {

                    var replaceString = item.Replace("(", "").Replace(")", "");

                    var elements = replaceString.Split('/');

                    for (int i = 0; i < elements.Length; i++)
                    {
                        var element = elements[i];
                        list.Add(int.Parse(element));
                    }
                }
            }

            List<int> monsterTypeIndexList = new List<int>();
            foreach (var item in list)
            {
                int monsterTypeIndex = DataManagerTest.instance.GetMonsterData(item).TypeIndex;

                if (monsterTypeIndexList.Contains(monsterTypeIndex) == false)
                {
                    monsterTypeIndexList.Add(monsterTypeIndex);
                }
            }

            return monsterTypeIndexList;
        }
    }

    private void SetPlayerPosTo(Transform position)
    {
        PlayerPrefab.transform.position = position.position;
    }
    private void SetPlayerPosToCollectingZone()
    {
        PlayerPrefab.transform.position = CollectingPos.position;
        PlayerPrefab.transform.localRotation = Quaternion.Euler(5, PlayerPrefab.transform.localRotation.y, PlayerPrefab.transform.localRotation.z);
    }

    public void StartCollectScene()
    {
        SetPlayerPosToCollectingZone();
        UIManager.Instance.DisableIngameUI();
        CollectManager.Instance.OnStartCollectScene();
    }

    IEnumerator CorFadeIn(float duration)
    {
        ScreenBlocker.SetActive(true);

        // 원래의 Material을 가져옵니다.
        Renderer renderer = ScreenBlocker.GetComponent<Renderer>();
        Material blockerMaterial = renderer.material; // material로 접근하면 인스턴스를 생성합니다.

        // Material의 색상을 가져오고, 알파 값을 저장합니다.
        Color col = blockerMaterial.color;
        col.a = 1;
        float curTime = 0f;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;

            // 알파 값이 1에서 0으로 서서히 감소하도록 설정
            col.a = Mathf.Lerp(1, 0f, curTime / duration);
            blockerMaterial.color = col;

            yield return null; // 다음 프레임까지 대기
        }

        // 루프가 끝난 후 최종 알파 값 설정 (혹시 남아있을 작은 오차를 방지)
        col.a = 0f;
        blockerMaterial.color = col;
        ScreenBlocker.SetActive(false);
        LoadingBar.SetActive(false);

    }
    IEnumerator CorFadeOut(float duration)
    {
        ScreenBlocker.SetActive(true);

        // 원래의 Material을 가져옵니다.
        Renderer renderer = ScreenBlocker.GetComponent<Renderer>();
        Material blockerMaterial = renderer.material; // material로 접근하면 인스턴스를 생성합니다.

        // Material의 색상을 가져오고, 알파 값을 저장합니다.
        Color col = blockerMaterial.color;
        col.a = 0;
        LoadingBar.SetActive(true);
        blockerMaterial.color = col;
        float curTime = 0f;
        while (curTime < duration)
        {
            curTime += Time.deltaTime;

            // 알파 값이 0에서 1로 서서히 증가하도록 설정
            col.a = Mathf.Lerp(0, 1f, curTime / duration);
            blockerMaterial.color = col;

            yield return null; // 다음 프레임까지 대기
        }

        // 루프가 끝난 후 최종 알파 값 설정 (혹시 남아있을 작은 오차를 방지)
        col.a = 1f;
        blockerMaterial.color = col;
    }

    IEnumerator CorLoading(Action action)
    {
        StartCoroutine(CorFadeOut(0.5f));
        yield return new WaitForSeconds(0.5f);
        action.Invoke();
        yield return new WaitForSeconds(2f);
        StartCoroutine(CorFadeIn(0.5f));
    }

    public void StartLoading(Action action)
    {
        StartCoroutine(CorLoading(action));
    }

    public void ResetPlayerPosition()
    {
        PlayerPrefab.transform.position = PlayerOriginPos;
    }
}
