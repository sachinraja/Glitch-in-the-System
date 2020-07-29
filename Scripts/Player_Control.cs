using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private int Health = 100;

    private NetworkManagerGITS room;

    [SerializeField] private Canvas playerNameCanvas;
    [SerializeField] private TMP_Text playerNamePrefab;

    public Material Dissolve;

    private NetworkManagerGITS Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerGITS;
        }
    }

    [Client]
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var player in Room.GamePlayers)
        {
            if (player.hasAuthority)
            {
                if (hasAuthority)
                {
                    CmdNamePlayer(player.displayName);
                    CmdColorPlayer(player.characterColor);
                }

                break;
            }
        }
    }

    [Client]
    void Update()
    {
        if (!hasAuthority) { return; }
    }

    [Command]
    void CmdNamePlayer(string name)
    {
        RpcNamePlayer(name);
    }

    [Command]
    void CmdColorPlayer(Color col)
    {
        RpcColorPlayer(col);
    }

    [ClientRpc]
    void RpcColorPlayer(Color col)
    {
        GetComponent<Renderer>().material.SetColor("_HologramColor", col);
    }

    [ClientRpc]
    void RpcNamePlayer(string name)
    {
        Vector3 textPos = Camera.main.WorldToScreenPoint(transform.position);
        textPos.y += 150.0f;

        TMP_Text playerName = Instantiate(playerNamePrefab, textPos, Quaternion.identity, playerNameCanvas.transform);

        playerName.text = name;
    }
}
