using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI levelCleared;
    public TextMeshProUGUI gameOver;
    public UnityEngine.UI.Button tryAgain;
    public GameObject enemyToKill;

    public GameObject pauseMenuUI;
    public UnityEngine.UI.Button resumeButton;
    public UnityEngine.UI.Button quitButton;

    private Collider portalCollider;
    private Renderer portalRenderer;
    private bool hasDied = false;
    private bool isPaused = false;

    void Start()
    {
        levelCleared.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        tryAgain.onClick.AddListener(OnButtonClicked);

        pauseMenuUI.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);

        portalCollider = GetComponent<Collider>();
        portalRenderer = GetComponent<Renderer>();
        SetPortalVisible(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !hasDied)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (enemyToKill == null)
        {
            SetPortalVisible(true);
        }

        if (player == null)
        {
            Died();
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.life <= 0)
        {
            Died();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void QuitGame()
    {
        Time.timeScale = 1f;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void SetPortalVisible(bool visible)
    {
        portalRenderer.enabled = visible;
        portalCollider.enabled = visible;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelCleared.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    void Died()
    {
        if (hasDied) return;
        hasDied = true;
        gameOver.gameObject.SetActive(true);
        tryAgain.gameObject.SetActive(true);
    }

    void OnButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}