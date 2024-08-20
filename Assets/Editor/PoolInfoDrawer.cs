using UnityEditor;
using UnityEngine;

// Inspector에 Name넣기
[CustomPropertyDrawer(typeof(PoolInfo))]
public class PoolInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // PoolInfo 리스트의 부모를 찾습니다.
        SerializedProperty parentList = property.serializedObject.FindProperty("poolInfoList");
        if (parentList == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // 현재 요소의 인덱스를 가져옵니다.
        int index = GetIndexInParentArray(property);

        // 인덱스를 기반으로 BlockName을 설정합니다.
        BlockName blockName = (BlockName)index;

        // BlockName을 라벨로 설정
        label.text = blockName.ToString();

        // 실제 BlockName 필드에 값을 설정합니다.
        SerializedProperty blockNameProperty = property.FindPropertyRelative("BlockName");
        blockNameProperty.enumValueIndex = index;

        // 나머지 필드들을 그립니다.
        EditorGUI.PropertyField(position, property, label, true);
    }

    // 부모 배열에서 현재 요소의 인덱스를 가져오는 헬퍼 메서드
    private int GetIndexInParentArray(SerializedProperty property)
    {
        string path = property.propertyPath;
        string indexStr = path.Substring(path.LastIndexOf('[')).Trim('[', ']');
        return int.Parse(indexStr);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}