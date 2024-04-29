using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;                  // 이름이 기니까 using을 사용해서 간단하게 바꾸자

public static class CustomProperty      // 확장메서드
{
    static PhotonHashtable property = new PhotonHashtable();

    public const string READY = "Ready";
    public const string LOAD = "Load";

    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;           // customProperty를 확장메서드로 구현하자!
        if (customProperty.TryGetValue(READY, out object value))          // object 로 가져와서
        {
            return (bool)value;                                             // bool로 변환해서 사용
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
        PhotonHashtable customProperty = player.CustomProperties;           // customProperty를 확장메서드로 구현하자!
        if (customProperty.TryGetValue(LOAD, out object value))          // object 로 가져와서
        {
            return (bool)value;                                             // bool로 변환해서 사용
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
        PhotonHashtable customProperty = room.CustomProperties;           // customProperty를 확장메서드로 구현하자!
        if (customProperty.TryGetValue(GAMESTART, out object value))          // object 로 가져와서
        {
            return (bool)value;                                             // bool로 변환해서 사용
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
        PhotonHashtable customProperty = room.CustomProperties;           // customProperty를 확장메서드로 구현하자!
        if (customProperty.TryGetValue(GAMESTART, out object value))          // object 로 가져와서
        {
            return (double)value;                                             // bool로 변환해서 사용
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
