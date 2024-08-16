using TMPro;
using UnityEngine;
using EventLibrary;
using EnumTypes;

public class BlockCount : MonoBehaviour
{
    TextMeshProUGUI BlockCountText;

    private void Start()
    {
        BlockCountText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // 이벤트 리스너 등록
        EventManager<UIEvent>.StartListening<int>(UIEvent.SetBlockCount, SetBlockCountText);
    }

    private void OnDisable()
    {
        // 이벤트 리스너 해제
        EventManager<UIEvent>.StopListening<int>(UIEvent.SetBlockCount, SetBlockCountText);
    }

    public void SetBlockCountText(int count)
    {
        BlockCountText.text = count.ToString();
    }
}
