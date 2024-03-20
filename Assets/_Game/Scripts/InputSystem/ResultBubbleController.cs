using System;
using _Game.Scripts.Bubble;
using _game.Scripts.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.InputSystem
{
    public class ResultBubbleController : MonoBehaviour
    {
        [SerializeField] private BubbleDataSo m_dataSo;
        [SerializeField] private BubbleVisualController m_visualController;
        [SerializeField] private float m_animDuration;
        [SerializeField] private float m_animScaleMultiplier;
        
        private (Sprite sprite, Color color) _data;
        private Tween _tween;
        private int _level = -1;

        public void SetBall(int level)
        {
            _data = m_dataSo.GetData(level);
            m_visualController.Open();
            m_visualController.SetVisual(_data, level);

            if (level == _level)
            {
                return;
            }
            _level = level;
            _tween?.Kill();
            transform.localScale = Vector3.one;
            _tween = transform.DOPunchScale(Vector3.one / m_animScaleMultiplier, m_animDuration, 3);
        }

        public void Close()
        {
            _level = -1;
            _tween?.Kill();
            m_visualController.Close();
        }
    }
}