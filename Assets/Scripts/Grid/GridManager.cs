using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap _map;
    [SerializeField] private Tilemap _indicatorMap;
    [SerializeField] private List<TileData> _tileDatas;
    [SerializeField] private Tile _deployTile;
    [SerializeField] private Tile _attackTile;
    [SerializeField] private Tile _movementTile;
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
    
    public void ChangeColor(int x, int y, Color color)
    {
        int tilePositionX = _gridPosition.x + x;
        int tilePositionY = _gridPosition.y + y;
        
        if (color == Color.red)
        {
            _indicatorMap.SetTile(new Vector3Int(tilePositionX, tilePositionY, 0), _attackTile);
        }
        
        if (color == Color.yellow)
        {
            _indicatorMap.SetTile(new Vector3Int(tilePositionX, tilePositionY, 0), _deployTile);
        }
        
        if (color == Color.blue)
        {
            _indicatorMap.SetTile(new Vector3Int(tilePositionX, tilePositionY, 0), _movementTile);
        }
    }

    public void ResetColor(int x, int y)
    {
        int tilePositionX = _gridPosition.x + x;
        int tilePositionY = _gridPosition.y + y;
        _map.SetTileFlags(new Vector3Int(tilePositionX, tilePositionY, 0), TileFlags.None);
        
        _indicatorMap.SetTile(new Vector3Int(tilePositionX, tilePositionY, 0), null);
    }
    
    public TileData GetTileData(int x, int y)
    {
        int tilePositionX = _gridPosition.x + x;
        int tilePositionY = _gridPosition.y + y;
        TileBase clickedTile = _map.GetTile(new Vector3Int(tilePositionX, tilePositionY, 0));
        Debug.Log(clickedTile);
        return _dataFromTiles[clickedTile];  
    }
    
    private void SetGridSize()
    {
        _map.CompressBounds();
        _gridSize = _map.cellBounds.size;
        _gridPosition = _map.cellBounds.position;
        _gridCellSize = _map.cellSize;
    }
}
