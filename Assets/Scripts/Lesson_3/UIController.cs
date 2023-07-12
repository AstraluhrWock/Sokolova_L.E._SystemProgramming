using UnityEngine;
using UnityEngine.UI;
using TMPro;

internal sealed class UIController : MonoBehaviour
{
    [SerializeField] private Button _buttonStartServer;
    [SerializeField] private Button _buttonShutDownServer;
    [SerializeField] private Button _buttonConnectClient;
    [SerializeField] private Button _buttonDisconnectClient;
    [SerializeField] private Button _buttonSendMessage;

    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextField _textField; 

    [SerializeField] Server _server;
    [SerializeField] Client _client;

    private void Start()
    {
        _buttonStartServer.onClick.AddListener(() => StartServer());
        _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        _buttonConnectClient.onClick.AddListener(() => ConnectClient());
        _buttonDisconnectClient.onClick.AddListener(() => DisconnectClient());
        _buttonSendMessage.onClick.AddListener(() => SendMessage());
        _client.onMessageReceive += ReceiveMessage;
    }

    private void StartServer()
    {
        _server.StartServer();
    }

    private void ShutDownServer()
    {
        _server.ShutDownServer();
    }

    private void ConnectClient()
    {
        _client.Connect();
    }

    private void DisconnectClient()
    {
        _client.Disconnect();
    }

    private void SendMessage()
    {
        _client.SendMessage(_inputField.text);
        _inputField.text = "";
    }

    public void ReceiveMessage(object message)
    {
        _textField.ReceiveMessage(message);
    }
}

