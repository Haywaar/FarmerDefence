using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

public class EnemyPositionGrid
{
    public const int NUM_CELLS = 50; //square = NUM_CELLS x NUM_CELLS
    public const int CELL_SIZE = 1;

    private LinkedList<AbstractEnemy>[,] _enemiesInCells;

    public LinkedList<AbstractEnemy>[,] EnemiesInCells => _enemiesInCells;

    public EnemyPositionGrid()
    {
        _enemiesInCells = new LinkedList<AbstractEnemy>[NUM_CELLS, NUM_CELLS];
        for (int i = 0; i < NUM_CELLS; i++)
        {
            for (int j = 0; j < NUM_CELLS; j++)
            {
                _enemiesInCells[i, j] = new LinkedList<AbstractEnemy>();
            }
        }
    }

    public void OnEnemyMoved(AbstractEnemy enemy, Vector3 newPos)
    {
        var position = enemy.transform.position;
        int oldCellX = (int)(position.x / CELL_SIZE);
        int oldCellY = (int)(position.z / CELL_SIZE);

        int cellX = (int)(newPos.x / CELL_SIZE);
        int cellY = (int)(newPos.z / CELL_SIZE);

        if (oldCellX == cellX && oldCellY == cellY)
            return;

        if (_enemiesInCells[oldCellX, oldCellY].Contains(enemy))
        {
            _enemiesInCells[oldCellX, oldCellY].Remove(enemy);
        }

        if (!_enemiesInCells[cellX, cellY].Contains(enemy))
        {
            _enemiesInCells[cellX, cellY].AddLast(enemy);
        }
    }

    public Vector2Int CalculateCellNumber(Vector3 position)
    {
        int cellX = (int)(position.x / CELL_SIZE);
        int cellY = (int)(position.z / CELL_SIZE);
        return new Vector2Int(cellX, cellY);
    }

    private List<Vector2Int> GetCellsInRadius(Vector3 position, float radius)
    {
        var curPosCell = CalculateCellNumber(position);

        var cells = new List<Vector2Int>();
        int cellRadius = Mathf.CeilToInt(radius / CELL_SIZE);

        var minIndexX = Mathf.Max(curPosCell.x - cellRadius, 0);
        var minIndexY = Mathf.Max(curPosCell.y - cellRadius, 0);

        var maxIndexX = Mathf.Min(curPosCell.x + cellRadius, NUM_CELLS - 1);
        var maxIndexY = Mathf.Min(curPosCell.y + cellRadius, NUM_CELLS - 1);

        for (int i = minIndexX; i < maxIndexX; i++)
        {
            for (int j = minIndexY; j < maxIndexY; j++)
            {
                cells.Add(new Vector2Int(i, j));
            }
        }

        return cells;
    }

    public List<AbstractEnemy> GetEnemiesInRadius(Vector3 position, float radius)
    {
        var enemiesList = new List<AbstractEnemy>();
        var cells = GetCellsInRadius(position, radius);

        foreach (var cell in cells)
        {
            foreach (var enemy in _enemiesInCells[cell.x, cell.y])
            {
                if (!enemy.IsDead)
                {
                    enemiesList.Add(enemy);
                }
            }
        }

        enemiesList.RemoveAll(x => Vector3.Distance(x.transform.position, position) > radius);
        //TODO - To find out why distinct required
        return enemiesList.Distinct().ToList();
    }
}