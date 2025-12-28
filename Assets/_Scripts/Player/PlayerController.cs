using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Units.Players {
    public class PlayerController : MonoBehaviour {
        [Header("Movement Settings")]
        [InfoBox("R/F --> +/-")]
        [SerializeField]
        private float _forwardStrength = 250f;

        [InfoBox("Z/X --> +/-")]
        [SerializeField]
        private float _torqueStrength = 1f;

        [SerializeField]
        [InfoBox("3/1 --> +/-")]
        private float _rollSpeedPerSecond = 75f;

        private Keyboard _keyboard;

        private Rigidbody _rigidbody;

        [Header("ReadOnly")]
        private Vector3 _rollDirection;

        public void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _keyboard = Keyboard.current;
        }

        public void Update() {
            UpdateMovementSettings();
        }

        public void FixedUpdate() {
            PhysicsMovement();
        }

        private void PhysicsMovement() {
            if (_keyboard.wKey.isPressed) {
                _rigidbody.AddTorque(1 * _torqueStrength * transform.right);
            }

            if (_keyboard.sKey.isPressed) {
                _rigidbody.AddTorque(-1 * _torqueStrength * transform.right);
            }

            if (_keyboard.dKey.isPressed) {
                _rigidbody.AddTorque(1 * _torqueStrength * transform.up);
            }

            if (_keyboard.aKey.isPressed) {
                _rigidbody.AddTorque(-1 * _torqueStrength * transform.up);
            }

            _rollDirection.z = 0;

            if (_keyboard.eKey.isPressed) {
                _rollDirection.z -= 1;
            }

            if (_keyboard.qKey.isPressed) {
                _rollDirection.z += 1;
            }

            if (_rollDirection.z != 0) {
                transform.Rotate(_rollDirection * (_rollSpeedPerSecond * Time.deltaTime), Space.Self);
            }

            _rigidbody.AddForce(transform.forward * _forwardStrength);
        }

        private void UpdateMovementSettings() {
            float deltaTime = Time.deltaTime;
            if (_keyboard.rKey.isPressed) {
                _forwardStrength += 100f * deltaTime;
            }

            if (_keyboard.fKey.isPressed) {
                _forwardStrength -= 100f * deltaTime;
            }

            if (_keyboard.zKey.isPressed) {
                _torqueStrength += 1f * deltaTime;
            }

            if (_keyboard.xKey.isPressed) {
                _torqueStrength -= 1f * deltaTime;
            }

            if (_keyboard.digit3Key.isPressed) {
                _rollSpeedPerSecond += 10f * deltaTime;
            }

            if (_keyboard.digit1Key.isPressed) {
                _rollSpeedPerSecond -= 10f * deltaTime;
            }
        }

        private void KineticMovement() {
            Keyboard keyboard = Keyboard.current;
            Vector3 targetDirection = transform.rotation.eulerAngles;
            targetDirection.z = 0;
            if (keyboard.wKey.isPressed) {
                targetDirection.x += 10;
            }

            if (keyboard.sKey.isPressed) {
                targetDirection.x -= 10;
            }

            if (keyboard.aKey.isPressed) {
                targetDirection.y -= 10;
            }

            if (keyboard.dKey.isPressed) {
                targetDirection.y += 10;
            }


            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetDirection),
                _rollSpeedPerSecond * Time.deltaTime);
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            eulerRotation.z = 0;
            transform.rotation = Quaternion.Euler(eulerRotation);
            transform.Translate(Vector3.forward * (Time.deltaTime * _forwardStrength));
        }
    }
}