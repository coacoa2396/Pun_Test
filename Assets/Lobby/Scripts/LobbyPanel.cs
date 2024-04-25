using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomEntry> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomEntry>();
    }

    private void OnDisable()
    {
        for (int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }
        roomDictionary.Clear();
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Update room info
        foreach (RoomInfo roomInfo in roomList)
        {
            // 1. ���� ������� ���
            if (roomInfo.RemovedFromList || roomInfo.IsOpen == false || roomInfo.IsVisible == false)
            {
                RoomEntry roomEntry = roomDictionary[roomInfo.Name];
                roomDictionary.Remove(roomInfo.Name);
                Destroy(roomEntry.gameObject);
            }
            // 2. ���� ���빰�� �ٲ�� ���
            else if (roomDictionary.ContainsKey(roomInfo.Name))
            {
                RoomEntry roomEntry = roomDictionary[roomInfo.Name];
                roomEntry.SetRoomInfo(roomInfo);
            }
            // 3. ���� ����� ���
            else
            {
                RoomEntry roomEntry = Instantiate(roomEntryPrefab, roomContent);
                roomEntry.SetRoomInfo(roomInfo);
                roomDictionary.Add(roomInfo.Name, roomEntry);
            }
        }
    }
}
