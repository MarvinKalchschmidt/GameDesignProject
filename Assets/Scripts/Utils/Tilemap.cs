using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    [SerializeField] private GridTile _tile;
    [SerializeField] private Color _colorUnoccupied;
    [SerializeField] private Color _colorOccupied;
    private CustomGrid<GridTowerObject> _grid;

    private CustomGrid<GridTile> _tilemap;

    public void InitTilemap(CustomGrid<GridTowerObject> grid)
    {
        _tilemap = new CustomGrid<GridTile>(grid.Width, grid.Height, grid.CellHeight, grid.OriginPosition);
        _grid = grid;

        for (int x = 0; x < _tilemap.Width; x++)
        {
            for (int y = 0; y < _tilemap.Height; y++)
            {
                _tilemap[x,y] = SpawnTile(_tile, x, y);
            }
        }
        UpdateTileMap();
    }

    private GridTile SpawnTile(GridTile tile, int x, int y)
    {
        return Instantiate(tile, new Vector3(x + 0.5f, y + 0.5f) * _grid.CellHeight, Quaternion.identity, transform);
    }

    public void UpdateTileMap()
    {
        for (int x = 0; x < _tilemap.Width; x++)
        {
            for (int y = 0; y < _tilemap.Height; y++)
            {
                GridTowerObject gridTowerObject = _grid.GetObject(x, y);

                if (gridTowerObject.OccupiedCheck())
                {
                    _tilemap[x, y].UpdateColor(_colorUnoccupied);
                } 
                else
                {
                    _tilemap[x, y].UpdateColor(_colorOccupied);
                }
            }
        }
    }
}
