using Unity.Netcode;
using UnityEngine;

public class PlayerNetcode : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab; 
    public void SpawnCharacter()
    {
        if (!IsServer)
        {
            return;
        }
        _playerPrefab = Resources.Load<GameObject>("Player");
    }   
}
