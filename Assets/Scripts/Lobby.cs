using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
	[SerializeField] private Menu _menu;

	private void Awake()
	{
		_menu.SetScreen(Menu.Screens.Connect);
	}
	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings();
		_menu.SetScreen(Menu.Screens.Wait);
	}
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		_menu.SetScreen(Menu.Screens.Rooms);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log($"Disconnected. Cause: {cause}"); ;
	}
}
