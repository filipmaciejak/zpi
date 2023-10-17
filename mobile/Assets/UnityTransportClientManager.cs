using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;
using Newtonsoft.Json;
using System;

enum MessageEvent
{
    GET_PLAYER_ID = 0,
    SET_PLAYER_ID,
    BUTTON_PUSHED,
    JOYSTICK_POSITION,
    START_MINIGAME,
    ABORT_MINIGAME,
    FINISH_MINIGAME_PART,
}

public class ClientManager : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool Done;
    int playerId = -1;


    void Start()
    {
        m_Driver = NetworkDriver.Create(new WebSocketNetworkInterface());
        m_Connection = default(NetworkConnection);

        var endpoint = NetworkEndpoint.Parse("192.168.209.21", 9000);
        m_Connection = m_Driver.Connect(endpoint);
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
            if (!Done)
                Debug.Log("Something went wrong during connect");
            return;
        }
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");

                if(playerId == -1)
                {
                    Dictionary<string, string> dict_message = new Dictionary<string, string>();
                    dict_message.Add("event", MessageEvent.GET_PLAYER_ID.ToString());
                    Debug.Log(JsonConvert.SerializeObject(dict_message));
                    SendMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict_message)));
                }
                //uint value = 1;
                //m_Driver.BeginSend(m_Connection, out var writer);
                //writer.WriteUInt(value);
                //m_Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                Debug.Log("Reading Data...");
                NativeArray<byte> buffer = new NativeArray<byte>(stream.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
                stream.ReadBytes(buffer);
                HandleMessage(buffer.ToArray());
                buffer.Dispose();
                Done = true;
                m_Connection.Disconnect(m_Driver);
                m_Connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                m_Connection = default(NetworkConnection);
            }
        }
    }

    void HandleMessage(byte[] message)
    {
        Debug.Log(Encoding.UTF8.GetString(message));

        Dictionary<string, string> dict_message = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(message));
        if (dict_message["event"].Equals(MessageEvent.SET_PLAYER_ID.ToString()))
        {
            playerId = Int32.Parse(dict_message["player"]);
            Debug.Log($"Player ID: {playerId}");
        }
        else
        {
            //handle invalid message
        }
        
    }

    void SendMessage(byte[] message)
    {
        Debug.Log("Sending data...");
        NativeArray<byte> buffer = new NativeArray<byte>(message, Allocator.Temp);
        m_Driver.BeginSend(NetworkPipeline.Null, m_Connection, out var writer);
        writer.WriteBytes(buffer);
        m_Driver.EndSend(writer);
        buffer.Dispose();
    }
}
