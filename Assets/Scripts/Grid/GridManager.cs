using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap _map;
    [SerializeField] private List<TileData> _tileDatas;
    private Dictionary<TileBase, TileData> _dataFromTiles;

    private Vector3Int _gridSize;
    private Vector3Int _gridPosition;
    private Vector3 _gridCellSize;

    private void Awake()
    {
        SetGridSize();
        _dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in _tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                _dataFromTiles.Add(tile, tileData);
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
            Vector3Int gridCellPosition = _map.WorldToCell(mouseVector3);
            TileBase clickedTile = _map.GetTile(gridCellPosition);
            Debug.Log("At position " + gridCellPosition + " there is tile "  + clickedTile + "movement cost: " + _dataFromTiles[clickedTile].movementCost);
        }
    }

    private void SetGridSize()
    {
        _map.CompressBounds();
        _gridSize = _map.cellBounds.size;
        _gridPosition = _map.cellBounds.position;
        _gridCellSize = _map.cellSize;

    }

    public Vector3Int GetGridSize()
    {
        return _gridSize;
    }

    public Vector3Int GetGridPosition()
    {
        return _gridPosition;
    }

    public Vector3 GetGridCellSize()
    {
        return _gridCellSize;
    }

    public void DestroyTile(Vector3Int position)
    {
        _map.SetTile(position, null);
    }
}
