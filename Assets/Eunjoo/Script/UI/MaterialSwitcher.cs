using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material[] materials;  // 3개의 머티리얼 배열로 설정

    private Renderer rend;  // 오브젝트의 렌더러

    void Start()
    {
        rend = GetComponent<Renderer>();

        // 기본적으로 첫 번째 머티리얼을 할당
        if (materials.Length > 0)
        {
            rend.material = materials[0];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))  // 키보드 '0' 키를 눌렀을 때
        {
            ChangeMaterial(0);  // 첫 번째 머티리얼로 변경
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))  // 키보드 '1' 키를 눌렀을 때
        {
            ChangeMaterial(1);  // 두 번째 머티리얼로 변경
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))  // 키보드 '2' 키를 눌렀을 때
        {
            ChangeMaterial(2);  // 세 번째 머티리얼로 변경
        }
    }

    void ChangeMaterial(int index)
    {
        if (index >= 0 && index < materials.Length)
        {
            rend.material = materials[index];  // 인덱스에 해당하는 머티리얼로 변경
        }
    }
}
