using System.Collections.Generic;
using System.Linq;
using _game.BaseClasses;
using _Game.Scripts.Bubble;
using _game.Utility.ObjectPooling;
using UnityEngine;

namespace _Game.Scripts.ConnectionLine
{
    public class ConnectionLineManager : MonoSingleton<ConnectionLineManager>
    {
        [SerializeField] private GameObject m_linePrefab;
        [SerializeField] private Transform m_poolParent;
        [SerializeField] private BubbleDataSo m_dataSo;

        private List<ConnectionLineController> _connectedLines = new();
        private Pool<ConnectionLineController> _pool;
        private ConnectionLineController _line;

        private void Awake()
        {
            _pool = new Pool<ConnectionLineController>(new PrefabFactory<ConnectionLineController>(m_linePrefab, m_poolParent));
        }

        public void SetConnection(Transform startBubble, Transform endBubble, int level)
        {
            _line = _pool.GetObject();
            _line.SetConnection(startBubble.position, endBubble.position, m_dataSo.GetColor(level));
            _connectedLines.Add(_line);
        }

        public void CloseLastLine()
        {
            if (_connectedLines.Count < 0)
            {
                return;
            }
            
            _pool.ReturnPool(_connectedLines.Last());
            _connectedLines.Remove(_connectedLines.Last());
        }

        public void CloseAll()
        {
            foreach (var line in _connectedLines)
            {
                _pool.ReturnPool(line);
            }
            _connectedLines.Clear();
        }
    }
}

