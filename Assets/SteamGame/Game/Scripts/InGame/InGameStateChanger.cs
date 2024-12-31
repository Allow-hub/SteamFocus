using UnityEngine;

namespace TechC
{
    public class InGameStateChanger : MonoBehaviour
    {
        [SerializeField] private GameManager.GameState changeState;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (GameManager.I == null) return;
                GameManager.I.SetState(changeState);
            }
        }
    }
}
