using UnityEngine;
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

    const int DISCONNECT_TIMEOUT = 5000;
    const int HEARTBEAT_TIMEOUT_MS = 500;
    const ushort SERVER_PORT = 9000;
    const int MAX_CONNECTIONS = 4;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
	
    void Start()
    {
        var settings = new NetworkSettings();
        settings.WithNetworkConfigParameters(disconnectTimeoutMS: DISCONNECT_TIMEOUT, heartbeatTimeoutMS: HEARTBEAT_TIMEOUT_MS);
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

        ModuleEventManager.instance.onModuleEntered.AddListener(
            (id, type, dict) => {
                dict.Add("event", MessageEvent.START_MINIGAME.ToString());
                dict.Add("player", id.ToString());
                dict.Add("minigame", type.ToString());
                SendMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dict)), id);
            }
        );
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
                    // Debug.Log("Reading data...");
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

    bool HandleButtonPushedEvent(Dictionary<string, string> dict_message)
    {
        try {
            int id = int.Parse(dict_message["player"]);
            string buttonName = dict_message["button_name"];
            string buttonEvent = dict_message["button_event"];
            
            if (buttonName.Equals("Button A") && buttonEvent.Equals("started")) {
                CrewmateEventManager.instance.onCrewmateButtonAPushed.Invoke(id);
                return true;
            }
            else if (buttonName.Equals("Button A") && buttonEvent.Equals("ended")) {
                CrewmateEventManager.instance.onCrewmateButtonAReleased.Invoke(id);
                return true;
            }
            else if (buttonName.Equals("Button B") && buttonEvent.Equals("started")) {
                CrewmateEventManager.instance.onCrewmateButtonBPushed.Invoke(id);
                return true;
            }
            else if (buttonName.Equals("Button B") && buttonEvent.Equals("ended")) {
                CrewmateEventManager.instance.onCrewmateButtonBReleased.Invoke(id);
                return true;
            }
            return false;
        } catch (Exception) {
            return false;
        }
    }

    bool HandleJoystickPositionEvent(Dictionary<string, string> dict_message)
    {
        try {
            int id = int.Parse(dict_message["player"]);
            float inputX = float.Parse(dict_message["x"]);
            float inputY = float.Parse(dict_message["y"]);
            CrewmateEventManager.instance.onCrewmateMoveInputUpdate.Invoke(id, inputX, inputY);
            return true;
        } catch (Exception) {
            return false;
        }
    }

    bool HandleUpdateMinigameEvent(Dictionary<string, string> dict_message)
    {
        try {
            int id = int.Parse(dict_message["player"]);
            if (dict_message.ContainsKey("energy")) { /* ENERGY_MODULE */

                int energy = int.Parse(dict_message["energy"]);
                if (energy == 1) {
                    ModuleEventManager.instance.onEnergyModuleUpdate.Invoke(id);
                }
                return true;

            } else if (dict_message.ContainsKey("parameter")) { /* GYROSCOPE_MODULE */

                float parameter = float.Parse(dict_message["parameter"]);
                ModuleEventManager.instance.onGyroscopeModuleUpdate.Invoke(id, parameter);
                return true;

            } else if (dict_message.ContainsKey("shield")) { /* SHIELD_MODULE */

                int shield = int.Parse(dict_message["shield"]);
                if (shield == 1) {
                    ModuleEventManager.instance.onShieldModuleUpdate.Invoke(id);
                }
                return true;

            } else if (dict_message.ContainsKey("speed_pos")) { /* MOVEMENT_MODULE - speed */

                float speed_pos = float.Parse(dict_message["speed_pos"]);
                ModuleEventManager.instance.onSpeedModuleUpdate.Invoke(id, speed_pos);
                return true;

            } else if (dict_message.ContainsKey("steering_pos")) { /* MOVEMENT_MODULE - steering */

                float steering_pos = float.Parse(dict_message["steering_pos"]);
                ModuleEventManager.instance.onSteeringModuleUpdate.Invoke(id, steering_pos);
                return true;

            } else if (dict_message.ContainsKey("aim_pos")) { /* CANNON_MODULE - aiming */

                float aim_pos = float.Parse(dict_message["aim_pos"]);
                ModuleEventManager.instance.onCannonAimModuleUpdate.Invoke(id, aim_pos);
                return true;

            } else if (dict_message.ContainsKey("chamber_open")) { /* CANNON_MODULE - is chamber open */

                bool chamber_open = bool.Parse(dict_message["chamber_open"]);
                if (chamber_open) {
                    ModuleEventManager.instance.onCannonModuleChamberOpened.Invoke(id);
                } else {
                    ModuleEventManager.instance.onCannonModuleChamberClosed.Invoke(id);
                }
                return true;

            } else if (dict_message.ContainsKey("chamber_loaded")) { /* CANNON_MODULE - is chamber loaded */

                bool chamber_loaded = bool.Parse(dict_message["chamber_loaded"]);
                if (chamber_loaded) {
                    ModuleEventManager.instance.onCannonModuleChamberLoaded.Invoke(id);
                } else {
                    ModuleEventManager.instance.onCannonModuleChamberUnloaded.Invoke(id);
                }
                return true;

            } else if (dict_message.ContainsKey("fire_event")) { /* CANNON_MODULE - fire event */

                string fire_event = dict_message["fire_event"];
                if (fire_event.Equals("fired")) {
                    ModuleEventManager.instance.onCannonModuleFired.Invoke(id);
                }
                return true;

            }

            return false;
        } catch (Exception) {
            return false;
        }
    }

    bool HandleAbortMinigameEvent(Dictionary<string, string> dict_message)
    {
        try {
            int id = int.Parse(dict_message["player"]);
            ModuleEventManager.instance.onMinigameAborted.Invoke(id);
            return true;
        } catch (Exception) {
            return false;
        }
    }

    void HandleMessage(byte[] message, int connectionId)
    {
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
                bool success = HandleButtonPushedEvent(dict_message);
                if (!success) {
                    Debug.Log("Unrecognised button event: " + Encoding.UTF8.GetString(message));
                }
            }
            else if (dict_message["event"].Equals(MessageEvent.JOYSTICK_POSITION.ToString()))
            {
                bool success = HandleJoystickPositionEvent(dict_message);
                if (!success) {
                    Debug.Log("Unrecognised joystick event: " + Encoding.UTF8.GetString(message));
                }
            }
            else if (dict_message["event"].Equals(MessageEvent.UPDATE_MINIGAME.ToString()))
            {
                bool success = HandleUpdateMinigameEvent(dict_message);
                if (!success) {
                    Debug.Log("Unrecognised minigame update event: " + Encoding.UTF8.GetString(message));
                }
            }
            else if (dict_message["event"].Equals(MessageEvent.ABORT_MINIGAME.ToString()))
            {
                bool success = HandleAbortMinigameEvent(dict_message);
                if (!success) {
                    Debug.Log("Unrecognised minigame abort event: " + Encoding.UTF8.GetString(message));
                }
            }
            else
            {
                Debug.Log("Unrecognised message: " + Encoding.UTF8.GetString(message));
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

        // Debug.Log("Sending data...");
        NativeArray<byte> buffer = new NativeArray<byte>(message, Allocator.Temp);
        m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[playerId], out var writer);
        writer.WriteBytes(buffer);
        m_Driver.EndSend(writer);
        buffer.Dispose();
    }

}
