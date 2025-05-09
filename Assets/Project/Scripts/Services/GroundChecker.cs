using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Collider _collider;

    public bool IsGrounded => Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, _layerMask).Length != 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, transform.localScale / 2);
    }
}