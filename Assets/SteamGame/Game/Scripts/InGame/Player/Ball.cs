using System.Collections;
using UnityEngine;

namespace TechC
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float radius = 1.5f;
        [SerializeField] private bool isDrawingGizmo;
        [SerializeField] private float velocityThreshold;
        [SerializeField] private Transform initPos;
        [SerializeField] private bool isDebug = true;
        [SerializeField] private GameObject breakEffect;
        public float Radius => radius;

        private Rigidbody _rb;
        public Rigidbody Rb => _rb;

        private void Awake()
        {
            breakEffect.SetActive(false);   
            _rb = GetComponent<Rigidbody>();
            if (isDebug) return;
            transform.position=initPos.position;
        }
        private void Start()
        {
            GameManager.I.ChangeNetWarkSettingState();
        }

        public   void BreakEffect()
        {
            StartCoroutine(Effect());
        }

        private IEnumerator Effect()
        {
            breakEffect.SetActive(true);    
            yield return new WaitForSeconds(2f);

            breakEffect.SetActive(false);
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
