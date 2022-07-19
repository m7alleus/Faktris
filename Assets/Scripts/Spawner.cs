using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    List<Block> blockPrefabs;

    List<Block> spawningQueue = new List<Block>();
    List<Block> spawningDequeue = new List<Block>();

    void Start() {
        blockPrefabs = new List<Block>(blockPrefabs);
    }
    public Block SpawnRandomBlock() {
        if (spawningQueue.Count == 0) {
            spawningQueue = new List<Block>(blockPrefabs);
            spawningDequeue.Clear();
        }

        int randomIndex = Random.Range(0, spawningQueue.Count);
        Block randomBlock = spawningQueue[randomIndex];

        spawningQueue.Remove(randomBlock);
        spawningDequeue.Add(randomBlock);

        return InstantiateBlock(randomBlock);
    }

    public Block InstantiateBlock(Block block) {
        return Instantiate(block, transform.position, Quaternion.identity) as Block;
    }
}
