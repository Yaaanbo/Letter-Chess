using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Timer UI")]
    [SerializeField] private Image timerImage;

    [Header("Lives UI")]
    [SerializeField] private Image[] livesImage;

    private void OnEnable()
    {
        gameManager.OnTimerUpdate += OnTimerUIUpdated;
        gameManager.OnLivesUpdate += OnLivesUpdated;
    }

    private void OnDisable()
    {
        gameManager.OnTimerUpdate -= OnTimerUIUpdated;
        gameManager.OnLivesUpdate -= OnLivesUpdated;
    }

    //Updating Timer UI
    private void OnTimerUIUpdated(float _currentTime, float _initTime)
    {
        timerImage.fillAmount = _currentTime / _initTime;
    }

    //Updating Lives UI
    private void OnLivesUpdated(int _lives)
    {
        livesImage[_lives].gameObject.SetActive(false);
    }
}
