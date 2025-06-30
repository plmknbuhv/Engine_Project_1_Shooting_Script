using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform arrow;
    [SerializeField] private List<GameObject> disableObject;
    [SerializeField] private List<GameObject> awakeObject;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image gameOverGui;
    [SerializeField] private Boss boss;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    private bool _isArrowUp = true;
    public bool isCanGame = true;
    private AudioSource _audioSource;
    public int playerCount = 0;

    private void Awake()
    {
        _audioSource = gameOverGui.GetComponent<AudioSource>();
    }

    public void GameOver()
    {
        audioSource2.Stop();
        StartCoroutine(GamOverCoroutine());
    }
    

    private IEnumerator GamOverCoroutine()
    {
        yield return new WaitForSeconds(3);
        yield return gameOverGui.DOFade(1, 2).WaitForCompletion();
        audioSource.Play();
        text.text = $"{playerCount} Dead";
        text2.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(16f);
        
        Application.Quit();
    }
    
    private void Update()
    {
        if (!isCanGame) return;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_isArrowUp) return;
            _isArrowUp = true;

            arrow.position = new Vector3(2.7f, -1.91f);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!_isArrowUp) return;
            _isArrowUp = false;
            
            arrow.position = new Vector3(2.7f, -3.52f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isArrowUp)
            {
                _audioSource.Play();
                
                foreach (var item in awakeObject)
                {
                    item.SetActive(true);
                }

                foreach (var item in disableObject)
                {
                    item.SetActive(false);
                }
                
                boss.BossBattleStart();
                isCanGame = false;
            }
            else if (!_isArrowUp)
            {
                Application.Quit();
            }
        }
    }
}
