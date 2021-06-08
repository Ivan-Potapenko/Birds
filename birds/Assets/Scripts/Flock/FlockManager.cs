using UnityEngine;
using Sirenix.OdinInspector;


namespace GamePlay
{
    public class FlockManager : MonoBehaviour
    {
        [ValidateInput(nameof(ValidateFlock), defaultMessage: "There is no Flock component on the object")]
        private Flock _flock;

        private bool ValidateFlock()
        {
            return GetComponent<Flock>();
        }

        

        [SerializeField]
        private int _startNumberOfBirds;

        void Start()
        {
            _flock = GetComponent<Flock>();

            for (int i = 0; i < _startNumberOfBirds; i++)
            {
                _flock.addBird();
            }

        }

    }
}