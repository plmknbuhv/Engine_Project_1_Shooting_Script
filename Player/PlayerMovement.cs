using System;
using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);
    
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Vector3 rightAndUpLimit;
    [SerializeField] private Vector3 leftAndDownLimit;
    [SerializeField] private GameObject weaponObject;
    private Player _player;
    public bool isCanMove;

    private void OnEnable()
    {
        transform.position = new Vector3(0, -7.18f);
        StartCoroutine(AwakeCoroutine());
    }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if(!isCanMove) return;
        
        transform.position = inputReader.MousePosition;
    }

    private void LateUpdate()
    {
        if(!isCanMove) return;
        
        transform.position = new Vector3(                       
            Mathf.Clamp(transform.position.x, leftAndDownLimit.x, rightAndUpLimit.x),
            Mathf.Clamp(transform.position.y, leftAndDownLimit.y, rightAndUpLimit.y));
    }
    
    private IEnumerator AwakeCoroutine()
    {
        Tween myTween = transform.DOMoveY(-3f, 3f);
        yield return myTween.WaitForCompletion();
        isCanMove = true;
        weaponObject.SetActive(true);
        Vector2 mousePos = Camera.main.WorldToScreenPoint(new Vector3(0, 3f));
        SetCursorPos((int)mousePos.x, (int)mousePos.y);
        _player.isCanEffect = true;
    }

    #region DrawGizmo
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rightAndUpLimit, new Vector2(leftAndDownLimit.x, rightAndUpLimit.y));
        Gizmos.DrawLine(rightAndUpLimit, new Vector2(rightAndUpLimit.x, leftAndDownLimit.y));
        Gizmos.DrawLine(leftAndDownLimit, new Vector2(leftAndDownLimit.x, rightAndUpLimit.y));
        Gizmos.DrawLine(leftAndDownLimit, new Vector2(rightAndUpLimit.x, leftAndDownLimit.y));
    }
#endif
    #endregion
}
