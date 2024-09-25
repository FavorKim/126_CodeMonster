using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectManager : Singleton<CollectManager>
{
    [SerializeField] Transform MonsterSpawnPos;
    [SerializeField] Transform FeedArea;
    [SerializeField] Slider captureGaugeSlider;
    GameObject spawnedMonster;
    float captureGauge = 0;
    public float CaptureGauge 
    {
        get
        {
            return captureGauge; 
        }
        set
        {
            captureGauge = value;
            captureGaugeSlider.value = captureGauge / MaxCaptureGauge;
            if(captureGauge>= MaxCaptureGauge)
            {
                GameManager.Instance.StartLoading(OnCompleteCollect);
            }
        }
    }

    [SerializeField] float MaxCaptureGauge = 5.0f;
    [SerializeField] float decreaseAmount = 0.5f;
    bool isCapturing;
    public bool IsCapturing
    {
        get
        {
            return isCapturing;
        }
        set
        {
            isCapturing = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
    public void OnStartCollectScene()
    {
        gameObject.SetActive(true);
        SpawnCollectableMonster();
    }
    public void OnEndCollectScene()
    {
        Destroy(spawnedMonster);
        spawnedMonster = null;
        gameObject.SetActive(false);
        UIManager.Instance.OutgameUIManager.ClickGoToChapter();
        GameManager.Instance.ResetPlayerPosition();
    }

    public void OnCompleteCollect()
    {
        GameManager.instance.AddMonsterInPlayerList(spawnedMonster.name);

        OnEndCollectScene();
    }

    public void SpawnCollectableMonster()
    {
        string uniqueMonsterName = string.Empty;
        switch (UIManager.Instance.SelectChapterNum)
        {
            case 1000:
                uniqueMonsterName = "Char_Unique_Battle_Character_Fire_01";
                break;

            case 2000:
                uniqueMonsterName = "Char_Unique_Battle_Character_Leaf_01";
                break;

            case 3000:
                uniqueMonsterName = "Char_Unique_Battle_Character_Water_01";
                break;
            default:
                Debug.LogError("수집가능한 스테이지가 아닙니다");
                break;
        }

        spawnedMonster = MonsterObjPoolManger.Instance.GetMonsterPrefab(uniqueMonsterName);

        Instantiate(spawnedMonster, MonsterSpawnPos.transform.position, Quaternion.Euler(0, 180, 0));
    }

    private void Update()
    {
        if(CaptureGauge > 0)
        {
            if (IsCapturing == false)
            {
                CaptureGauge -= Time.deltaTime * decreaseAmount; 
            }
            else
            {
                CaptureGauge += Time.deltaTime;
            }
        }
        MoveSpawnedMonster();
    }

    void MoveSpawnedMonster()
    {
        if (spawnedMonster == null) return;
        if (isCapturing)
        {
            if (CaptureGauge < MaxCaptureGauge)
                spawnedMonster.transform.Translate(FeedArea.position * CaptureGauge / MaxCaptureGauge);
        }
        else
        {
            if (CaptureGauge <= 0)
            {

            }
            else
            {
                spawnedMonster.transform.Translate(MonsterSpawnPos.position * (1 - CaptureGauge / MaxCaptureGauge));
            }
        }
    }
}
