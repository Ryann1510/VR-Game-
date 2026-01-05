using UnityEngine;
using TMPro; // Crucial: You need this namespace for TextMeshPro
public enum GameState { Starting, Playing, GameOver }

public class GameManager : MonoBehaviour
{
    // 1. Singleton: Allows other scripts to call ScoreManager.Instance.AddScore()
    public static GameManager Instance; 
    
    private int currentScore;
    

    
    [Header("Game Settings")]
    public float gameDuration = 60f; // Total time for one round
    private float timeRemaining;
    public bool isGameActive = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject startPanel;
    
    public GameState currentState = GameState.Starting;
    
    void Awake() 
    {
        Debug.Log("GameManager Awake: " + gameObject.name);
        // Set up the static instance
        if (Instance == null)
        {
            Instance = this;
            timeRemaining = gameDuration;
        }
        else
        {
            Destroy(gameObject); // Prevent multiple managers
        }
    }

    void Start()
    {
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
        if(startPanel != null) startPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");
        Target[] activeTargets = GameObject.FindObjectsOfType<Target>();
        
        foreach (Target t in activeTargets)
        {
            Destroy(t.gameObject);
        }

        currentScore = 0;
        timeRemaining = gameDuration;
        isGameActive = true;
        
        UpdateScoreUI();

        // Hide both the Start and Game Over panels
        if(startPanel != null) startPanel.SetActive(false);
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void EndGame()  
    {
        isGameActive = false;
        timeRemaining = 0;
        if(gameOverPanel != null) gameOverPanel.SetActive(true);
        if(endScoreText != null) endScoreText.text = "Final Score: " + currentScore;
        Debug.Log("Game Over! Final Score: " + currentScore);
    }

    public void AddScore(int points)
    {
        if (!isGameActive) return;
        currentScore += points;
        UpdateScoreUI();
        Debug.Log("Score Updated! Current Score: " + currentScore.ToString() + " (Points Gained: " + points.ToString() + ")");
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    void Update()
{
    // Logic for when the game is NOT running (Menus)
    if (!isGameActive)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Keyboard shortcut to start the game
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartGame();
        }
    }
    // Logic for when the game IS running
    else
    {
        // FIX: Allow mouse movement for the 3D reticle
        Cursor.lockState = CursorLockMode.Confined; 
        Cursor.visible = false; 

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndGame();
        }
    }
}

}