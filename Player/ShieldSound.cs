using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSound : MonoBehaviour
{
    [SerializeField] private AudioSource offSource;
    [SerializeField] private AudioSource onSource;
    private bool _isStart;

    private void OnEnable()
    {
        if (!_isStart)
        {
            _isStart = true;
        }
        else
        {
            onSource.Play();
        }
    }

    private void OnDisable()
    {
        offSource.Play();
    }
}
