using DG.Tweening;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts
{
     public class TweenAnimatorExample : MonoBehaviour
     {
          [SerializeField] private Transform _transform;
          [SerializeField] private PlayerInputController _playerInputController;
     
          private void OnEnable()
          {
               _playerInputController.ActionButtonStarted += Do;
          }

          private void Do()
          {
               _transform.DOMoveX(10, 1.5f).From();
          }
     }
}