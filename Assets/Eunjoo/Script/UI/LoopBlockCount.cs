using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoopBlockCount : MonoBehaviour
{
    private TextMeshProUGUI BlockCountText;

    private void Start()
    {
        BlockCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // 이벤트 리스너 등록
        EventManager<UIEvent>.StartListening<int>(UIEvent.LoopBlockContainerBlockCount, SetBlockCountText);

    }

    private void OnDisable()
    {
        // 이벤트 리스너 해제
        EventManager<UIEvent>.StopListening<int>(UIEvent.LoopBlockContainerBlockCount, SetBlockCountText);

    }

    public void SetBlockCountText(int count)
    {
        if (count <= UIManager.Instance.MakeLoopBlockContainerLength)
            BlockCountText.text = count.ToString();
    }
}
