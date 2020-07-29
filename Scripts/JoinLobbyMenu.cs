using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerGITS networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    public static string NetworkAddress { get; private set; }

    private const string PlayerPrefsNetworkAddressKey = "localhost";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        //set to localhost by default
        if (!PlayerPrefs.HasKey(PlayerPrefsNetworkAddressKey)) 
        { 
            ipAddressInputField.text = "localhost"; 
        }

        ipAddressInputField.text = PlayerPrefs.GetString(PlayerPrefsNetworkAddressKey);
    }

    private void OnEnable()
    {
        NetworkManagerGITS.OnClientConnected += HandleClientConnected;
        NetworkManagerGITS.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerGITS.OnClientConnected -= HandleClientConnected;
        NetworkManagerGITS.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }

    public void SetNetworkAddress()
    {
        string networkaddress = ipAddressInputField.text;

        joinButton.interactable = !string.IsNullOrEmpty(networkaddress);
    }

    public void SaveNetworkAddress()
    {
        NetworkAddress = ipAddressInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNetworkAddressKey, NetworkAddress);
    }
}
