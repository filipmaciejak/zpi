using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

enum MessageEvent
{
    GET_PLAYER_ID = 0,
    SET_PLAYER_ID,
    BUTTON_PUSHED,
    JOYSTICK_POSITION,
    START_MINIGAME,
    ABORT_MINIGAME,
    UPDATE_MINIGAME,
}

public class ClientManager : MonoBehaviour
{
    NetworkDriver m_Driver;
    NetworkConnection m_Connection;
    int playerId = -1;

    const int DISCONNECT_TIMEOUT_MS = 5000;
    const int CONNECT_TIMEOUT_MS = 5000;
    const int HEARTBEAT_TIMEOUT_MS = 500;
    const int MAX_CONNECT_ATTEMPTS = 3;
    const ushort SERVER_PORT = 9000;

    public event Action<Dictionary<string, string>> StartMinigame; 
    public event Action<Dictionary<string, string>> UpdateMinigame;
    public event Action Connect;
    public event Action Disconnect;

    void Start()
    {
        var settings = new NetworkSettings();
        settings.WithNetworkConfigParameters(disconnectTimeoutMS: DISCONNECT_TIMEOUT_MS, connectTimeoutMS: CONNECT_TIMEOUT_MS, maxConnectAttempts: MAX_CONNECT_ATTEMPTS, heartbeatTimeoutMS: HEARTBEAT_TIMEOUT_MS);
        m_Driver = NetworkDriver.Create(new WebSocketNetworkInterface(), settings);
        m_Connection = default(NetworkConnection);
    }

    public void OnDestroy() 
    {
        m_Driver.Dispose();
    }


    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");

                Dictionary<string, string> dict_message = new Dictionary<string, string>();
                dict_message.Add("event", MessageEvent.GET_PLAYER_ID.ToString());
                dict_message.Add("player", playerId.ToString());
                Debug.Log(JsonConvert.SerializeObject(dict_message));
                SendClientMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict_message)));
                Connect?.Invoke();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                Debug.Log("Reading Data...");
                NativeArray<byte> buffer = new NativeArray<byte>(stream.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
                stream.ReadBytes(buffer);
                HandleMessage(buffer.ToArray());
                buffer.Dispose();
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client failed to connect or got disconnected from server");
                m_Connection = default(NetworkConnection);
                if(SceneManager.GetActiveScene().name != "QrReadingScene")
                    SceneManager.LoadScene("QrReadingScene");
                Disconnect?.Invoke();
            }
        }
    }

    void HandleMessage(byte[] message)
    {
        Debug.Log(Encoding.UTF8.GetString(message));
        
        Dictionary<string, string> dictMessage;
        try
        {
            dictMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(message));
        } catch (Exception) {
            Debug.Log("Got invalid message: " + Encoding.UTF8.GetString(message));
            return;
        }
        
        Boolean conversionSuccess = Enum.TryParse(dictMessage["event"], out MessageEvent eventType);
        if (!conversionSuccess)
        {
            Debug.Log("Got invalid MessageEvent: " + dictMessage["event"]);
            return;
        }
        
        switch (eventType)
        {
            case MessageEvent.SET_PLAYER_ID:
                OnSetPlayerId(dictMessage);
                break;
            case MessageEvent.START_MINIGAME:
                StartMinigame?.Invoke(dictMessage);
                break;
            case MessageEvent.UPDATE_MINIGAME:
                UpdateMinigame?.Invoke(dictMessage);
                break;
            default:
                Debug.Log("Unhandled message event: " + dictMessage["event"]);
                break;
        }
    }

    public void SendClientMessage(byte[] message)
    {
        if (!m_Connection.IsCreated)
        {
            Debug.Log("Connection not established. Unable to send message");
            return;
        }

        Debug.Log("Sending data...");
        NativeArray<byte> buffer = new NativeArray<byte>(message, Allocator.Temp);
        m_Driver.BeginSend(NetworkPipeline.Null, m_Connection, out var writer);
        writer.WriteBytes(buffer);
        m_Driver.EndSend(writer);
        buffer.Dispose();
    }

    public void SendDict(Dictionary<string,string> dict)
    {
        dict.Add("player", playerId.ToString());
        byte[] sentMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict));
        SendClientMessage(sentMessage);
    }

    public void ConnectToIp(string ipAddress)
    {
        var endpoint = NetworkEndpoint.Parse(ipAddress, SERVER_PORT);
        if (endpoint == default(NetworkEndpoint)) 
        {
            throw new Exception("Bad ip format");
        }

        m_Connection = m_Driver.Connect(endpoint);
    }

    private void OnSetPlayerId(Dictionary<string, string> message)
    {
        playerId = Int32.Parse(message["player"]);
        Debug.Log($"Player ID: {playerId}");
    }
}
