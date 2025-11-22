using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Tower
    {
        public Transform floor; 
        public Transform snapPoint; 
    }

    public Tower[] towers; 
    public GameObject[] blocks; 

    void Start()
    {
        SpawnBlocks();
    }

    void SpawnBlocks()
    {
        List<GameObject> blockPool = new List<GameObject>(blocks);

        while (blockPool.Count > 0)
        {
            int bIndex = Random.Range(0, blockPool.Count);
            GameObject block = blockPool[bIndex];
            blockPool.RemoveAt(bIndex);

            int tIndex = Random.Range(0, towers.Length);
            Tower tower = towers[tIndex];

            Vector3 spawnPos = tower.snapPoint.position + new Vector3(0, 2f, 0); 
            Instantiate(block, spawnPos, Quaternion.identity);
        }
    }
}
