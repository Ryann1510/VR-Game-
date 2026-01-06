using UnityEngine;
using TMPro; // Crucial: You need this namespace for TextMeshPro
public enum GameState { Starting, Playing, GameOver }

public class GameManager : MonoBehaviour
{
    // 1. Singleton: Allows other scripts to call ScoreManager.Instance.AddScore()
    public static GameManager Instance; 
    
    private int currentScore;
    [SerializeField] private Transform xrOrigin;

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
    [SerializeField] private Transform xrCamera;

    public GameState currentState = GameState.Starting;
    private Vector3 playerStartPosition = new Vector3(0f, 0f, 0f);
    
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

<<<<<<< HEAD
        if(gameOverPanel != null) gameOverPanel.SetActive(true);
        if(endScoreText != null) endScoreText.text = "Final Score: " + currentScore;
        
        // Move end canvas in front of the player
        Vector3 spawnPos = xrCamera.position + xrCamera.forward * 2f + Vector3.up * 0.5f; // slightly above eyes
        gameOverPanel.transform.position = spawnPos;

        // Make end canvas face the player
        gameOverPanel.transform.LookAt(xrCamera);
        gameOverPanel.transform.Rotate(0, 180f, 0);        
        
        
=======
        if (xrOrigin != null) xrOrigin.position = playerStartPosition;
        xrOrigin.rotation = Quaternion.Euler(0f, 90f, 0f);
        if (xrCamera != null) xrCamera.localRotation = Quaternion.identity;

        if(gameOverPanel != null) gameOverPanel.SetActive(true);
        if(endScoreText != null) endScoreText.text = "Final Score: " + currentScore;
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
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

<<<<<<< HEAD
    void Update()
    {
        if (startPanel != null && !startPanel.activeSelf) Debug.Log("StartPanel disabled by something");
        
        if (!isGameActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if(isGameActive)
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
            if(timeRemaining > 0)
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
=======
   void Update()
{
    if (!isGameActive)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    else // When the game IS active
    {
        // CHANGE THIS: Use Confined instead of Locked
        // Confined keeps the mouse inside the game window but let's it move
        Cursor.lockState = CursorLockMode.Confined; 
        
        // Keep visible as false if you only want to see the UI Reticle
        Cursor.visible = false; 

        if(timeRemaining > 0)
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
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)

}