using Assets.Scripts.Events;
using Assets.Scripts.Weapon;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using EventType = Assets.Scripts.Events.EventType;

namespace Assets.Scripts.Player
{
    public class PlayerController : IObserver
    {
        public Action<Vector3, WeaponType> OnWeaponShot;
        public Action OnDie;

        public Transform WeaponTransform => _weaponTransform;
        public Transform PlayerTransform => _playerTransform;

        public float Velocity => _speed;
        public float Angle => _playerTransform.eulerAngles.z;

        private PlayerInput _playerInput;        

        private GameObject _playerObject;
        private Transform _playerTransform;
        private Player _player;
        private Transform _weaponTransform;        

        private float _maxSpeed;
        private float _rotationSpeed;
        private float _acceleration;
        private float _deceleration;

        private float _worldHeightHalf;
        private float _worldWidthHalf;

        private Vector2 _currentMovement = Vector2.zero;
        private float _speed = 0f;
        private float _playerHeightHalf;
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

            _playerHeightHalf = _player.SpriteRenderer.bounds.extents.y;

            _playerInput = playerInput;
            _playerInput.Gameplay.Move.performed += OnMovementInput;
            _playerInput.Gameplay.Move.canceled += OnMovementInput;
            _playerInput.Gameplay.Shoot_MachineGun.performed += OnMachineGunShoot;
            _playerInput.Gameplay.Shoot_Laser.performed += OnLaserShoot;

            _worldHeightHalf = worldHeight / 2;
            _worldWidthHalf = worldWidth / 2;

            _player.OnPlayerCrossObject += OnPlayerCrossObject;
        }

        public void SetPlayerPosition(Vector3 newPosition, Vector3 newRotation)
        {
            _playerTransform.position = newPosition;
            _playerTransform.eulerAngles = newRotation;
        }

        public Vector2 GetPlayerPositionWithOffset()
        {
            return new Vector3(_playerTransform.position.x + _worldWidthHalf, _playerTransform.position.y + _worldHeightHalf, 0);
        }

        public void Update(INotifier notifier, EventType eventType)
        {
            if (eventType == EventType.Update)
            {
                CheckLevelBounds();

                CalculateSpeed();

                Rotate();

                _playerTransform.position += _playerTransform.up * _speed * Time.deltaTime;
            }
        }        

        private void CheckLevelBounds()
        {
            float currentX = _playerTransform.position.x;
            float currentY = _playerTransform.position.y;

            if (Mathf.Abs(currentX) > (Mathf.Abs(_worldWidthHalf) + _playerHeightHalf))
            {
                float newY = (Mathf.Abs(currentY) > (Mathf.Abs(_worldHeightHalf) - _delta))
                    ? -currentY
                    : currentY;

                _playerTransform.position = new Vector3(-currentX, newY, 0);
            }
            else if (Mathf.Abs(currentY) > (Mathf.Abs(_worldHeightHalf) + _playerHeightHalf))
            {
                float newX = (Mathf.Abs(currentX) > (Mathf.Abs(_worldWidthHalf) - _delta))
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

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovement = context.ReadValue<Vector2>();
        }

        private void OnMachineGunShoot(InputAction.CallbackContext context)
        {
            Shoot(_playerTransform.up, WeaponType.MachineGun);
        }

        private void OnLaserShoot(InputAction.CallbackContext context)
        {
            Shoot(_playerTransform.up, WeaponType.Laser);
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
