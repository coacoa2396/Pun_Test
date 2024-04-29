using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;                  // �̸��� ��ϱ� using�� ����ؼ� �����ϰ� �ٲ���

public static class CustomProperty      // Ȯ��޼���
{
    static PhotonHashtable property = new PhotonHashtable();

    public const string READY = "Ready";
    public const string LOAD = "Load";

    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;           // customProperty�� Ȯ��޼���� ��������!
        if (customProperty.TryGetValue(READY, out object value))          // object �� �����ͼ�
        {
            return (bool)value;                                             // bool�� ��ȯ�ؼ� ���
        }
        else
        {
            return false;
        }
    }

    public static void SetReady(this Player player, bool value)
    {
        property.Clear();
        property[READY] = value;
        player.SetCustomProperties(property);
    }

    public static bool GetLoad(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;           // customProperty�� Ȯ��޼���� ��������!
        if (customProperty.TryGetValue(LOAD, out object value))          // object �� �����ͼ�
        {
            return (bool)value;                                             // bool�� ��ȯ�ؼ� ���
        }
        else
        {
            return false;
        }
    }

    public static void SetLoad(this Player player, bool value)
    {
        property.Clear();
        property[LOAD] = value;
        player.SetCustomProperties(property);
    }

    public const string GAMESTART = "GameStart";
    public static bool GetGameStart(this Room room)
    {
        PhotonHashtable customProperty = room.CustomProperties;           // customProperty�� Ȯ��޼���� ��������!
        if (customProperty.TryGetValue(GAMESTART, out object value))          // object �� �����ͼ�
        {
            return (bool)value;                                             // bool�� ��ȯ�ؼ� ���
        }
        else
        {
            return false;
        }
    }

    public static void SetGameStart(this Room room, bool value)
    {
        property.Clear();
        property[GAMESTART] = value;
        room.SetCustomProperties(property);
    }

    public const string GAMESTARTTIME = "GameStartTime";
    public static double GetGameStartTime(this Room room)
    {
        PhotonHashtable customProperty = room.CustomProperties;           // customProperty�� Ȯ��޼���� ��������!
        if (customProperty.TryGetValue(GAMESTART, out object value))          // object �� �����ͼ�
        {
            return (double)value;                                             // bool�� ��ȯ�ؼ� ���
        }
        else
        {
            return 0;
        }
    }

    public static void SetGameStartTime(this Room room, double value)
    {
        property.Clear();
        property[GAMESTARTTIME] = value;
        room.SetCustomProperties(property);
    }
}
