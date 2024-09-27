using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnEndEvent : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] FXType type;


    private void OnEnable()
    {
        if (particle == null)
            particle = GetComponent<ParticleSystem>();
        particle.Play();
    }

    private void OnDisable()
    {
        particle.Stop();
    }

    private void Update()
    {
        if (particle.isStopped)
        {
            FXManager.Instance.ReturnToPool(this, type);
        }
    }
}
