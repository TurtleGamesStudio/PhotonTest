using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private int Idle = Animator.StringToHash("Idle");
    private int Run = Animator.StringToHash("Run");
    private int Die = Animator.StringToHash("Die");

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Character _character;

    private Animator _animator;
    private PhotonView _photonView;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _characterMovement.Staying += OnStaying;
        _characterMovement.Running += OnRunning;
        _character.Dying += OnDying;
    }

    private void OnDisable()
    {
        _characterMovement.Staying -= OnStaying;
        _characterMovement.Running -= OnRunning;
        _character.Dying -= OnDying;
    }

    private void OnDying()
    {
        _animator.SetTrigger(Die);
        _photonView.RPC(nameof(RPC_OnDying), RpcTarget.All);
    }

    private void OnStaying()
    {
        _animator.SetTrigger(Idle);
        _photonView.RPC(nameof(RPC_OnStaying), RpcTarget.All);
    }

    private void OnRunning()
    {
        _animator.SetTrigger(Run);
        _photonView.RPC(nameof(RPC_OnRunning), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_OnDying()
    {
        _animator.SetTrigger(Die);
    }

    [PunRPC]
    private void RPC_OnStaying()
    {
        _animator.SetTrigger(Idle);
    }

    [PunRPC]
    private void RPC_OnRunning()
    {
        _animator.SetTrigger(Run);
    }
}
