using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private List<Transform> _spawnPoints;

    private void Awake()
    {
        CreateSpawnPoints();
    }

    private void CreateSpawnPoints()
    {
        SpawnPoint[] spawnPoints = GetComponentsInChildren<SpawnPoint>();
        _spawnPoints = new List<Transform>(spawnPoints.Length);

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            _spawnPoints.Add(spawnPoint.transform);
        }
    }

    public Transform GetNextPoint()
    {
        Player[] players = PhotonNetwork.PlayerList;
        int i = 0;
        while (players[i].IsLocal == false)
        {
            i++;
        }

        i = Repeat(i, _spawnPoints.Count);

        Transform result = _spawnPoints[i];
        return result;
    }

    private static int Repeat(int t, int length)
    {
        if (t >= length)
        {
            return length % t;
        }
        else
        {
            return t;
        }
    }
}
