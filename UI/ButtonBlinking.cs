using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ButtonBlinking : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        Cursor.visible = false;
    }

    private void OnEnable()
    {
        StartCoroutine("BlinkingCoroutine");
    }

    private void OnDisable()
    {
        StopCoroutine("BlinkingCoroutine");
    }

    private IEnumerator BlinkingCoroutine()
    {
        while (true)
        {
            text.alpha = 0;
            yield return new WaitForSeconds(0.28f);
            text.alpha = 1;
            yield return new WaitForSeconds(0.44f);
        }
    }

    public void StopBlinking()
    {
        StopCoroutine("BlinkingCoroutine");
    }
}
