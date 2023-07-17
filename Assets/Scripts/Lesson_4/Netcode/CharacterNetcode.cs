using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class CharacterNetcode : NetworkBehaviour
{
    protected Action OnUpdateAction { get; set; }
    protected abstract FireActionNetcode fireAction { get; set; }
    protected NetworkVariable<Vector3> serverPosition = new NetworkVariable<Vector3>();

    protected virtual void Initiate()
    {
        OnUpdateAction += Movement;
    }
    private void Update()
    {
        OnUpdate();
    }
    private void OnUpdate()
    {
        OnUpdateAction?.Invoke();
    }
    [ServerRpc]
    protected void CmdUpdatePositionServerRpc(Vector3 position)
    {
        serverPosition.Value = position;
    }
    public abstract void Movement();
}