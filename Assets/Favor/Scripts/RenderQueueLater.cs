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
        text.font.material.renderQueue = 3005;
    }

}
