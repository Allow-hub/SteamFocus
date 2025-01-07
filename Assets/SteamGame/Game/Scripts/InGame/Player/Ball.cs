using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float radius = 1.5f;
        [SerializeField] private bool isDrawingGizmo;
        [SerializeField] private float velocityThreshold;
        public float Radius => radius;

        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_rb == null) return;
            //if (_rb.velocity.magnitude <= velocityThreshold)return;
            //_rb.velocity=Vector3.zero;
            //for (int i = 0;i<GameManager.I.GetActivePlayerCount();i++)
            //{
            //    var player = GameManager.I.GetPlayer(i);
            //    var playerRb = player.GetComponent<Rigidbody>();
            //    if (playerRb == null) return;
            //    player.transform.position =transform.position;
            //    playerRb.velocity=Vector3.zero;
                
            //}
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!isDrawingGizmo) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
