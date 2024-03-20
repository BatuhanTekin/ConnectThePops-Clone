using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Bubble
{
    [CreateAssetMenu(fileName = "BubbleDataSo", menuName = "PopGame/Bubble/BubbleData", order = 0)]
    public class BubbleDataSo : ScriptableObject
    {
        public Color[] bubbleColorArray;
        public Sprite[] bubbleSpriteArray;
        public float spriteFrequency;
        public int levelTargetMerge;

        public (Sprite sprite, Color color) GetData(int level)
        {
            return (GetSprite(level), GetColor(level));
        }

        public Sprite GetSprite(int level)
        {
            return bubbleSpriteArray[(int)(level / spriteFrequency) % bubbleSpriteArray.Length];
        }

        public Color GetColor(int level)
        {
            return bubbleColorArray[level % bubbleColorArray.Length];
        }
    }
}