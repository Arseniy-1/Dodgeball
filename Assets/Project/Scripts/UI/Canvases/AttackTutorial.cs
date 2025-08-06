using Project.Scripts.Entities;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class AttackTutorial : TutorialScreen
    {
        private Camera _mainCamera;

        public override void Initialize()
        {
            _mainCamera = Camera.main;
            GameStatusService.Instance.HolderChanged += OnHolderChanged;
        }

        private void OnHolderChanged(Entity entity)
        {
            if (entity is Player == false)
                return;
        
            gameObject.SetActive(true);
        
            if (entity == null)
            {
                SelectionCircle.gameObject.SetActive(false);
            
                return;
            }

            SelectionCircle.gameObject.SetActive(true);
        
            UpdateSelectionCirclePosition(entity);

            ApplyButton.ButtonClicked += OnApplyButtonClicked;
        }

        private void UpdateSelectionCirclePosition(Entity entity)
        {
            if (entity == null || _mainCamera == null) 
            {
                SelectionCircle.gameObject.SetActive(false);
                
                return;
            }

            Vector3 worldPosition = entity.transform.position + Vector3.up * 1f;
        
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);

            if (screenPosition.z > 0)
            {
                SelectionCircle.rectTransform.position = screenPosition;
                SelectionCircle.gameObject.SetActive(true);
            }
            else 
            {
                SelectionCircle.gameObject.SetActive(false);
            }
        }

        private void OnApplyButtonClicked()
        {
            GameStatusService.Instance.HolderChanged -= OnHolderChanged;
            ApplyButton.ButtonClicked -= OnApplyButtonClicked;
        
            gameObject.SetActive(false);
        }
    }
}