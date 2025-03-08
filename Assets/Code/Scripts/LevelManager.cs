using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;
    public Transform startPoint1;

    public int currency;
    public int playerHealth = 10;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        currency = 260;
        FindObjectOfType<Menu>().UpdateCurrencyUI();
        UpdateHealthUI(); 
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        FindObjectOfType<Menu>().UpdateCurrencyUI();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            FindObjectOfType<Menu>().UpdateCurrencyUI();
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
            Debug.Log("Game Over!");
           
        }
    }

    private void UpdateHealthUI()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = "HP: " + playerHealth.ToString();
        }
    }
}
