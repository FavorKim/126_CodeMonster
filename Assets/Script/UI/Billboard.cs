using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        //transform.forward = _camera.transform.forward;
        Vector3 targetPosition = _camera.transform.position;

        transform.LookAt(targetPosition);
    }
}
