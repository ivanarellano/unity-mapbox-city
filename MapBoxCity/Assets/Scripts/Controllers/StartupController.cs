using UnityEngine;

public class StartupController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    private GameObject _playerInstance;

    private void Awake()
    {
        Messenger.AddListener(MapEvent.MAP_INITIALIZED, OnMapInitialized);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(MapEvent.MAP_INITIALIZED, OnMapInitialized);
    }

    private void OnMapInitialized()
    {
        if (_playerInstance == null)
        {
            _playerInstance = Instantiate(_player);
            _playerInstance.transform.position = new Vector3(-80, 1, -80); // TODO: Serialize? LatLng->World?
        }
    }
}
