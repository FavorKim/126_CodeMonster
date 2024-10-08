using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectManager : Singleton<CollectManager>
{
    [SerializeField] Transform MonsterSpawnPos;
    [SerializeField] Transform MonsterDestination;
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
            if (isSucceed) return;

            captureGauge = value;
            captureGaugeSlider.value = captureGauge / MaxCaptureGauge;
            if (captureGauge >= MaxCaptureGauge)
            {
                OnCompleteCollect();
            }
        }
    }

    bool isSucceed = false;

    [SerializeField] float MaxCaptureGauge = 5.0f;
    [SerializeField] float decreaseAmount = 0.5f;
    [SerializeField] bool isCapturing;
    public bool IsCapturing
    {
        get
        {
            return isCapturing;
        }
        set
        {
            if (isSucceed) return;
            isCapturing = value;
            if (isCapturing)
            {
                UIManager.Instance.PrintOnFeeding();
            }
            else
            {
                UIManager.Instance.PrintOnGrabFood();
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }


    public void OnStartCollectScene()
    {
        isSucceed = false;
        CaptureGauge = 0;
        IsCapturing = false;
        gameObject.SetActive(true);
        SpawnCollectableMonster();
        UIManager.Instance.PrintCollectStage();
    }
    public void OnEndCollectScene()
    {
        Destroy(spawnedMonster);
        spawnedMonster = null;
        gameObject.SetActive(false);
        UIManager.Instance.OutgameUIManager.ClickGoToChapter();
        UIManager.Instance.SelectCharacterUIManager.RemoveAllMonsters();
        GameManager.Instance.SetPlayerPosToMainMenu();
        FieldManager.Instance.OnStartOutGameUI();
        FieldManager.Instance.EnableAllMonsters();
        FieldManager.Instance.MoveAllMonsters();
    }
    private void StartLoadingOnEndCollectScene()
    {
        GameManager.Instance.StartLoading(OnEndCollectScene);
    }

    public void OnCompleteCollect()
    {
        isSucceed = true;
        GameManager.instance.AddMonsterInPlayerList(spawnedMonster.name);
        UIManager.Instance.PrintOnSuccessCollect();
        GameManager.Instance.StartInvokeCallBack(StartLoadingOnEndCollectScene, 5);
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

        spawnedMonster = Instantiate(spawnedMonster, MonsterSpawnPos.transform.position, Quaternion.Euler(0, 180, 0));
        spawnedMonster.SetActive(true);
    }

    private void Update()
    {
        if (!isSucceed)
        {
            if (IsCapturing == false)
            {
                if (CaptureGauge >= 0)
                    CaptureGauge -= Time.deltaTime * decreaseAmount;
            }
            else
            {
                if (CaptureGauge <= MaxCaptureGauge)
                    CaptureGauge += Time.deltaTime;
            }
            MoveSpawnedMonster();
        }


    }

    void MoveSpawnedMonster()
    {
        if (spawnedMonster == null) return;
        if (isCapturing)
        {
            if (CaptureGauge < MaxCaptureGauge - 2)
            {
                AnimationPlayer.SetBool("isWalk", spawnedMonster, true);
                AnimationPlayer.SetBool("isEating", spawnedMonster, false);

                spawnedMonster.transform.position = Vector3.Lerp(MonsterSpawnPos.position, MonsterDestination.position, CaptureGauge / (MaxCaptureGauge - 2));
            }
            else
            {
                AnimationPlayer.SetBool("isWalk", spawnedMonster, false);
                AnimationPlayer.SetBool("isEating", spawnedMonster, true);
            }
        }
        else
        {
            if (CaptureGauge <= 0)
            {

            }
            else
            {
                AnimationPlayer.SetBool("isWalk", spawnedMonster, true);
                AnimationPlayer.SetBool("isEating", spawnedMonster, false);
                spawnedMonster.transform.position = Vector3.Lerp(MonsterDestination.position, MonsterSpawnPos.position, 1 - (CaptureGauge / (MaxCaptureGauge-2)));
            }
        }
    }
}
