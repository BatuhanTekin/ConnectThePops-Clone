using System.Threading.Tasks;
using _Game.Scripts.Bubble;
using _game.Utility.ObjectPooling;
using UnityEngine;

public class BubbleController : MonoBehaviour, IPoolingObject
{
    [SerializeField] private BubbleDataSo m_dataSo;
    
    [SerializeField] private BubbleVisualEffect m_bubbleVisualEffect;
    [SerializeField] private BubbleVisualController m_bubbleVisualController;
    
    private int _level;
    private (Sprite sprite, Color color) _visualData;
    private bool _enable;

    private void Awake()
    {
        m_bubbleVisualEffect.Init(transform);
    }

    public void SetBubble(int level)
    {
        _level = level;
        _visualData = m_dataSo.GetData(_level);
        m_bubbleVisualController.SetVisual(_visualData, _level);
    }

    public int GetLevel()
    {
        return _level;
    }

    

    #region Effects

    public void OnSelect(bool isSelected)
    {
        m_bubbleVisualEffect.SelectEffect(isSelected);
    }

    public void OnGrid(Vector3 position)
    {
        transform.position = position;
        m_bubbleVisualEffect.SpawnEffect();
    }
    public async Task OnAsyncGrid(float y)
    {
        await m_bubbleVisualEffect.PlayDropEffect(y);
    }
    
    
    public void OnMatchedEffect()
    {
        m_bubbleVisualEffect.OnMatchedEffect();
    }
    
    public async Task OnMatch(Transform targetPosition)
    {
        await m_bubbleVisualEffect.MatchEffect(targetPosition);
    }

    #endregion


    #region Pool

    public void OnSpawn()
    {
    }

    public void SetEnable()
    {
        gameObject.SetActive(true);
        _enable = true;
    }

    public void SetDisable()
    {
        gameObject.SetActive(false);
        _enable = false;
    }

    public bool IsDisabled()
    {
        return !_enable;
    }

    #endregion

    
}
