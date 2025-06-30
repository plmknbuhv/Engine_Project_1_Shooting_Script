using TMPro;
using UnityEngine;

public class Phase : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private TextMeshProUGUI tmp;

    private void Awake()
    {
        boss.currentEnum.OnValueChanged += HandleChangePhaseText;
    }

    private void OnEnable()
    {
        tmp.text = "Phase 1";
    }

    private void HandleChangePhaseText(BossPhase prev, BossPhase next)
    {
        if ((int)next == 4)
        {
            tmp.text = $"Phase Over";
        }
        else
        {
            tmp.text = $"Phase {(int)next + 1}";
        }
    }
}
