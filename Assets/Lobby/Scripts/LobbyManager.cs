using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MainPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;

    ClientState state;

    public override void OnConnected()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.ApplicationQuit)
            return;

        SetActivePanel(Panel.Login);
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Lobby);
    }

    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Create room failed with error : {message}({returnCode})");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create room success");
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join random room failed : {message}({returnCode})");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobbyPanel.UpdateRoomList(roomList);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomPanel.PlayerEnterRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomPanel.PlayerLeftRoom(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomPanel.MasterClientChanged(newMasterClient);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        roomPanel.PlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    private void Start()
    {
        SetActivePanel(Panel.Login);
    }

    private void Update()
    {
        ClientState curState = PhotonNetwork.NetworkClientState;
        if (state == curState)
            return;

        state = curState;
        Debug.Log(state);
    }

    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject.SetActive(panel == Panel.Login);
        menuPanel.gameObject.SetActive(panel == Panel.Menu);
        roomPanel.gameObject.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
    }
}
