using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] float countDownTime;

    void Start()
    {
        PhotonNetwork.LocalPlayer.SetLoad(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {

            if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
            {
                // �ε� �Ϸ�
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.CurrentRoom.SetGameStart(true);
                    PhotonNetwork.CurrentRoom.SetGameStartTime(PhotonNetwork.Time);
                }
            }
            else
            {
                // �ٸ� �÷��̾� ��ٸ���
                infoText.text = $" Load : {PlayerLoadCount()} / {PhotonNetwork.PlayerList.Length}";
            }
        }
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(CustomProperty.GAMESTART))
        {
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator StartTimer()
    {
        double loadTime = PhotonNetwork.CurrentRoom.GetGameStartTime();
        while (PhotonNetwork.Time - loadTime < countDownTime)
        {
            int remainTime = (int)(countDownTime - (PhotonNetwork.Time - loadTime));
            infoText.text = (remainTime + 1).ToString();
            yield return null;
        }

        infoText.text = "GameStart!";
        yield return new WaitForSeconds(3f);

        infoText.text = "";
    }

    int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad())
            {
                loadCount++;
            }
        }
        return loadCount;
    }

    public void GameStart()
    {
        // ���ӳ���
    }
}
