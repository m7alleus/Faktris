using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Text TimerUI;

    Block currentBlock;
    Block previewBlock;

    [SerializeField]
    bool withGhost;
    float ghostTransparency = .3f;
    Block ghost;

    Vector3 fallingBlockPosition = new Vector3(5, 18, 0);

    Spawner spawner;
    bool canSpawn;

    Holder holder;
    bool canHold;
    Block holdedBlock;

    void Start() {
        FindObjectOfType<GridManager>().IsFull += StopGame;
        canHold = true;
        canSpawn = true;
        spawner = FindObjectOfType<Spawner>();
        holder = FindObjectOfType<Holder>();
    }

    void UpdateTimer() {
        float secondsSurvived = Mathf.Round(Time.timeSinceLevelLoad * 100.0f) / 100.0f;
        TimerUI.text = secondsSurvived.ToString();
    }

    void Update() {
        UpdateTimer();

        if (canSpawn) {
            Spawn();
        }

        if (withGhost && ghost != null) {
            ghost.transform.position = currentBlock.FindClosestFloorPosition();
            ghost.transform.rotation = currentBlock.transform.rotation;
        }

        if (Input.GetKey(KeyCode.C) && canHold && currentBlock != null) {
            Hold();
        }
    }

    void Spawn() {
        if (previewBlock == null) {
            previewBlock = spawner.SpawnRandomBlock();
        }

        if (currentBlock == null) {
            TriggerBlock(previewBlock);
            previewBlock = spawner.SpawnRandomBlock();
        }

        canSpawn = false;
    }

    void Hold() {
        if (holdedBlock == null) {
            holdedBlock = currentBlock;
            holder.PlaceBlockOnHold(currentBlock);
        } else {
            Block previousHoldedBlock = holdedBlock;
            holdedBlock = currentBlock;
            holder.PlaceBlockOnHold(currentBlock);
            TriggerBlock(previousHoldedBlock);
        }
        canHold = false;
    }

    public void TriggerBlock(Block block) {
        currentBlock = block;
        currentBlock.currentState = Block.State.Falling;
        currentBlock.transform.position = fallingBlockPosition;
        currentBlock.IsStill += MarkAsReadyToSpawn;
        currentBlock.IsStill += MarkAsReadyToHold;
        CreateGhost();
    }

    void CreateGhost() {
        if (withGhost) {
            if (ghost != null) {
                Destroy(ghost.gameObject);
            }
            ghost = spawner.InstantiateBlock(currentBlock);
            ghost.currentState = Block.State.Inactive;
            ghost.SetTransparency(ghostTransparency);
        }
    }

    public void MarkAsReadyToSpawn() {
        currentBlock = null;
        canSpawn = true;
    }

    void MarkAsReadyToHold() {
        canHold = true;
    }

    void StopGame() {
        this.enabled = false;
    }
}
