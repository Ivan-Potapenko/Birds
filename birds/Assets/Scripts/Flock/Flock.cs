using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace GamePlay
{
    public class Flock : MonoBehaviour
    {

        [SerializeField]
        private List<Bird> _birds;

        [SerializeField]
        private float _flockSpeed;

        [SerializeField]
        private Bird _bird;

        [SerializeField]
        private BirdType _birdType;

        public BirdType BirdType { get => _birdType; }

        [SerializeField]
        private GameObject FlockComposition;

        private bool _flockStopCoroutineIsActive;

        private bool _stopFlockCoroutineIsReadyForStart;

        [SerializeField]
        private Vector3 _flockDirectionVector;

        public Vector3 FlockDirectionVector { get => _flockDirectionVector;}


        private void Awake()
        {
            FlockComposition = transform.parent.gameObject;
            _flockStopCoroutineIsActive = false;
            _stopFlockCoroutineIsReadyForStart = false;
            _flockDirectionVector = Vector3.zero;
        }

        private void OnDisable()
        {
            StopCoroutine(nameof(FlockStopCoroutine));
        }

        public void addBird()
        {
            if (_birds == null)
            {
                _birds = new List<Bird>();
            }
            Bird bird = Instantiate(_bird, FlockComposition.transform);
            bird.Flock = this;
            bird.transform.position = gameObject.transform.position;
            _birds.Add(bird);

        }

        public void DestroyBird(Bird bird)
        {
            if (_birds.Contains(bird))
            {
                _birds.Remove(bird);
                Destroy(bird.gameObject);
            }
        }

        public int GetBirdsCount()
        {
            if (_birds == null)
            {
                return 0;
            }
            return _birds.Count;
        }

        public void MoveFLockAndBreakStopCoroutine(Vector2 directionVector)
        {
            if (_flockStopCoroutineIsActive)
            {
                StopCoroutine(nameof(FlockStopCoroutine));
            }
            _stopFlockCoroutineIsReadyForStart = true;
            MoveFlock(directionVector, _flockSpeed);
        }

        private void MoveFlock(Vector2 directionVector, float flockSpeed)
        {
            transform.Translate(directionVector * flockSpeed * Time.deltaTime, Space.World);
            _flockDirectionVector = directionVector;
        }


        public void StopFlock(Vector2 directionVector)
        {
            if (!_flockStopCoroutineIsActive && _stopFlockCoroutineIsReadyForStart)
            {
                StartCoroutine(FlockStopCoroutine(directionVector));
            }
        }


        private IEnumerator FlockStopCoroutine(Vector3 directionVector)
        {
            _flockStopCoroutineIsActive = true;
            _stopFlockCoroutineIsReadyForStart = false;
            var flockSpeed = _flockSpeed;

            while (flockSpeed > 0.00001f)
            {
                MoveFlock(directionVector, flockSpeed);
                flockSpeed *= 0.9f;
                yield return new WaitForFixedUpdate();
            }

            _flockDirectionVector = Vector3.zero;
            _flockStopCoroutineIsActive = false;

        }

    }
}

