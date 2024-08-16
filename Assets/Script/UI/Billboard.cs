using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    private void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>().transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.rotation * Vector3.forward, _camera.rotation * Vector3.up);
    }
}
