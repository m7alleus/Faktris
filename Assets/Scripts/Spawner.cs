using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    List<Block> blockPrefabs;

    Block currentBlock;
    Block previewBlock;

    [SerializeField]
    bool withGhost;
    Block ghost;
    float ghostTransparency = .3f;

    bool canSpawn;
    Vector3 defaultBlockPosition = new Vector3(5, 18, 0);
        
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
        int randomIndex = Random.Range(0, blockPrefabs.Count);
        Block randomBlock = blockPrefabs[randomIndex];
        return InstantiateBlock(randomBlock);
    }

    Block InstantiateBlock(Block block) {
        return Instantiate(block, transform.position, Quaternion.identity) as Block;
    }

    void Spawn() {
        canSpawn = false;
        previewBlock = SpawnRandomBlockInPreview();
        if (currentBlock == null) {
            currentBlock = previewBlock;
            previewBlock = SpawnRandomBlockInPreview();
        }
        if (withGhost) {
            if (ghost != null) {
                Destroy(ghost.gameObject);
            }
            ghost = InstantiateBlock(currentBlock);
            ghost.SetTransparency(ghostTransparency);
        }
        currentBlock.transform.position = defaultBlockPosition;
        currentBlock.SetAsFalling();
        currentBlock.IsStill += MarkAsReadyToSpawn;
    }

    void MarkAsReadyToSpawn() {
        if (ghost != null) {
            Destroy(ghost.gameObject);
        }
        currentBlock = previewBlock;
        canSpawn = true;
    }
}
