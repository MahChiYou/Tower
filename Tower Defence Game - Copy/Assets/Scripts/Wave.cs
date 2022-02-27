using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Data", menuName = "Tower Defense Game/Wave Data", order = 1)]
public class Wave : ScriptableObject
{
    public float spawnRate;
    public WaveGroup[] enemies;
    
    [System.Serializable]
    public class WaveGroup
    {
        public GameObject enemy;
        public int count;
    }
}

