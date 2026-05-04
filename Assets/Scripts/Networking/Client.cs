using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using NetworkConnections;
using OSCTools;

/// <summary>
/// The client is the class that lets game code (Controller and View classes) communicate with 
/// the server, and handles network connections.
/// </summary>
public class Client : MonoBehaviour
{
	// ----- General client things:
	public IPAddress ServerIP = IPAddress.Loopback;
	TcpNetworkConnection connection;
	OSCDispatcher dispatcher;

	
	
	// ----- Game client things:

	// Views subscribe here, on any client:
	public static event Action OnInstanceReady;
	public event Action OnStartRound;
	public event Action<int> OnStateChanged;
	public event Action<int> OnActivePlayerChanged;
	public event Action<int> OnSelectedShipPlayer1Changed;
	public event Action<int> OnSelectedShipPlayer2Changed;
	public event Action<bool> OnReady1Changed;
	public event Action<bool> OnReady2Changed;
	public event Action<int[,]> OnShipsGrid1Changed;
	public event Action<int[,]> OnShipsGrid2Changed;
	public event Action<int[,]> OnShotsGrid1Changed;
	public event Action<int[,]> OnShotsGrid2Changed;

	//idk wtf this is
	public event System.Action<int> OnPlayerInfoReceived;


	void Start()
    {
		TcpClient client = new TcpClient();
		client.Connect(new IPEndPoint(ServerIP, 50006));
		connection = new TcpNetworkConnection(client);
		// TODO: error handling

		Debug.Log("Starting client, connecting to " + ServerIP);

		// Initialize the dispatcher and callbacks for incoming OSC messages:
		dispatcher = new OSCDispatcher();
		dispatcher.ShowIncomingMessages = true;
		Initialize();
    }

	/// <summary>
	/// Called from NetworkConnection callback (connection.Update), when a packet arrives:
	/// </summary>
	void HandlePacket(byte[] packet, IPEndPoint remote) {
		OSCMessageIn mess = new OSCMessageIn(packet);
		Debug.Log("Message arrives on client: " + mess);
		dispatcher.HandlePacket(packet, remote);
	}

	void Update()
    {
		// Check for incoming packets, and deal with them:
		while (connection.Available()>0) {
			HandlePacket(connection.GetPacket(), connection.Remote);
		}
		// TODO: disconnect handling
    }

	void Initialize() {
		// The (optional) list of parameter types (OSCUtil.INT) lets the dispatcher filter
		//  messages that do not satisfy the expected signature (=parameter list):
		
		
		
		
		// Listens to the actual messages, currently errors
		
		//dispatcher.AddListener("/CellChange", CellChangeRpc, OSCUtil.INT, OSCUtil.INT, OSCUtil.INT); 
		//dispatcher.AddListener("/ActivePlayer", ActivePlayerChangeRpc, OSCUtil.INT);
		//dispatcher.AddListener("/GameOver", GameOverRpc, OSCUtil.INT);
		//dispatcher.AddListener("/PlayerInfo", PlayerInfoRpc, OSCUtil.INT);
	}

	// ----- Incoming RPCs (events are triggered, and View classes subscribe):
	
	void PlayerInfoRpc(OSCMessageIn message, IPEndPoint remote) {
		int playerIndex = message.ReadInt();
		OnPlayerInfoReceived?.Invoke(playerIndex);
	}

	// ----- Outgoing RPCs (called from Controller):

	public void MakeMoveRequest(int row, int col) {
		OSCMessageOut message = new OSCMessageOut("/MakeMove").AddInt(row).AddInt(col);
		connection.Send(message.GetBytes());
	}
	public void ResetRequest() {
		OSCMessageOut message = new OSCMessageOut("/Reset");
		connection.Send(message.GetBytes());
	}
}