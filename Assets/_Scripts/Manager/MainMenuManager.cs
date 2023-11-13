using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "Highscore";

    [SerializeField] private TMP_Text highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
            highScoreText.text = $"Highscore : \n{PlayerPrefs.GetInt(HIGH_SCORE_KEY)}";
        else
            highScoreText.text = "Highscore : \n0";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
