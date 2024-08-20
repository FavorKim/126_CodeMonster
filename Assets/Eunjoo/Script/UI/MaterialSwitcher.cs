using UnityEngine;




public class MaterialChanger : MonoBehaviour
{
    public Material[] materials;  // 3개의 머티리얼 배열로 설정

    private Renderer rend;  // 오브젝트의 렌더러

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        keyDown();
    }

    private void keyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))  // 키보드 '0' 키를 눌렀을 때
        {
            ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);  // 첫 번째 머티리얼로 변경 : 일반 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))  // 키보드 '1' 키를 눌렀을 때
        {
            ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);  // 두 번째 머티리얼로 변경 : 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))  // 키보드 '2' 키를 눌렀을 때
        {
            ChangeMaterial(MaterialType.USE_CODEBLOCK_MATERIAL);  // 세 번째 머티리얼로 변경
        }
    }

    private void ChangeMaterial(int index)
    {
        if (index >= 0 && index < materials.Length)
        {
            rend.material = materials[index];  // 인덱스에 해당하는 머티리얼로 변경
        }
    }
}


public static class MaterialType
{
    public const int NORMAL_CODEBLOCK_MATERIAL = 0;
    public const int OUTLINE_CODEBLOCK_MATERIAL = 1;
    public const int USE_CODEBLOCK_MATERIAL = 2;
}
