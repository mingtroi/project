using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer turretSpriteRenderer; 
    [SerializeField] private Sprite upgradedSprite;
    [SerializeField] private Sprite[] turretSprites; // Mảng chứa 4 sprite, mỗi sprite ứng với 1 cấp

    [Header("Upgrade Settings")]
    [SerializeField] private int maxLevel = 4;   


    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private float damage = 10f;


    private float bpsBase;
    private float targetingRangeBase;

    private Transform target;
    private float timeUntilFire;

    private int level = 1;
    private Plot parentPlot; 

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;

        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(Sell);
    }

    public void SetParentPlot(Plot plot)
    {
        parentPlot = plot;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                  target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            Transform closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
            target = closestEnemy;
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        bulletScript.SetDamage((int)damage);
    }

    public void OpenUpgradeUI()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(true);
        }
    }

    public void CloseUpgradeUI()
    {
        if (upgradeUI != null)
        {
            upgradeUI.SetActive(false);
        }
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (level >= maxLevel)
        {
            Debug.Log("Turret đã ở cấp tối đa!");
            return;
        }

        int upgradeCost = CalculateCost();
        if (upgradeCost > LevelManager.main.currency)
        {
            Debug.Log("Không đủ tiền để nâng cấp!");
            return;
        }

        LevelManager.main.SpendCurrency(upgradeCost);
        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();
        damage = CalculateDamage();  

        if (turretSpriteRenderer != null && turretSprites.Length >= level)
        {
            turretSpriteRenderer.sprite = turretSprites[level - 1];
        }

        CloseUpgradeUI();
        Debug.Log($"Upgraded to level {level}. New BPS: {bps}, New Range: {targetingRange}, New Damage: {damage}");
    }



    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private float CalculateDamage()
    {
        return damage * Mathf.Pow(level, 0.5f);  
    }

    public void Sell()
    {
        int sellValue = Mathf.RoundToInt(CalculateCost() * 0.5f);
        LevelManager.main.IncreaseCurrency(sellValue);

        if (parentPlot != null)
        {
            parentPlot.ClearTurret();
        }

        Destroy(gameObject);
    }

}
