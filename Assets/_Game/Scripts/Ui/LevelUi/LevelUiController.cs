using System.Globalization;
using _Game.Scripts.Bubble;
using _game.Scripts.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Ui.LevelUi
{
    public class LevelUiController : MonoBehaviour
    {
        [SerializeField] private BubbleDataSo m_dataSo;
        [SerializeField] private Image m_fillBar;
        [SerializeField] private TextMeshProUGUI m_point;
        
        [SerializeField] private TextMeshProUGUI m_currentLevelText;
        [SerializeField] private Image m_currentLevelImage;
        
        [SerializeField] private TextMeshProUGUI m_nextLevelText;
        [SerializeField] private Image m_nextLevelImage;

        private const string PointPref = "TotalPoint";
        private const string MergePref = "TotalMerge";
        private const string LevelPref = "CurrentLevel";

        private void Start()
        {
            SetVisual();
            SetLevelText();
        }
        
        public void OnMerge(int point)
        {
            if (GetMerge() + 1 >= m_dataSo.levelTargetMerge)
            {
                AddLevel();
            }
            
            AddPoint(point);
            AddMerge();
            SetVisual();
        }

        private void SetVisual()
        {
            m_fillBar.DOFillAmount( (float) GetMerge() / m_dataSo.levelTargetMerge, 0.3f);
            m_point.text = NumberConverter.ConvertToFloatFormat(GetPoint());
        }

        private void SetLevelText()
        {
            m_currentLevelText.text = GetLevel().ToString(CultureInfo.InvariantCulture);
            m_currentLevelImage.color = m_dataSo.GetColor(GetLevel());
            
            m_fillBar.color = m_dataSo.GetColor(GetLevel());
            
            m_nextLevelText.text = (GetLevel() + 1).ToString(CultureInfo.InvariantCulture);
            m_nextLevelImage.color = m_dataSo.GetColor(GetLevel() + 1);

        }

        private void AddMerge()
        {
            PlayerPrefs.SetInt(MergePref, (GetMerge() + 1) % m_dataSo.levelTargetMerge);
        }

        private void AddPoint(int point)
        {
            PlayerPrefs.SetInt(PointPref, point + GetPoint());
        }
        
        private void AddLevel()
        {
            PlayerPrefs.SetInt(LevelPref, GetLevel() + 1);
            SetLevelText();
        }

        private int GetMerge()
        {
            return PlayerPrefs.GetInt(MergePref, 0);
        }

        private int GetPoint()
        {
           return PlayerPrefs.GetInt(PointPref, 0);;
        }

        private int GetLevel()
        {
            return PlayerPrefs.GetInt(LevelPref, 0);
        }
    }
}