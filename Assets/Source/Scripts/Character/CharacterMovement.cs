using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 1000f;
    [SerializeField] private float _rotationSpeed = 1000f;
    [SerializeField] private GameObject _body;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _sensetivity;

    private Coroutine _transformUpdating;
    private MouseInput _mouseInput;
    private KeyboardInput _keyboardInput;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private float _eulerX;
    private State _currentState = State.Stay;

    public event Action Staying;
    public event Action Running;

    public void Init(MouseInput mouseInput, KeyboardInput keyboardInput)
    {
        _mouseInput = mouseInput;
        _keyboardInput = keyboardInput;
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();

        _transformUpdating = StartCoroutine(Updating());
    }

    public void StopUpdating()
    {
        if (_transformUpdating != null)
        {
            StopCoroutine(_transformUpdating);
        }
    }

    private IEnumerator Updating()
    {
        while (true)
        {
            RotateHorizontal(_mouseInput.DeltaX);
            RotateVertical(_mouseInput.DeltaY);
            Move(_keyboardInput.Direction);
            yield return null;
        }
    }

    private void Move(Vector2 direction)
    {
        float speed = _maxSpeed;
        Vector3 worldDirection = RotateTo90degres(direction);
        worldDirection = transform.rotation * worldDirection;
        _rigidbody.velocity = speed * worldDirection;

        if (direction == Vector2.zero)
        {
            ChangeState(State.Stay, Staying);
        }
        else
        {
            ChangeState(State.Run, Running);
        }
    }

    private void ChangeState(State state, Action action)
    {
        if (_currentState != state)
        {
            action.Invoke();
            _currentState = state;
        }
    }

    private void RotateHorizontal(float value)
    {
        transform.localEulerAngles += new Vector3(0f, value * _sensetivity, 0f);
    }

    private void RotateVertical(float value)
    {
        _eulerX += value * _sensetivity;
        _eulerX = Mathf.Clamp(_eulerX, _minAngle, _maxAngle);
        _body.transform.localEulerAngles = Vector3.left * _eulerX;
    }

    private Vector3 RotateTo90degres(Vector3 vector)
    {
        return new Vector3(vector.x, vector.z, vector.y);
    }

    private enum State
    {
        Stay,
        Run
    }
}