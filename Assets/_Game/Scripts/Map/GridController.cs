using System;
using System.Threading.Tasks;
using _Game.Scripts.InputSystem;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public class GridController : MonoBehaviour
    {
        public int x => _placement.x;
        public int y => _placement.y;

        private BubbleController _bubble;
        private (int x, int y) _placement;


        public void Init((int x, int y) placement, BubbleController bubbleController)
        {
            _placement = placement;
            AddNewBubble(bubbleController);
        }

        private void OnMouseDown()
        {
            InputManager.OnGridSelected.Invoke(this);
        }

        private void OnMouseEnter()
        {
#if UNITY_EDITOR
            if (!Input.GetMouseButton(0))
            {
                return;
            }
#endif
            if (Input.GetMouseButton(0))
            {
                InputManager.OnGridSelected.Invoke(this);
            }
        }

        public void OnSelect()
        {
            _bubble.OnSelect(true);
        }

        public void ResetSelection()
        {
            _bubble.OnSelect(false);
        }

        public int GetLevel()
        {
            return _bubble.GetLevel();
        }

        public async Task OnMatch(GridController lastGrid)
        {
            await _bubble.OnMatch(lastGrid.transform);
            
            _bubble.SetDisable();
            _bubble = null;
        }

        public void UpdateLevel(int level)
        {
            _bubble.SetBubble(level);
            _bubble.OnMatchedEffect();
        }

        public bool IsEmpty()
        {
            return ReferenceEquals(_bubble, null);
        }

        public void AddBubble(BubbleController bubble)
        {
            _bubble = bubble;
        }

        public async Task MoveBubble()
        {
            await _bubble.OnAsyncGrid(transform.position.y);
        }

        public void RemoveBubble(out BubbleController bubble)
        {
            bubble = _bubble;
            _bubble = null; 
        }

        public void AddNewBubble(BubbleController bubble)
        {
            _bubble = bubble;
            _bubble.OnGrid(transform.position);
        }
    }
}