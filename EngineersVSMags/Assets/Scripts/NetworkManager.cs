using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // === Настройки ===
    public int udpPort = 8888;
    public int tcpPort = 9999;
    public bool isServer;

    private TcpListener tcpListener;
    private TcpClient tcpClient;
    private NetworkStream stream;

    private UdpClient udpBroadcaster;
    private Thread udpListenerThread;
    private Thread tcpListenerThread;
    private Thread tcpReceiveThread;

    public string messageFrom;

    private void Awake()
    {
        isServer = UnionClass.isServ;
    }
    void Start()
    {
        StartUdpBroadcast();
        StartUdpListen();

        if (isServer)
        {
            StartTcpServer();
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
      
            SendTcpMessage("Абоба");
        }
    }

    public void OnApplicationQuit()
    {
        udpBroadcaster?.Close();
        udpListenerThread?.Abort();
        tcpListenerThread?.Abort();
        tcpReceiveThread?.Abort();
        tcpClient?.Close();
        tcpListener?.Stop();
    }

    public void NetworkExit() 
    {
        udpBroadcaster?.Close();
        udpListenerThread?.Abort();
        tcpListenerThread?.Abort();
        tcpReceiveThread?.Abort();
        tcpClient?.Close();
        tcpListener?.Stop();
    }

    // === UDP ===
    void StartUdpBroadcast()
    {
        udpBroadcaster = new UdpClient();
        udpBroadcaster.EnableBroadcast = true;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, udpPort);

        InvokeRepeating(nameof(SendUdpBroadcast), 0f, 2f);
    }

    void SendUdpBroadcast()
    {
        string msg = "HELLO_GAME";
        byte[] data = Encoding.UTF8.GetBytes(msg);
        udpBroadcaster.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, udpPort));
    }

    void StartUdpListen()
    {
        udpListenerThread = new Thread(() =>
        {
            UdpClient listener = new UdpClient(udpPort);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, udpPort);

            while (true)
            {
                byte[] data = listener.Receive(ref remoteEP);
                string msg = Encoding.UTF8.GetString(data);
                if (msg == "HELLO_GAME" && remoteEP.Address.ToString() != GetLocalIPAddress())
                {
                    //Debug.Log("[UDP] Обнаружен игрок: " + remoteEP.Address);

                    if (!isServer && tcpClient == null)
                        ConnectToServer(remoteEP.Address.ToString());
                }
            }
        });
        udpListenerThread.IsBackground = true;
        udpListenerThread.Start();
    }

    // === TCP Server ===
    void StartTcpServer()
    {
        tcpListenerThread = new Thread(() =>
        {
            tcpListener = new TcpListener(IPAddress.Any, tcpPort);
            tcpListener.Start();
            Debug.Log("[TCP] Сервер ждёт подключения...");

            tcpClient = tcpListener.AcceptTcpClient();
            Debug.Log("[TCP] Клиент подключился");

            stream = tcpClient.GetStream();
            StartReceiving();
        });
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    // === TCP Client ===
    void ConnectToServer(string ip)
    {
        try
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(ip), tcpPort);

            Debug.Log("[TCP] Подключено к серверу: " + ip);

            stream = tcpClient.GetStream();
            StartReceiving();
        }
        catch (Exception e)
        {
            Debug.LogError("[TCP] Ошибка подключения: " + e.Message);
        }
    }

    // === TCP Communication ===
    public void StartReceiving() //Тут получает сообщение
    {
        tcpReceiveThread = new Thread(() =>
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int len = stream.Read(buffer, 0, buffer.Length);
                    if (len == 0) break;
                    string msg = Encoding.UTF8.GetString(buffer, 0, len);
                    messageFrom = msg;
                    Debug.Log("[TCP] Получено: " + msg);
                }
                catch
                {
                    break;
                }
            }
        });
        tcpReceiveThread.IsBackground = true;
        tcpReceiveThread.Start();
    }

    

    public void SendTcpMessage(string msg)
    {
        if (tcpClient != null && tcpClient.Connected)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent message: " + msg);
        }
        else
        {
            Debug.LogWarning("TCP client not connected!");
        }
    }

    string GetLocalIPAddress()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "127.0.0.1";
    }
}
