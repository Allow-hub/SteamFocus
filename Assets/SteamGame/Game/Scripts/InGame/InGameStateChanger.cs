using UnityEngine;

namespace TechC
{
    public class InGameStateChanger : MonoBehaviour
    {
        [SerializeField] private GameManager.GameState changeState;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                if (GameManager.I == null) return;
                GameManager.I.SetState(changeState);
            }
        }
    }
}
