using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    public enum State { Preview, Falling, Still };
    public State currentState = State.Preview;
    public event System.Action IsStill;

    static readonly float moveInterval = .8f;
    float moveTimer;

    GridManager gridManager;

    void Awake() {
        moveTimer = moveInterval;
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update() {
        if (currentState == State.Falling) {
            MoveDownPeriodically();
            HandleStrafing();
            HandleRotating();
        }
    }

    public Vector3 FindClosestFloorPosition() {
        int rowIndexDistance = FindClosestHittingRowIndex();
        return transform.position + Vector3.down * rowIndexDistance;
    }

    int FindClosestHittingRowIndex() {
        for (int i = 1; i <= Mathf.FloorToInt(transform.position.y); i++) {
            if (!ValidPosition(Vector3.down * i)) {
                // the previous row is the closest one
                return i - 1;
            }
        }

        return 0;
    }

    void MoveDownPeriodically() {
        if (Input.GetKey(KeyCode.DownArrow)) {
            moveTimer -= Time.deltaTime * 20;
        } else {
            moveTimer -= Time.deltaTime;
        }
        if (moveTimer <= 0) {
            moveTimer = moveInterval;
            if (ValidPosition(Vector3.down)) {
                transform.position += Vector3.down;
            } else {
                // the block is on top of another one or is on the floor, it's dead
                SetAsStillAndRegisterToGrid();
            }
        }
    }

    public void SetAsFalling() {
        currentState = State.Falling;
    }

    void HandleStrafing() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (ValidPosition(Vector3.right)) {
                transform.position += Vector3.right;
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (ValidPosition(Vector3.left)) {
                transform.position += Vector3.left;
            }
        }
    }

    void HandleRotating() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            // it's a bit junky
            // try to rotate around a defined point for each block
            transform.Rotate(new Vector3(0, 0, -90));
            if (!ValidPosition(Vector3.zero)) {
                transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }

    bool ValidPosition(Vector3 direction) {
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

    void SetAsStillAndRegisterToGrid() {
        currentState = State.Still;
        if (IsStill != null) {
            IsStill();
        }
        gridManager.Register(this);
    }

    public void SetTransparency(float transparency) {
        foreach (Transform square in transform) {
            SpriteRenderer renderer = square.GetComponent<SpriteRenderer>();
            Color colorWithTransparency = renderer.material.color;
            colorWithTransparency.a = transparency;
            renderer.material.color = colorWithTransparency;
        }
    }
}
