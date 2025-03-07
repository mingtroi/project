using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;
    public Transform startPoint1;

    public int currency;
    public int playerHealth = 10;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private GameObject gameOverUI;
    private Menu menu;
    public bool isGameOver = false;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        currency = 260;
        menu = FindObjectOfType<Menu>();

        if (menu != null)
        {
            menu.UpdateCurrencyUI();
        }
        else
        {
            Debug.LogWarning("Menu not found in the scene!");
        }

        UpdateHealthUI();

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOver UI not assigned in the Inspector!");
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        if (menu != null)
        {
            menu.UpdateCurrencyUI();
        }
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            if (menu != null)
            {
                menu.UpdateCurrencyUI();
            }
            return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    private void UpdateHealthUI()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = "HP: " + playerHealth.ToString();
        }
        else
        {
            Debug.LogError("playerHealthText is not assigned!");
        }
    }
}
