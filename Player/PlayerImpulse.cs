using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerImpulse : MonoBehaviour
{
    [SerializeField] private float impulsePower;
    [SerializeField] private CinemachineImpulseSource horizontalImpulse;
    [SerializeField] private CinemachineImpulseSource verticalImpulse;

    private void Awake()
    {
        horizontalImpulse = GetComponent<CinemachineImpulseSource>();
    }

    public void PlayImpulse()
    {
        horizontalImpulse.GenerateImpulse(impulsePower);
        verticalImpulse.GenerateImpulse(impulsePower);
    }
}
