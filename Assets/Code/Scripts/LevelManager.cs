﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;
    public Transform startPoint1;

    public int currency = 400;
    public int playerHealth = 20;

    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject mainMenuUI;

    private Menu menu;
    public bool isGameOver = false;
    public bool isGameWin = false;
    public int enemyKillCount = 0;
    private void Awake()
    {
        main = this;
    }
    public void IncreaseKillCount()
    {
        enemyKillCount++;
        Debug.Log("Enemy killed! Total kills: " + enemyKillCount);

        // Only increase health when kills reach exactly 10, 20, 30, etc.
        if (enemyKillCount > 0 && enemyKillCount % 10 == 0)
        {
            Debug.Log("Đủ điều kiện tăng máu!");
            playerHealth++;
            Debug.Log("Máu trước khi cập nhật UI: " + playerHealth);
            UpdateHealthUI();
            Debug.Log("Tăng 1 máu! Máu hiện tại: " + playerHealth);
        }
    }



    void Start()
    {
        Time.timeScale = 0; // Dừng game khi vào
        mainMenuUI.SetActive(true); // Hiện menu

       
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
        if (gameWinUI != null)
        {
            gameWinUI.SetActive(false);
        }
        else
        {
            Debug.LogError("GameWin UI not assigned in the Inspector!");
        }
    }
    public void StartGame()
    {
        Time.timeScale = 1; // Chạy game
        mainMenuUI.SetActive(false); // Ẩn menu
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
            gameOverUI.SetActive(true);
    }    
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
            gameWinUI.SetActive(true);
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
    }    public bool IsGameWin()
    {
        return isGameWin;
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
