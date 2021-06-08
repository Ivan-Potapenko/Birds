using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace GamePlay
{
    public class Bird : MonoBehaviour
    {
        private Flock _flock;
        public Flock Flock { set => _flock = value; }

        [ValidateInput(nameof(ValidateRigidbody), defaultMessage: "There is no Rigidbody2D component on the object")]
        private Rigidbody2D _rigidbody2D;

        private bool ValidateRigidbody()
        {
            return GetComponent<Rigidbody2D>();
        }


        [SerializeField]
        private int _hp;

        [SerializeField]
        private int _birdDmg;

        private Vector3 _displacementVector;

        [SerializeField]
        private float _birdSpeed;

        [SerializeField]
        private Color _birdColor;

        [SerializeField]
        private List<Sprite> _birdSprites;

        [SerializeField]
        private List<RuntimeAnimatorController> _birdsAnimationClips;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _birdSpeed *= Random.Range(0.8f, 1.6f);
            _displacementVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            SetBirdValues();
        }

        private void SetBirdValues()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            switch (_flock.BirdType)
            {

                case BirdType.playerBird:
                    {
                        spriteRenderer.sprite = _birdSprites[0];
                        _animator.runtimeAnimatorController = _birdsAnimationClips[0];
                        _animator.speed = Random.Range(0.8f, 1.2f);
                        gameObject.transform.localScale = gameObject.transform.localScale * 3;
                        _hp = 600;
                        _birdDmg = 300;
                        break;
                    }
                case BirdType.enemyBird:
                    {
                        spriteRenderer.sprite = _birdSprites[0];
                        _animator.runtimeAnimatorController = _birdsAnimationClips[0];
                        gameObject.transform.localScale = gameObject.transform.localScale * 2;
                        _animator.speed = Random.Range(0.8f, 1.2f);
                        _hp = 100;
                        _birdDmg = 100;
                        break;
                    }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var bird = collision.gameObject.GetComponent<Bird>();
            if ((bird != null) && (bird._flock != _flock))
            {
                bird.receiveDmgFromBird(this);
                if (bird._birdDmg > _birdDmg)
                {
                    _flock.DestroyBird(this);

                }
            }
        }

        private void receiveDmgFromBird(Bird enemyBird)
        {
            if (_hp - _birdDmg <= 0)
            {
                _flock.DestroyBird(this);

            }
            else
            {
                _hp -= _birdDmg;
            }
        }

        private void FixedUpdate()
        {
            BirdMove();
        }

        private void BirdMove()
        {
            if (_flock == null)
            {
                return;
            }
            _rigidbody2D.velocity = (_flock.transform.position - transform.position -
                 _displacementVector) * _birdSpeed;
            ChangeBirdDirection();
        }

        private void ChangeBirdDirection()
        {
            if (_flock.FlockDirectionVector != Vector3.zero)
            {
                var angleOfRotation = Vector3.Angle(Vector3.left, _flock.FlockDirectionVector);

                if (Vector3.Angle(Vector3.up, _flock.FlockDirectionVector) < 90)
                {
                    angleOfRotation *= -1;
                }


                int xRotation = 0;
                if (Vector3.Angle(Vector3.right, _flock.FlockDirectionVector) < 90)
                {
                    angleOfRotation *= -1;
                    xRotation = 180;
                }


                gameObject.transform.localEulerAngles = new Vector3(xRotation, 0, angleOfRotation);
            }
        }
    }
}

