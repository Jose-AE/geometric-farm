using TMPro;
using UnityEngine;


public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text hudScoreText;

    [SerializeField] GameObject HUD;

    public void ResetLevel()
    {
        SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        gameOverScreen.SetActive(false);

    }


    void OnTimeRanOut()
    {
        scoreText.text = hudScoreText.text;
        gameOverScreen.SetActive(true);
        HUD.SetActive(false);
    }


    void OnEnable()
    {
        Level1GameplayLogic.OnTimeRanOut += OnTimeRanOut;
        Level2GameplayLogic.OnTimeRanOut += OnTimeRanOut;
        Level3GameplayLogic.OnTimeRanOut += OnTimeRanOut;


    }
    void OnDisable()
    {
        Level1GameplayLogic.OnTimeRanOut -= OnTimeRanOut;
        Level2GameplayLogic.OnTimeRanOut -= OnTimeRanOut;
        Level3GameplayLogic.OnTimeRanOut -= OnTimeRanOut;

    }

}