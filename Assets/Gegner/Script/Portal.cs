using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI levelCleared;
    public TextMeshProUGUI gameOver;
    public UnityEngine.UI.Button tryAgain;

    void Start()
    {
        levelCleared.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        tryAgain.onClick.AddListener(OnButtonClicked);
    }

    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            levelCleared.gameObject.SetActive(true);
        }
    }

    void Died()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth.life <= 0)
        {
            gameOver.gameObject.SetActive(true);
            tryAgain.gameObject.SetActive(true);
        }
    }

    void OnButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
        
