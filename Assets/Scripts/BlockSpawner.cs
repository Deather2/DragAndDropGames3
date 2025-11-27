using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    [System.Serializable]
    public class TowerSlot   
    {
        public Transform floor;     
        public Transform snapPoint; 
    }

    public TowerSlot[] towers;      
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
            GameObject blockPrefab = blockPool[bIndex];
            blockPool.RemoveAt(bIndex);

            int tIndex = Random.Range(0, towers.Length);
            TowerSlot slot = towers[tIndex];

            Vector3 spawnPos = slot.snapPoint.position + new Vector3(0, 2f, 0);
            GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);

            Block block = newBlock.GetComponent<Block>();
            if (block != null)
            {
                Tower towerComponent = slot.floor.GetComponentInParent<Tower>();
                if (towerComponent != null)
                {
                    block.currentTower = towerComponent;
                    towerComponent.blocks.Add(block);
                }
                else
                {
                    Debug.LogWarning("No Tower component found above floor: " + slot.floor.name);
                }
            }
            else
            {
                Debug.LogWarning("Spawned object has no Block component: " + newBlock.name);
            }
        }
    }
}
