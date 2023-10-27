﻿using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using System.Text;
using System.IO;
using Unity.VisualScripting;
using System.Collections.Generic;
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
    UPDATE_MINIGAME,
}

public class ServerManager : MonoBehaviour
{
    public NetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    const int DISCONNECT_TIMEOUT = 3000;
    const ushort SERVER_PORT = 9000;
    const int MAX_CONNECTIONS = 4;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
	
    void Start()
    {
        var settings = new NetworkSettings();
        settings.WithNetworkConfigParameters(disconnectTimeoutMS: DISCONNECT_TIMEOUT);
        m_Driver = NetworkDriver.Create(new WebSocketNetworkInterface(), settings);
        var endpoint = NetworkEndpoint.AnyIpv4;
        Debug.Log("Ip: " + endpoint.Address);
        endpoint.Port = SERVER_PORT;
        if (m_Driver.Bind(endpoint) != 0)
            Debug.Log("Failed to bind to port 9000");
        else
            m_Driver.Listen();

        m_Connections = new NativeList<NetworkConnection>(MAX_CONNECTIONS, Allocator.Persistent);
        for (int i = 0; i < m_Connections.Capacity; i++)
        {
            m_Connections.Add(default(NetworkConnection));
        }

        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
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

        /* kod z dokumentacji, raczej nieprzydatny
        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }
        */

        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection))
        {
            for(int i=0; i < m_Connections.Length; i++)
            {
                if (m_Connections[i] == default(NetworkConnection))
                {
                    m_Connections[i] = c;
                    Debug.Log("Accepted a connection");
                    break;
                }
            }
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
                    HandleMessage(buffer.ToArray(), i);
                    buffer.Dispose();
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    //TO DO: zatrzymaj grę i czekaj na połączenie
                    Debug.Log("Client disconnected from server");
                    m_Connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    void HandleMessage(byte[] message, int connectionId)
    {
        Debug.Log(Encoding.UTF8.GetString(message));
        
        try
        {
            Dictionary<string, string> dict_message = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(message));
            if (dict_message["event"].Equals(MessageEvent.GET_PLAYER_ID.ToString()))
            {
                int playerId = Int32.Parse(dict_message["player"]);
                if (playerId >= 0 && playerId < MAX_CONNECTIONS && m_Connections[playerId] == default(NetworkConnection)) {
                    m_Connections[playerId] = m_Connections[connectionId];
                    m_Connections[connectionId] = default(NetworkConnection);
                    connectionId = playerId;
                }

                Dictionary<string, string> dict_response = new Dictionary<string, string>();
                dict_response.Add("event", MessageEvent.SET_PLAYER_ID.ToString());
                dict_response.Add("player", connectionId.ToString());
                SendMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict_response)), connectionId);
            }
            else if (dict_message["event"].Equals(MessageEvent.BUTTON_PUSHED.ToString()))
            {
                int id = int.Parse(dict_message["player"]);
                if (dict_message["button_event"].Equals("started") && dict_message["button_name"].Equals("Button A"))
                    CrewmateEventManager.instance.onCrewmateJump.Invoke(id);
                else if (dict_message["button_event"].Equals("started") && dict_message["button_name"].Equals("Button B"))
                    CrewmateEventManager.instance.onCrewmateInteractionStart.Invoke(id);
                else if (dict_message["button_event"].Equals("ended") && dict_message["button_name"].Equals("Button B"))
                    CrewmateEventManager.instance.onCrewmateInteractionEnd.Invoke(id);
            }
            else if (dict_message["event"].Equals(MessageEvent.JOYSTICK_POSITION.ToString()))
            {
                int id = int.Parse(dict_message["player"]);
                float inputX = float.Parse(dict_message["x"]);
                float inputY = float.Parse(dict_message["y"]);
                CrewmateEventManager.instance.onCrewmateMoveInputUpdate.Invoke(id, inputX, inputY);
            }
            else
            {
                Debug.Log("Unrecognised message event: " + dict_message["event"]);
            }
        }
        catch (Exception)
        {
            Debug.Log("Got invalid message: " + Encoding.UTF8.GetString(message));
        }
    }

    void SendMessage(byte[] message, int playerId)
    {
        if (!m_Connections[playerId].IsCreated)
        {
            Debug.Log("Connection not established. Unable to send message");
            return;
        }

        Debug.Log("Sending data...");
        NativeArray<byte> buffer = new NativeArray<byte>(message, Allocator.Temp);
        m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[playerId], out var writer);
        writer.WriteBytes(buffer);
        m_Driver.EndSend(writer);
        buffer.Dispose();
    }


}
