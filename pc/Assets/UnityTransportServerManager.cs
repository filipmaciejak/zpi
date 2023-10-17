using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using System.Text;
using System.IO;
using Unity.VisualScripting;
using System.Collections.Generic;
using Newtonsoft.Json;

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

public class ServerManager : MonoBehaviour
{
    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;
    int maxPlayerId = 0;

    void Start()
    {
        m_Driver = NetworkDriver.Create(new WebSocketNetworkInterface());
        var endpoint = NetworkEndpoint.AnyIpv4;
        Debug.Log("Ip: " + endpoint.Address);
        endpoint.Port = 9000;
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log("Failed to bind to port 9000");
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }

    void Update()
    {
        m_Driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
                continue;
            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    Debug.Log("Reading data...");
                    NativeArray<byte> buffer = new NativeArray<byte>(stream.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
                    stream.ReadBytes(buffer);
                    HandleMessage(buffer.ToArray());
                    buffer.Dispose();
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    void HandleMessage(byte[] message)
    {
        Debug.Log(Encoding.UTF8.GetString(message));
        
        Dictionary<string, string> dict_message = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(message));
        if (dict_message["event"].Equals(MessageEvent.GET_PLAYER_ID.ToString()))
        {
            Dictionary<string, string> dict_response = new Dictionary<string, string>();
            dict_response.Add("event", MessageEvent.SET_PLAYER_ID.ToString());
            dict_response.Add("player", maxPlayerId++.ToString());
            SendMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict_response)), maxPlayerId - 1);
        }
        else if (dict_message["event"].Equals(MessageEvent.BUTTON_PUSHED))
        {
            //handle using dict_response["parameter"]
        }
        else if (dict_message["event"].Equals(MessageEvent.JOYSTICK_POSITION))
        {
            //handle using dict_response["parameter_x"] i dict_response["parameter_y"]
        }
        else
        {
            //handle invalid message
        }
    }

    //TO DO: lepsza translacja playerId do connection, bo obecna może się rozjechać przy problemach z połączeniem
    void SendMessage(byte[] message, int playerId)
    {
        Debug.Log("Sending data...");
        NativeArray<byte> buffer = new NativeArray<byte>(message, Allocator.Temp);
        m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[playerId], out var writer);
        writer.WriteBytes(buffer);
        m_Driver.EndSend(writer);
        buffer.Dispose();
    }
}
