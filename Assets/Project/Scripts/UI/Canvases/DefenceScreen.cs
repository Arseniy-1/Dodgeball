using Project.Scripts.Entities;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class DefenceScreen : TutorialScreen
    {
        private Camera _mainCamera;

        public override void Initialize()
        {
            _mainCamera = Camera.main;
            GameStatusService.Instance.HolderChanged += OnHolderChanged;
        }

        private void OnHolderChanged(Entity entity)
        {
            if (entity is Enemy == false)
                return;

            Time.timeScale = 0.25f;
        
            gameObject.SetActive(true);
        
            if (entity == null)
            {
                SelectionCircle.gameObject.SetActive(false);
            
                return;
            }

            SelectionCircle.gameObject.SetActive(true);
        
            UpdateSelectionCirclePosition(entity);

            ApplyButton.ApplyButtonClicked += OnApplyButtonClicked;
            GameStatusService.Instance.HolderChanged += CheckHolder;
        }

        private void UpdateSelectionCirclePosition(Entity entity)
        {
            if (entity == null || _mainCamera == null) 
            {
                SelectionCircle.gameObject.SetActive(false);
                return;
            }

            Vector3 worldPosition = entity.transform.position;
        
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

        private void CheckHolder(Entity entity)
        {
            if (entity is Player)
                OnApplyButtonClicked();
        }
    
        private void OnApplyButtonClicked()
        {
            Time.timeScale = 1f;
            GameStatusService.Instance.HolderChanged -= OnHolderChanged;
            GameStatusService.Instance.HolderChanged -= CheckHolder;
            ApplyButton.ApplyButtonClicked -= OnApplyButtonClicked;

            gameObject.SetActive(false);
        }
    }
}