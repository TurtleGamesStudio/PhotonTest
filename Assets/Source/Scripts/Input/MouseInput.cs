using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class MouseInput : MonoBehaviour
{
    private PhotonView _photonView;

    public event Action LeftMouseButtonPressed;
    public event Action LeftMouseButtonReleased;

    public float DeltaX { get; private set; }
    public float DeltaY { get; private set; }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_photonView.IsMine == false)
            return;

        DeltaX = Input.GetAxis("Mouse X");
        DeltaY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPressed?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            LeftMouseButtonReleased?.Invoke();
        }
    }
}
