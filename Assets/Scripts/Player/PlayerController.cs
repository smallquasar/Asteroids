using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController
    {
        private GameObject _playerObject;
        private Transform _playerTransform;
        private Player _player;

        private float _speed = 0f;
        private float _maxSpeed = 4f;
        private float _rotationSpeed = 40f;
        private float _acceleration = 2f;
        private float _deceleration = 3f;

        private float _worldHeight;
        private float _worldWidth;

        private float _halfPlayer = 0.25f;
        private float _delta = 0.5f;

        public PlayerController(GameObject playerObject, float worldHeight, float worldWidth)
        {
            _playerObject = playerObject;
            _playerTransform = _playerObject.transform;
            _player = playerObject.GetComponent<Player>();

            _maxSpeed = _player.MaxSpeed;
            _rotationSpeed = _player.RotationSpeed;
            _acceleration = _player.Acceleration;
            _deceleration = _player.Deceleration;

            _worldHeight = worldHeight;
            _worldWidth = worldWidth;

            _player.OnPlayerUpdate += Update;
        }

        private void Update()
        {
            CheckLevelBounds();

            if (Input.GetKey(KeyCode.W) && _speed < _maxSpeed)
            {
                _speed += _acceleration * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S) && _speed > -_maxSpeed)
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

            if (Input.GetKey(KeyCode.A))
            {
                _playerTransform.Rotate(_playerTransform.forward * _rotationSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _playerTransform.Rotate(-_playerTransform.forward * _rotationSpeed * Time.deltaTime);
            }

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
    }
}
