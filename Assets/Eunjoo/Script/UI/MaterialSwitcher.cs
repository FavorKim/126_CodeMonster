using EPOOutline;
using UnityEngine;




public class MaterialChanger : MonoBehaviour
{
    public Material[] materials;  // 3개의 머티리얼 배열로 설정

    private Renderer rend;  // 오브젝트의 렌더러
    [SerializeField]private Outlinable outlineable;
   

    public void ChangeMaterial(MaterialType index)
    {
        if (outlineable == null)
        {
            outlineable = gameObject.GetComponentInParent<Outlinable>();
            outlineable.AddAllChildRenderersToRenderingList();
        }

        if (index >= 0 && index < MaterialType.MAX)
        {
            if (rend == null)
                rend = GetComponent<Renderer>();
            rend.material = materials[(int)index];  // 인덱스에 해당하는 머티리얼로 변경
        }
        if ((int)index == 0) ChangeOutlineNormal();
        else if((int)index == 1) ChangeOutlinePlay();
        else if ((int)index == 2) ChangeOutlineUsed();
    }

    public void ChangeOutlineNormal()
    {
        outlineable.OutlineParameters.Color = Color.white;
    }

    public void ChangeOutlinePlay()
    {
        outlineable.OutlineParameters.Color = Color.red;
    }
    public void ChangeOutlineUsed()
    {
        outlineable.OutlineParameters.Color = Color.black;
    }
}


public enum MaterialType
{
    NORMAL_CODEBLOCK_MATERIAL = 0,
    OUTLINE_CODEBLOCK_MATERIAL = 1,
    USE_CODEBLOCK_MATERIAL = 2,
    MAX
}
