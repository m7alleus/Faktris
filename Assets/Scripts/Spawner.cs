using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    List<Block> blockPrefabs;

    Block currentBlock;
    Block previewBlock;
    bool canSpawn;

    void Start() {
        canSpawn = true;
    }

    void Update() {
        if (canSpawn) {
            Spawn();
        }
    }

    Block SpawnBlockInPreview() {
        return Instantiate(SelectRandomBlock(), transform.position, Quaternion.identity) as Block;
    }

    void Spawn() {
        canSpawn = false;
        previewBlock = SpawnBlockInPreview();
        if (currentBlock == null) {
            currentBlock = previewBlock;
            previewBlock = SpawnBlockInPreview();
        }
        currentBlock.transform.position = transform.position + (Vector3.down * 4);
        currentBlock.SetAsFalling();
        currentBlock.IsStill += MarkAsReadyToSpawn;
    }

    Block SelectRandomBlock() {
        int randomIndex = Random.Range(0, blockPrefabs.Count);
        return blockPrefabs[randomIndex];
    }

    void MarkAsReadyToSpawn() {
        currentBlock = previewBlock;
        canSpawn = true;
    }
}
