using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig
{
    public string waveName;
    public List<GameObject> enemyPrefabs;
    public int enemyCount;
    public float enemiesPerSecond;
}
