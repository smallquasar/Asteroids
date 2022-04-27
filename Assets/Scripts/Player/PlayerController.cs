using Assets.Scripts.Weapon;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerController
    {
        public Action<Vector3, WeaponType> OnWeaponShot;
        public Action OnDie;

        public Transform WeaponTransform => _weaponTransform;
        public Transform PlayerTransform => _playerTransform;

        public Vector2 Coordinates => _playerTransform.position;
        public float Velocity => _speed;
        public float Angle => _playerTransform.eulerAngles.z;

        private PlayerInput _playerInput;
        private Vector2 _currentMovement = Vector2.zero;

        private GameObject _playerObject;
        private Transform _playerTransform;
        private Player _player;
        private Transform _weaponTransform;

        private float _speed = 0f;
        private float _maxSpeed = 4f;
        private float _rotationSpeed = 40f;
        private float _acceleration = 2f;
        private float _deceleration = 3f;

        private float _worldHeight;
        private float _worldWidth;

        private float _halfPlayer = 0.25f;
        private float _delta = 0.5f;

        public PlayerController(GameObject playerObject, PlayerInput playerInput, float worldHeight, float worldWidth)
        {
            _playerObject = playerObject;
            _playerTransform = _playerObject.transform;
            _player = playerObject.GetComponent<Player>();

            _weaponTransform = _player.WeaponTransform;

            _maxSpeed = _player.MaxSpeed;
            _rotationSpeed = _player.RotationSpeed;
            _acceleration = _player.Acceleration;
            _deceleration = _player.Deceleration;

            _playerInput = playerInput;
            _playerInput.Gameplay.Move.performed += OnMovementInput;
            _playerInput.Gameplay.Move.canceled += OnMovementInput;
            _playerInput.Gameplay.Shoot_MachineGun.performed += OnMachineGunShoot;
            _playerInput.Gameplay.Shoot_Laser.performed += OnLaserShoot;

            _worldHeight = worldHeight;
            _worldWidth = worldWidth;

            _player.OnPlayerUpdate += Update;
            _player.OnPlayerCrossObject += OnPlayerCrossObject;
        }

        public void SetPlayerPosition(Vector3 newPosition, Vector3 newRotation)
        {
            _playerTransform.position = newPosition;
            _playerTransform.eulerAngles = newRotation;
        }

        public void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovement = context.ReadValue<Vector2>();
        }

        public void OnMachineGunShoot(InputAction.CallbackContext context)
        {
            Shoot(_playerTransform.up, WeaponType.MachineGun);
        }

        public void OnLaserShoot(InputAction.CallbackContext context)
        {
            Shoot(_playerTransform.up, WeaponType.Laser);
        }

        private void Update()
        {
            CheckLevelBounds();

            CalculateSpeed();

            Rotate();

            _playerTransform.position += _playerTransform.up * _speed * Time.deltaTime;
        }        

        private void CheckLevelBounds()
        {
            float currentX = _playerTransform.position.x;
            float currentY = _playerTransform.position.y;

            if (Mathf.Abs(currentX) > (Mathf.Abs(_worldWidth / 2) + _halfPlayer))
            {
                float newY = (Mathf.Abs(currentY) > (Mathf.Abs(_worldHeight / 2) - _delta))
                    ? -currentY
                    : currentY;

                _playerTransform.position = new Vector3(-currentX, newY, 0);
            }
            else if (Mathf.Abs(currentY) > (Mathf.Abs(_worldHeight / 2) + _halfPlayer))
            {
                float newX = (Mathf.Abs(currentX) > (Mathf.Abs(_worldWidth / 2) - _delta))
                    ? -currentX
                    : currentX;

                _playerTransform.position = new Vector3(newX, -currentY, 0);
            }
        }

        private void CalculateSpeed()
        {
            if (_currentMovement.y > 0 && _speed < _maxSpeed)
            {
                _speed += _acceleration * Time.deltaTime;
            }
            else if (_currentMovement.y < 0 && _speed > -_maxSpeed)
            {
                _speed -= _acceleration * Time.deltaTime;
            }
            else
            {
                if (_speed > _deceleration * Time.deltaTime)
                {
                    _speed -= _deceleration * Time.deltaTime;
                }
                else if (_speed < -_deceleration * Time.deltaTime)
                {
                    _speed += _deceleration * Time.deltaTime;
                }
                else
                {
                    _speed = 0;
                }

            }
        }

        private void Rotate()
        {
            if (_currentMovement.x < 0)
            {
                _playerTransform.Rotate(_playerTransform.forward * _rotationSpeed * Time.deltaTime);
            }
            if (_currentMovement.x > 0)
            {
                _playerTransform.Rotate(-_playerTransform.forward * _rotationSpeed * Time.deltaTime);
            }
        }

        private void Shoot(Vector3 playerDirection, WeaponType weaponType)
        {
            OnWeaponShot?.Invoke(playerDirection, weaponType);
        }

        private void OnPlayerCrossObject(Collider2D collisionObject)
        {
            if (collisionObject.CompareTag("Enemy"))
            {
                OnDie?.Invoke();
            }
        }
    }
}
