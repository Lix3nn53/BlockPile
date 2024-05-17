using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Lix.Core;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private float _offsetX = 1;
    [SerializeField] private float _offsetY = 1;
    private float _offsetHeigth = 1;
    [SerializeField] private float _cellSize = 1;
    public SerializedDictionary<Vector2Int, GridSlot> Slots = new SerializedDictionary<Vector2Int, GridSlot>();
    private GameObjectPool _gridSlotPool;

    // Cache
    [SerializeField] private float _unit;
    [SerializeField] private float _unitHalf;

    private void Start()
    {
        _gridSlotPool = AssetManager.Instance.GridSlotPool;
        _offsetHeigth = GameManager.Instance.BlockHeight / 2f;

        _unit = _cellSize * Mathf.Sqrt(2);
        _unitHalf = _unit / 2;

        SerializedDictionary<Vector2Int, GridSlot> dictionary = new SerializedDictionary<Vector2Int, GridSlot>();

        foreach (Vector2Int pos in Slots.Keys)
        {
            GridSlot gridSlot = _gridSlotPool.Pool.Get().GetComponent<GridSlot>();
            dictionary.Add(pos, gridSlot);

            gridSlot.GridPosition = pos;
            gridSlot.transform.position = GridToWorldPosition(pos);
            gridSlot.gameObject.SetActive(true);
        }

        Slots = dictionary;
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        // Assuming the grid starts at (0,0) and extends in the positive X and Z directions
        // You may need to adjust this based on your specific grid layout
        float x = (_offsetX + gridPos.x) * _unit;
        float z = (_offsetY + gridPos.y) * _unitHalf;

        if (gridPos.y % 2 == 0)
        {
            x += _unitHalf;
        }

        // Apply rotation to the position
        Quaternion rotation = Quaternion.Euler(0f, 45f, 0f); // 45 degrees around the Y axis
        Vector3 rotatedPosition = rotation * new Vector3(x, _offsetHeigth, z);

        return rotatedPosition;
    }
}
