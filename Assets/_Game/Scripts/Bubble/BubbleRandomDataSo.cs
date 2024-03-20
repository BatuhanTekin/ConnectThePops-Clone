using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Bubble
{
    [CreateAssetMenu(fileName = "BubbleRandomDataSo", menuName = "PopGame/Bubble/BubbleRandomDataSo", order = 0)]
    public class BubbleRandomDataSo : ScriptableObject
    {
        public RandomData[] randomData;
        
        private float _random;
        private float _currentProbability;

        public int GetRandomLevel()
        {
            _random = Random.Range(0f, 100f);
            _currentProbability = 0f;

            foreach (var data in randomData)
            {
                if (_random < data.randomValue + _currentProbability)
                {
                    return data.level;
                }
                
                _currentProbability += data.randomValue;
            }

            return randomData[0].level;
        }
        
    }

    [Serializable]
    public struct RandomData
    {
        public int level;
        public float randomValue;
    }
}