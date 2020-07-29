using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NetworkGamePlayerGITS : NetworkBehaviour
{
    [SyncVar]
    public string displayName = "Loading...";

    [SyncVar]
    public Color characterColor = Color.black;

    private NetworkManagerGITS room;

    [SerializeField]
    private GameObject scrollView = null;

    [SerializeField] private GameObject chatUI = null;

    [SerializeField] private TMP_Text chatText = null;

    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    private NetworkManagerGITS Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerGITS;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayname)
    {
        this.displayName = displayname;
    }

    [Server]
    public void SetCharacterColor(Color charactercolor)
    {
        this.characterColor = charactercolor;
    }

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (hasAuthority)
        {
            OnMessage -= HandleNewMessage;
        }
    }

    [Client]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            scrollView.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > Screen.width * 0.25f)
        {
            scrollView.SetActive(false);
        }
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

    [Client]
    public void Send()
    {
        string message = inputField.text;

        if (Input.GetKeyDown(KeyCode.Return) && string.IsNullOrWhiteSpace(message) == false)
        {
            CmdSendMessage(message);

            inputField.text = string.Empty;
        }
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"{gameObject.GetComponent<NetworkGamePlayerGITS>().displayName}: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
