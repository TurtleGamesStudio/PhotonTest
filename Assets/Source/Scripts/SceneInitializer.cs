using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private MouseInput _mouseInputTemplate;
    [SerializeField] private KeyboardInput _keyboardInputTemplate;
    [SerializeField] private Character _character;
    [SerializeField] private SpawnPoints _spawnPointsInScene;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private GameMenu _menu;

    private MouseInput _mouseInput;
    private KeyboardInput _keyboardInput;
    private PhotonView _photonView;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _mouseInput = PhotonNetwork.Instantiate(_mouseInputTemplate.name, Vector3.zero, Quaternion.identity).GetComponent<MouseInput>();
        _keyboardInput = PhotonNetwork.Instantiate(_keyboardInputTemplate.name, Vector3.zero, Quaternion.identity).GetComponent<KeyboardInput>();
        Transform randomPoint = _spawnPointsInScene.GetNextPoint();
        Character character = PhotonNetwork.Instantiate(_character.name, randomPoint.position, Quaternion.identity).GetComponent<Character>();

        character.Init(_mouseInput, _keyboardInput, _hitEffect);
        _menu.Init(_keyboardInput);
    }
}
