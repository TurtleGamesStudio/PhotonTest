using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Health))]
public class Character : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _body;
    [SerializeField] private Camera _camera;
    [SerializeField] private Weapon _weapon;

    private CharacterMovement _characterMovement;
    private MouseInput _mouseInput;
    private Health _health;
    private Collider _collider;

    private PhotonView _photonView;
    private Hashtable _hashTable;
    private bool _isInitialized = false;

    public event Action Dying;

    private void Awake()
    {
        if (_isInitialized == false)
            CommonInit();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (_mouseInput != null)
        {
            _mouseInput.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
            _mouseInput.LeftMouseButtonReleased -= OnLeftMouseButtonReleased;
        }

        _health.EqualToZero -= Die;
    }

    private void CommonInit()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _health = GetComponent<Health>();
        _collider = GetComponent<Collider>();
        _photonView = GetComponent<PhotonView>();

        _health.Init();
        _isInitialized = true;

        if (_photonView.IsMine == false)
        {
            Destroy(GetComponent<Rigidbody>());
        }
    }

    public void Init(MouseInput mouseInput, KeyboardInput keyboardInput, ParticleSystem hitEffect)
    {
        if (_isInitialized == false)
            CommonInit();

        _hashTable = new Hashtable();
        _hashTable.Add(HashNames.Collider, true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(_hashTable);

        _weapon.Init(hitEffect);
        _characterMovement.Init(mouseInput, keyboardInput);
        _body.SetActive(false);
        _camera.gameObject.SetActive(true);
        _camera.AddComponent<AudioListener>();

        _mouseInput = mouseInput;
        _mouseInput.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
        _mouseInput.LeftMouseButtonReleased += OnLeftMouseButtonReleased;
        _health.EqualToZero += Die;
    }

    private void OnLeftMouseButtonPressed()
    {
        _weapon.StartShooting();
    }

    private void OnLeftMouseButtonReleased()
    {
        _weapon.StopShooting();
    }

    private void Die()
    {
        _characterMovement.StopUpdating();
        _collider.enabled = false;
        _hashTable[HashNames.Collider] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_hashTable);
        _mouseInput.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
        _mouseInput.LeftMouseButtonReleased -= OnLeftMouseButtonReleased;
        _weapon.StopShooting();
        Dying?.Invoke();

        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (_photonView.IsMine == false && targetPlayer == _photonView.Owner)
        {
            _collider.enabled = (bool)changedProps[HashNames.Collider];
        }
    }

    public void TakeDamage(float value)
    {
        _photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.All, value);
    }

    [PunRPC]
    public void RPC_TakeDamage(float value)
    {
        _health.Decrease(value);
    }

    private class HashNames
    {
        public const string Collider = "Collider";
    }
}