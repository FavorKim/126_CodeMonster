using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CodeBlockManager : MonoBehaviour
{
    public Partner partner;
    public CodeBlockContainer container;

    public void ExecuteAll()
    {
        if (container == null || partner == null)
        {
            Debug.LogWarning("CodeBlockContainer or Partner is not set!");
            return;
        }

        // BehaviorTree 가져오기
        var behaviorTree = partner.GetComponent<BehaviorTree>();
        if (behaviorTree == null)
        {
            Debug.LogError("Partner does not have a BehaviorTree component.");
            return;
        }

        // 태스크들을 BehaviorTree에 전달 (에디터에서 RootTask가 이미 설정되어 있어야 함)
        var blocks = container.GetBlocks();
        foreach (var block in blocks)
        {
            Task task = block.CreateBehaviorTask();
            if (task != null)
            {
                // BehaviorTree의 기존 트리에 추가하거나 처리할 수 있습니다.
                // 에디터에서 루트 트리 설정 및 관리가 가능합니다.
            }
        }

        // BehaviorTree 활성화
        behaviorTree.EnableBehavior();
    }
}
