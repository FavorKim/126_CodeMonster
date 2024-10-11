using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;

public class MyMeshSimplifier : MonoBehaviour
{
    static int oi;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SimpliFy();
    }

    private void SimpliFy()
    {
        if (oi == 0)
        {
            var originalMesh = GetComponent<MeshFilter>().mesh;
            float quality = 0.2f;
            var meshSimplifier = new MeshSimplifier();
            meshSimplifier.Initialize(originalMesh);
            meshSimplifier.SimplifyMesh(quality);
            var destMesh = meshSimplifier.ToMesh();
            SaveMesh(destMesh, "simplified");
            GetComponent<MeshFilter>().mesh = destMesh;
            oi++;
        }
    }
    private void OnApplicationQuit()
    {
        oi = 0;
    }
    public static void SaveMesh(Mesh mesh, string fileName)
    {
#if UNITY_EDITOR
        string path = "Assets/" + fileName + ".asset";

        // 메쉬가 유니티 에셋으로 저장되지 않았다면 새로 생성
        if (!AssetDatabase.Contains(mesh))
        {
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Mesh saved at: " + path);
        }
        else
        {
            Debug.LogWarning("This mesh is already saved as an asset.");
        }
#endif
    }
}
