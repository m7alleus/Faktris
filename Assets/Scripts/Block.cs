using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum State { Preview, Falling, Still };
    public State currentState = State.Preview ;
    public event System.Action IsStill;

    static readonly float moveInterval = .8f;
    float moveTimer;

    GridManager gridManager;

    void Start() {
        moveTimer = moveInterval;
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update() {
        if(currentState == State.Falling) {
            MoveDownPeriodically();
            HandlePlayerMovement();
            HandlePlayerRotation();
        }
    }

    void MoveDownPeriodically() {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0) {
            moveTimer = moveInterval;
            if (ValidNextPosition(Vector3.down)) {
                transform.position += Vector3.down;
            } else {
                // the block is on top of another one or is on the floor, it's dead
                DisableAndRegisterToGrid();
            }
        }
    }

    public void SetAsFalling() {
        currentState = State.Falling;
    }

    void HandlePlayerMovement() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (ValidNextPosition(Vector3.right)) {
                transform.position += Vector3.right;
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (ValidNextPosition(Vector3.left)) {
                transform.position += Vector3.left;
            }
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            if (ValidNextPosition(Vector3.down)) {
                transform.position += Vector3.down;
            } else {
                // the block is on top of another one or is on the floor, it's dead
                DisableAndRegisterToGrid();
            }
        }
    }

    void HandlePlayerRotation() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // it's a bit junky
            // try to rotate around a defined point for each block
            // TODO: prevent rotation if invalid
            transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    bool ValidNextPosition(Vector3 direction) {
        foreach (Transform square in transform) {
            Vector3 currentSquarePosition = square.transform.position;
            if (!ValidSquarePosition(currentSquarePosition + direction)) {
                return false;
            }
        }
        return true;
    }

    bool ValidSquarePosition(Vector3 squarePosition) {
        int x = Mathf.FloorToInt(squarePosition.x);
        int y = Mathf.FloorToInt(squarePosition.y);

        if (x < 0 || x >= gridManager.width || y < 0 || gridManager.grid[x, y] != null) {
            return false;
        }

        return true;
    }

    void DisableAndRegisterToGrid() {
        currentState = State.Still;
        this.enabled = false;
        if (IsStill != null) {
            IsStill();
        }
        gridManager.Register(this);
    }
}
