using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    }

    private void OnDisable()
    {
        for (int i = 0; i < playerContent.childCount; i++)
        {
            Destroy(playerContent.GetChild(i).gameObject);
        }
        playerList.Clear();
    }

    public void StartGame()
    {

    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerContent);
        playerEntry.SetPlayer(newPlayer);
        playerList.Add(playerEntry);
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
    }

    public void MasterClientChanged(Player newMasterClient)
    {
        startButton.gameObject.SetActive(newMasterClient.IsLocal);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
