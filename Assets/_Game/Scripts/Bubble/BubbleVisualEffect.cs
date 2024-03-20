using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace _Game.Scripts.Bubble
{
    [Serializable]
    public struct BubbleVisualEffect
    {
        [SerializeField]
        private float m_animationDuration;

        [SerializeField]
        private float m_animationScaleDuration;

        [SerializeField]
        private float m_animationDropScaleDuration;

        [SerializeField]
        private float m_animationTargetScale;
        [SerializeField]
        private Transform m_visualTarget;

        [SerializeField]
        private float m_animationSpawnDuration;

        [SerializeField]
        private float m_animationSelectDuration;

        [SerializeField]
        private float m_animationSelectScale;

        [SerializeField]
        private float m_animationMatchDuration; 
        [SerializeField]
        private float m_animationOnMatchedDuration;
        [SerializeField]
        private float m_animScaleMultiplier;

        private Transform _transform;
        private Sequence _sequence;
        private Vector3 _localScale;
        private Tween _tween;

        public void Init(Transform transform)
        {
            _transform = transform;
            _localScale = _transform.localScale;
        }
        
        public async Task PlayDropEffect(float targetY)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_transform.DOMoveY(targetY, m_animationDuration).SetEase(Ease.OutSine));
            _sequence.Append(m_visualTarget.DOScaleY(m_animationTargetScale, m_animationScaleDuration));
            _sequence.Append(m_visualTarget.DOScaleY(_localScale.y, m_animationDropScaleDuration));
            await _sequence.AsyncWaitForCompletion();
        }
        
        public void SpawnEffect()
        {
            _transform.localScale = Vector3.zero;
            _transform.DOScale(_localScale, m_animationSpawnDuration);
        }
        
        public void SelectEffect(bool isSelected)
        {
            _tween?.Kill();
            if (isSelected)
            {
                _tween = _transform.DOScale(m_animationSelectScale, m_animationSelectDuration);
                return;
            }

            _tween = _transform.DOScale(_localScale, m_animationSelectDuration);
        }

        public void OnMatchedEffect()
        {
            _tween?.Kill();
            _transform.localScale = _localScale;
            _tween = _transform.DOPunchScale(Vector3.one / m_animScaleMultiplier, m_animationOnMatchedDuration, 3);
        }


        public async Task MatchEffect(Transform targetPosition)
        {
            await _transform.DOMove(targetPosition.position, m_animationMatchDuration).AsyncWaitForCompletion();
        }
    }
}