using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] Button startButton;

    List<PlayerEntry> playerList;

    private void Awake()
    {
        playerList = new List<PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerContent);
            playerEntry.SetPlayer(player);
            playerList.Add(playerEntry);
        }

        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);     // �����̸� true, �ƴϸ� false

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);
        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;            // ������ �� �̵��� ���󰣴�
    }

    private void OnDisable()
    {
        for (int i = 0; i < playerContent.childCount; i++)
        {
            Destroy(playerContent.GetChild(i).gameObject);
        }
        playerList.Clear();
        PhotonNetwork.AutomaticallySyncScene = false;           // �濡�� �������Ƿ� ����ũ�� ���ش�
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerContent);
        playerEntry.SetPlayer(newPlayer);
        playerList.Add(playerEntry);
        AllPlayerReadyCheck();
    }

    public void PlayerLeftRoom(Player otherPlayer)
    {
        PlayerEntry playerEntry = null;
        foreach (PlayerEntry entry in playerList)
        {
            if (entry.Player.ActorNumber == otherPlayer.ActorNumber)
            {
                playerEntry = entry;
            }
        }

        Destroy(playerEntry.gameObject);
        playerList.Remove(playerEntry);
        AllPlayerReadyCheck();
    }

    public void MasterClientChanged(Player newMasterClient)
    {
        startButton.gameObject.SetActive(newMasterClient.IsLocal);
        AllPlayerReadyCheck();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        PlayerEntry playerEntry = null;
        foreach (PlayerEntry entry in playerList)
        {
            if (entry.Player.ActorNumber == targetPlayer.ActorNumber)
            {
                playerEntry = entry;
            }
        }
        AllPlayerReadyCheck();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
            {
                readyCount++;
            }
        }

        startButton.interactable = readyCount == PhotonNetwork.PlayerList.Length;
        // �Ʒ��� ����

        //if (readyCount == PhotonNetwork.PlayerList.Length)
        //{
        //    startButton.interactable = true;
        //}
        //else
        //{
        //    startButton.interactable = false;
        //}
    }
}
