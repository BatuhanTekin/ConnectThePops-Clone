using System;
using _game.Scripts.Utility;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Bubble
{
    [Serializable]
    public struct BubbleVisualController
    {
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private SpriteRenderer m_inline;
        [SerializeField] private TextMeshProUGUI m_text;
        private string _number;

        public void SetVisual((Sprite sprite, Color color) data , int level)
        {
            _number = NumberConverter.ConvertToFormat((long)Math.Pow(2, level + 1));
            
            m_text.SetText(_number);
            m_inline.sprite = data.sprite;
            m_renderer.color = data.color;
        }

        public void Close()
        {
            m_renderer.enabled = false;
            m_inline.enabled = false;
            m_text.SetText("");
        }
        public void Open()
        {
            m_renderer.enabled = true;
            m_inline.enabled = true;
        }
    }
}