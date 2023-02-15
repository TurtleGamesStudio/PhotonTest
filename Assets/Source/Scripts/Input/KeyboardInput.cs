using Photon.Pun;
using UnityEngine;
using System;

public class KeyboardInput : MonoBehaviour
{
    private PhotonView _photonView;

    public event Action EscapePressed;

    public Vector2 Direction { get; private set; } = Vector2.zero;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_photonView.IsMine == false)
            return;

        Direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Direction += Vector2.up;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Direction -= Vector2.up;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Direction += Vector2.right;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Direction -= Vector2.right;
        }

        Direction.Normalize();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePressed.Invoke();
        }
    }
}
