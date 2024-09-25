using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _camera;
    float originZ;
    [SerializeField]Quaternion targetRot;
    
    private void Start()
    {
        _camera = Camera.main.transform;
        originZ = transform.rotation.z;
    }

    private void Update()
    {
        //transform.forward = _camera.transform.forward;
        targetRot = GameManager.Instance.PlayerPrefab.transform.rotation;
        targetRot.y += 180;
        //targetRot.x -= 30;
        targetRot.z += 90;
        transform.rotation = targetRot;
        /*
        Vector3 targetPosition = _camera.transform.position;

        transform.LookAt(targetPosition);
        targetRot = transform.rotation;
        targetRot.z = originZ;
        transform.rotation = targetRot;
        */
    }
}
