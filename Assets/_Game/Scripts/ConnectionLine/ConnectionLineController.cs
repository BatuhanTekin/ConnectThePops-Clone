using _game.Utility.ObjectPooling;
using UnityEngine;

namespace _Game.Scripts.ConnectionLine
{
    public class ConnectionLineController : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private SpriteRenderer m_renderer;

        private float _distance;
        private Transform _transform;
        private float _angle;
        private Vector2 _direction;
        
        public void SetConnection(Vector2 bubbleStart, Vector2 bubbleEnd, Color color)
        {
            _transform.position = bubbleStart;
            _distance = Vector2.Distance(bubbleStart, bubbleEnd);
            _transform.localScale = new Vector2(_distance, _transform.localScale.y);

            _direction = bubbleEnd - bubbleStart;
            _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

            _transform.rotation = Quaternion.Euler(0, 0, _angle);

            m_renderer.color = color;
        }

        public void OnSpawn()
        {
            _transform = transform;
        }

        public void SetDisable()
        {
            m_renderer.enabled = false;
        }

        public void SetEnable()
        {
            m_renderer.enabled = true;
        }

        public bool IsDisabled()
        {
            return !m_renderer.enabled;
        }
    }
}