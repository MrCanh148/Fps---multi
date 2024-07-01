using UnityEngine;
using Photon.Pun;

public class RoomManage : MonoBehaviourPunCallbacks
{
    public static RoomManage instance;

    public GameObject Player;
    [SerializeField] private Transform PlaceSpawn;
    [SerializeField] private GameObject roomCam;

    private GameObject _player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Connecting .......");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomCam.SetActive(false);
        ReSpawnPlayer();

    }

    public void ReSpawnPlayer()
    {
        _player = PhotonNetwork.Instantiate(Player.name, PlaceSpawn.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
    }
}
