using _game.BaseClasses;
using _game.Utility.ObjectPooling;
using UnityEngine;

namespace _Game.Scripts.Bubble
{
    public class BubbleManager : MonoSingleton<BubbleManager>
    {
        [SerializeField] private BubbleRandomDataSo m_randomData; 
        [SerializeField] private GameObject m_bubblePrefab; 
        [SerializeField] private Transform m_poolParent; 
        
        private Pool<BubbleController> _bubblePool;
        private BubbleController _bubble;

        private void Awake()
        {
            _bubblePool = new Pool<BubbleController>(new PrefabFactory<BubbleController>(m_bubblePrefab, m_poolParent));
        }

        public BubbleController GetRandomBubble()
        {
            _bubble = _bubblePool.GetObject();
            _bubble.SetBubble(m_randomData.GetRandomLevel());
            return _bubble;
        }
        
        public BubbleController GetBubble(int level)
        {
            _bubble = _bubblePool.GetObject();
            _bubble.SetBubble(level);
            return _bubble;
        }
    }
}