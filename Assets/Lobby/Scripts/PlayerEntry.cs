using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] Button playerReadyButton;

    Player player;
    public Player Player { get { return player; } }

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerReady.text = player.GetReady() ? "Ready" : "";
        playerName.text = player.NickName;

        // ���� ���� Ȯ���ϴ� �ΰ��� ���
        // player.IsLocal;
        // PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber;

        playerReadyButton.gameObject.SetActive(player.IsLocal);
    }

    public void Ready()     // Ȯ��޼���� ó���ع���
    {
        bool ready = player.GetReady();
        player.SetReady(!ready);
    }

    public void ChangeCustomProperty(PhotonHashtable property)
    {
        bool ready = player.GetReady();
        playerReady.text = ready ? "Ready" : "";
    }
}
