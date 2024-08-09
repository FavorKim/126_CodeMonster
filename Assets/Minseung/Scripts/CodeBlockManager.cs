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

        // BehaviorTree ��������
        var behaviorTree = partner.GetComponent<BehaviorTree>();
        if (behaviorTree == null)
        {
            Debug.LogError("Partner does not have a BehaviorTree component.");
            return;
        }

        // �½�ũ���� BehaviorTree�� ���� (�����Ϳ��� RootTask�� �̹� �����Ǿ� �־�� ��)
        var blocks = container.GetBlocks();
        foreach (var block in blocks)
        {
            Task task = block.CreateBehaviorTask();
            if (task != null)
            {
                // BehaviorTree�� ���� Ʈ���� �߰��ϰų� ó���� �� �ֽ��ϴ�.
                // �����Ϳ��� ��Ʈ Ʈ�� ���� �� ������ �����մϴ�.
            }
        }

        // BehaviorTree Ȱ��ȭ
        behaviorTree.EnableBehavior();
    }
}
