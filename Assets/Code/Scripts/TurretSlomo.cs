using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class TurretSlomo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private GameObject upgradeUI;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float aps = 0.25f; // attack per second
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int level = 1; // Cấp độ của tháp
    [SerializeField] private int maxLevel = 5; // Cấp độ tối đa
    [SerializeField] private int upgradeCost = 100; // Chi phí nâng cấp

    private float timeUntilFire;

    // Start is called before the first frame update
    void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);  // Gắn hàm Upgrade cho nút
        sellButton.onClick.AddListener(Sell);        // Gắn hàm Sell cho nút
        UpdateStats();  // Cập nhật thuộc tính ban đầu của tháp
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / aps)
        {
            freezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void freezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f); // Giảm tốc độ của kẻ thù

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }

    public void Upgrade()
    {
        if (level < maxLevel)
        {
            // Nếu đủ tiền, nâng cấp
            if (LevelManager.main.currency >= upgradeCost)
            {
                LevelManager.main.SpendCurrency(upgradeCost);
                level++;
                UpdateStats();
                Debug.Log($"SlomoTurret upgraded to level {level}. New targeting range: {targetingRange}, New APS: {aps}, New Freeze time: {freezeTime}");

                // Mở UI nâng cấp sau khi nâng cấp
                OpenUpgradeUI();
            }
            else
            {
                Debug.Log("Not enough currency to upgrade!");
            }
        }
        else
        {
            Debug.Log("SlomoTurret is already at max level.");
        }
    }

    private void UpdateStats()
    {
        targetingRange = 3f + level * 0.5f;
        aps = 0.25f + level * 0.05f;
        freezeTime = 1f + level * 0.2f;

        if (level >= maxLevel)
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }


    public void Sell()
    {
        // Tính giá trị bán lại của tháp và trả lại một phần tiền
        int sellValue = Mathf.RoundToInt(upgradeCost * 0.5f);
        LevelManager.main.IncreaseCurrency(sellValue);

        // Đóng UI nâng cấp nếu nó đang mở khi bán tháp
        CloseUpgradeUI();

        Destroy(gameObject); // Xóa tháp khỏi trò chơi
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    public void OpenUpgradeUI()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(true); // Hiển thị UI nâng cấp
        }
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(false); // Ẩn UI nâng cấp
        }
        UIManager.main.SetHoveringState(false);
    }
}