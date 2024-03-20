using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.ConnectionLine;
using _Game.Scripts.Map;
using _Game.Scripts.Ui.LevelUi;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private ResultBubbleController m_resultBubble;
        [SerializeField] private LevelUiController m_levelUi;
        public static Action<GridController> OnGridSelected;

        private List<GridController> _selections = new();
        private MapManager _mapManager;
        private GridController _lastGrid;
        private ConnectionLineManager _connectionLineManager;
        private bool _enable;

        private List<Task> _asyncList = new();
        private int _totalLevel;

        private void OnEnable()
        {
            OnGridSelected += OnGridSelect;
        }
        private void OnDisable()
        {
            OnGridSelected -= OnGridSelect;
        }

        private void Start()
        {
            _mapManager = MapManager.Instance;
            _connectionLineManager = ConnectionLineManager.Instance;
            _enable = true;
        }

        private void Update()
        {
            if (!_enable)
            {
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (_selections.Count > 1)
                {
                    MatchGrids();
                    return;
                }

                ClearSelection();
            }
        }

        private void OnGridSelect(GridController grid)
        {
            if (!_enable)
            {
                return;
            }
            
            if (_selections.Count <= 0)
            {
                _selections.Add(grid);
                _lastGrid = grid;
                grid.OnSelect();
                SetTotalPoint();
            }

            if (grid.GetLevel() != _lastGrid.GetLevel()) 
            {
                return;
            }

            if (_selections.Count > 1)
            {
                if (_selections[^2] == grid)
                {
                    RemoveLastGrid(grid);
                    SetTotalPoint();
                    return;
                }
            }

            if (_selections.Contains(grid))
            {
                return;
            }
            
            if (_mapManager.IsLinked(grid, _lastGrid))
            {
                AddGrid(grid);
                SetTotalPoint();
                
            }
        }

        private void SetTotalPoint()
        {
            for (int i = 1; i < 7; i++)
            {
                if (_selections.Count < math.pow(2, i))
                {
                    _totalLevel = _lastGrid.GetLevel() + i - 1;
                    m_resultBubble.SetBall(_totalLevel);
                    return;
                }
            }
        }

        private void AddGrid(GridController grid)
        {
            _connectionLineManager.SetConnection(grid.transform, _lastGrid.transform, grid.GetLevel());
            _selections.Add(grid);
            grid.OnSelect();
            _lastGrid = grid;
        }

        private void RemoveLastGrid(GridController grid)
        {
            _lastGrid.ResetSelection();
            _selections.Remove(_lastGrid);
            _connectionLineManager.CloseLastLine();
            _lastGrid = grid;
        }

        private async void MatchGrids()
        {
            _connectionLineManager.CloseAll();
            _asyncList.Clear();
            _enable = false;
            m_resultBubble.Close();
            
            foreach(var grid in _selections)
            {
                if (_lastGrid == grid)
                {
                    continue;
                }

                _asyncList.Add(grid.OnMatch(_lastGrid));
            }
            
            await Task.WhenAll(_asyncList);
            m_levelUi.OnMerge((int) Math.Pow(2, _totalLevel + 1));
            _lastGrid.UpdateLevel(_totalLevel);
            await _mapManager.OnMatchCompleted();
            
            _enable = true;
            _selections.Clear();
        }

        private void ClearSelection()
        {
            foreach (var grid in _selections)
            {
                grid.ResetSelection();
            }
            _selections.Clear();
            _connectionLineManager.CloseAll();
            m_resultBubble.Close();
        }
    }
}