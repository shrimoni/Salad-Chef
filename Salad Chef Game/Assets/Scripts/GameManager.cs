using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    public Booster boosterPrefab; // booster prefab 

    // Game players
    public Player player1;
    public Player player2;

    // UI Elements
    public GameObject gameOverPanel;

    public Text playerVictoryText;

    public bool isGameOver; // to check if game is over or not

    // Clamp transforms to restrict player movement
    public Transform clampPositionLeft;
    public Transform clampPositionRight;
    public Transform clampPositionTop;
    public Transform clampPositionBottom;

    private void Awake()
    {
        PauseGame();
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If Game is over
        if (!isGameOver && player1.timeLeft <= 0 && player2.timeLeft <= 0)
        {
            PauseGame();
            Debug.Log("Game Over..");
            gameOverPanel.SetActive(true);
            if (player1.score > player2.score)
            {
                playerVictoryText.text = "Player 1 wins!";
                Debug.Log("Player 1 wins");
            }
            else if (player1.score < player2.score)
            {
                playerVictoryText.text = "Player 2 wins!";
                Debug.Log("Player 2 wins");
            }else
            {
                playerVictoryText.text = "Match Tie!";
                Debug.Log("Match Tie");
            }
            isGameOver = true;
        }
    }

    // Pauses the game
    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    // Resumes the game
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    // Restarts the game
    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }

    // Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
