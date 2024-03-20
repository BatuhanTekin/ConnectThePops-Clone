using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _game.BaseClasses;
using _Game.Scripts.Bubble;
using _game.Utility.ObjectPooling;
using Unity.Mathematics;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField]
        private GridController m_gridControllerPrefab;


        [SerializeField]
        private int m_gridSize = 5;

        [SerializeField]
        private float m_gridScales = 0.8f;

        private GridController[,] _gridControllers;
        private Pool<BubbleController> _bubblePool;
        private List<Task> _taskList = new();
        private GridSaveData _saveData;

        private GridController _gridObject;
        private BubbleManager _bubbleManager;


        private void Start()
        {
            _bubbleManager = BubbleManager.Instance;
            CreateGrid();
        }

        private void CreateGrid()
        {
            _gridControllers = new GridController[m_gridSize, m_gridSize];

            if (GridSaver.GetSave(out _saveData))
            {
                SetData();
                return;
            }

            CreateNewData();
        }

        private void SetData()
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                for (int x = 0; x < m_gridSize; x++)
                {
                    _gridObject = Instantiate(m_gridControllerPrefab, transform);
                    _gridObject.transform.localPosition = new Vector2(x, y) * m_gridScales;
                    _gridControllers[y, x] = _gridObject;
                    _gridObject.name += $"_{y},{x}";
                    _gridObject.Init((y, x), _bubbleManager.GetBubble(_saveData.list[(m_gridSize * x) + y]));
                }
            }
        }

        private void CreateNewData()
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                for (int x = 0; x < m_gridSize; x++)
                {
                    _gridObject = Instantiate(m_gridControllerPrefab, transform);
                    _gridObject.transform.localPosition = new Vector2(x, y) * m_gridScales;
                    _gridControllers[y, x] = _gridObject;
                    _gridObject.name += $"_{y},{x}";
                    _gridObject.Init((y, x), _bubbleManager.GetRandomBubble());
                }
            }

            GridSaver.Save(_gridControllers);
        }

        public bool IsLinked(GridController lastGrid, GridController selectedGrid)
        {
            if (lastGrid.x == selectedGrid.x)
            {
                return math.abs(lastGrid.y - selectedGrid.y) <= 1;
            }

            if (lastGrid.y == selectedGrid.y)
            {
                return math.abs(lastGrid.x - selectedGrid.x) <= 1;
            }

            if (math.abs(lastGrid.x - selectedGrid.x) <= 1 && math.abs(lastGrid.y - selectedGrid.y) <= 1)
            {
                return true;
            }

            return false;
        }

        public async Task OnMatchCompleted()
        {
            for (int x = 0; x < m_gridSize; x++)
            {
                for (int y = 0; y < m_gridSize; y++)
                {
                    _gridObject = _gridControllers[y, x];
                    if (_gridObject.IsEmpty())
                    {
                        FindBubbleOnColumn(_gridObject, y, x);
                    }
                }
            }

            await Task.WhenAll(_taskList);

            if (!HasMatch())
            {
                AddMatchedValue();
            }

            foreach (var grid in _gridControllers)
            {
                if (grid.IsEmpty())
                {
                    grid.AddNewBubble(_bubbleManager.GetRandomBubble());
                }
            }

            GridSaver.Save(_gridControllers);
        }

        private void AddMatchedValue()
        {
            for (int x = 0; x < m_gridSize; x++)
            {
                for (int y = 0; y < m_gridSize; y++)
                {
                    if (!_gridControllers[y, x].IsEmpty())
                    {
                        continue;
                    }

                    if (FindNeighborAndSetValue(_gridControllers[y, x]))
                    {
                        return;
                    }
                }
            }
        }

        // For Break Deadlock
        private bool FindNeighborAndSetValue(GridController gridController)
        {
            var x = gridController.x;
            var y = gridController.y;

            for (int i = x - 1; i < x + 2; ++i)
            {
                for (int j = y - 1; j < y + 2; ++j)
                {
                    if (i == x && y == j || i < 0 || j < 0 
                        || i >= m_gridSize || j >= m_gridSize)
                    {
                        continue;
                    }

                    if (!_gridControllers[i, j].IsEmpty() &&
                        Math.Abs(i - x) <= 1 && Math.Abs(j - y) <= 1)
                    {
                        gridController.AddNewBubble(_bubbleManager.GetBubble(_gridControllers[i, j].GetLevel()));
                        return true;
                    }
                }
            }

            return false;
        }

        private bool HasMatch()
        {
            for (int x = 0; x < m_gridSize; x++)
            {
                for (int y = 0; y < m_gridSize; y++)
                {
                    if (_gridControllers[y, x].IsEmpty())
                    {
                        continue;
                    }

                    if (CheckMatch(_gridControllers[y, x]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckMatch(GridController gridController)
        {
            var level = gridController.GetLevel();
            var x = gridController.x;
            var y = gridController.y;

            for (int i = x - 1; i < (x + 2); ++i)
            {
                for (int j = y - 1; j < y + 2; ++j)
                {
                    if (i == x && y == j || i < 0 || j < 0 
                        || i >= m_gridSize || j >= m_gridSize)
                    {
                        continue;
                    }

                    if (!_gridControllers[i, j].IsEmpty() &&
                        _gridControllers[i, j].GetLevel() == level &&
                        Math.Abs(i - x) <= 1 && Math.Abs(j - y) <= 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void FindBubbleOnColumn(GridController gridObject, int y, int x)
        {
            for (int i = y; i < m_gridSize; i++)
            {
                if (_gridControllers[i, x].IsEmpty())
                {
                    continue;
                }

                _gridControllers[i, x].RemoveBubble(out var bubble);
                gridObject.AddBubble(bubble);
                _taskList.Add(gridObject.MoveBubble());
                return;
            }
        }
    }
}