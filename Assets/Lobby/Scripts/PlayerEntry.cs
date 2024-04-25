using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        playerName.text = player.NickName;

        // ���� ���� Ȯ���ϴ� �ΰ��� ���
        // player.IsLocal;
        // PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber;

        playerReadyButton.gameObject.SetActive(player.IsLocal);
    }

    public void Ready()
    {

    }
}
