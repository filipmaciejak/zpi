using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public NetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public bool Done;


    void Start()
    {
        m_Driver = NetworkDriver.Create(new WebSocketNetworkInterface());
        m_Connection = default(NetworkConnection);

        var endpoint = NetworkEndpoint.LoopbackIpv4;
        endpoint.Port = 9000;
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

                string stringMessage = "Hello Server!";

                NativeArray<byte> data = new NativeArray<byte>(Encoding.UTF8.GetBytes(stringMessage), Allocator.Temp);
                m_Driver.BeginSend(m_Connection, out var writer);
                writer.WriteBytes(data);
                m_Driver.EndSend(writer);
                data.Dispose();
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                NativeArray<byte> buffer = new NativeArray<byte>(stream.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
                stream.ReadBytes(buffer);
                HandleMessages(buffer.ToArray());
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

    void HandleMessages(byte[] message)
    {
        Debug.Log(Encoding.UTF8.GetString(message));
    }

}
