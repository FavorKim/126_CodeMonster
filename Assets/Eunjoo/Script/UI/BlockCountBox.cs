using TMPro;
using UnityEngine;
using EventLibrary;
using EnumTypes;
using System.Collections;

public class BlockCountBox : MonoBehaviour
{
    private TextMeshProUGUI BlockCountText;
    [SerializeField] private GameObject BlockCountTextbox;
    [SerializeField] private GameObject BlockXTextbox;

    public Coroutine blockCountErrorCoroutine;

    private void Start()
    {
        BlockCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // 이벤트 리스너 등록
        EventManager<UIEvent>.StartListening<int>(UIEvent.BlockCountainerBlockCount, SetBlockCountText);
        EventManager<UIEvent>.StartListening(UIEvent.SetBlockCountError, SetBlockCountErrorText);

    }

    private void OnDisable()
    {
        // 이벤트 리스너 해제
        EventManager<UIEvent>.StopListening<int>(UIEvent.BlockCountainerBlockCount, SetBlockCountText);
        EventManager<UIEvent>.StopListening(UIEvent.SetBlockCountError, SetBlockCountErrorText);

    }

    public void SetBlockCountText(int count)
    {
        if(count <= UIManager.Instance.BlockContainerLength)
            BlockCountText.text = count.ToString();
    }

    public void SetBlockCountErrorText()
    {
        // 연달아 실행 됐을시 기존에 사용하던 코루틴은 중지하고 새 코루틴 할당
        // 만약 코루틴이 이미 실행 중이라면 중지
        if (blockCountErrorCoroutine != null)
        {
            StopCoroutine(blockCountErrorCoroutine);
        }

        // 코루틴을 시작하고 그 참조를 저장
        blockCountErrorCoroutine = StartCoroutine(BlockCountError());
    }

    IEnumerator BlockCountError()
    {
        BlockCountTextbox.SetActive(false);
        BlockXTextbox.SetActive(true);

        yield return new WaitForSeconds(1f);

        BlockCountTextbox.SetActive(true);
        BlockXTextbox.SetActive(false);

        blockCountErrorCoroutine = null;
    }
}
