using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    List<Block> blockPrefabs;

    public Block currentBlock;
    Block previewBlock;

    [SerializeField]
    bool withGhost;
    float ghostTransparency = .3f;
    public Block ghost; // should not be public

    bool canSpawn;
    Vector3 defaultBlockPosition = new Vector3(5, 18, 0);

    List<Block> spawningQueue = new List<Block>();
    List<Block> spawningDequeue = new List<Block>();

    void Start() {
        canSpawn = true;
    }

    void Update() {
        if (canSpawn) {
            Spawn();
        }

        if (withGhost && ghost != null) {
            ghost.transform.position = currentBlock.FindClosestFloorPosition();
            ghost.transform.rotation = currentBlock.transform.rotation;
        }
    }

    Block SpawnRandomBlockInPreview() {
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

    Block InstantiateBlock(Block block) {
        return Instantiate(block, transform.position, Quaternion.identity) as Block;
    }

    public void Spawn() {
        canSpawn = false;
        if (previewBlock == null) {
            previewBlock = SpawnRandomBlockInPreview();
        }

        if (currentBlock == null) {
            TriggerBlock(previewBlock);
            previewBlock = SpawnRandomBlockInPreview();
        }
    }

    public void TriggerBlock(Block block) {
        currentBlock = block;
        currentBlock.transform.position = defaultBlockPosition;
        currentBlock.currentState = Block.State.Falling;
        currentBlock.IsStill += MarkAsReadyToSpawn;
        CreateGhost();
    }

    void CreateGhost() {
        if (withGhost) {
            if (ghost != null) {
                Destroy(ghost.gameObject);
            }
            ghost = InstantiateBlock(currentBlock);
            ghost.currentState = Block.State.Inactive;
            ghost.SetTransparency(ghostTransparency);
        }
    }

    public void MarkAsReadyToSpawn() {
        currentBlock = null;
        canSpawn = true;
    }
}
