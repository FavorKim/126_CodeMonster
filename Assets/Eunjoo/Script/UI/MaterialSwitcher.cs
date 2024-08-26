using UnityEngine;


public class MaterialChanger : MonoBehaviour
{
    public Material[] materials;  // 3개의 머티리얼 배열로 설정
    public GameObject X_Icon;

    private Renderer rend;  // 오브젝트의 렌더러


    public void ChangeMaterial(MaterialType index)
    {
        if (index >= 0 && index < MaterialType.MAX)
        {
            if (rend == null)
                rend = GetComponent<Renderer>();
            rend.material = materials[(int)index];  // 인덱스에 해당하는 머티리얼로 변경
        }
    }

    public void EnableXIcon()
    {
        X_Icon.SetActive(true);
    }

    public void DisableXIcon()
    {
        X_Icon.SetActive(false);
    }
}

public enum MaterialType
{
    NORMAL_CODEBLOCK_MATERIAL = 0,
    OUTLINE_CODEBLOCK_MATERIAL = 1,
    USE_CODEBLOCK_MATERIAL = 2,
    MAX
}
