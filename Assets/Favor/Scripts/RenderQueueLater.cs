using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RenderQueueLater : MonoBehaviour
{
    [SerializeField]TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        Material mat = new Material(text.fontMaterial);
        mat.renderQueue = 3005;
        text.font.material = mat;
    }

}
