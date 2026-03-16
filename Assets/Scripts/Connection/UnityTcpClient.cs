using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net; // For IPAddress
using System.Net.Sockets; // For TcpClient
using System.Text;
using System.Threading;
using Rug.Osc;
using UnityEngine;

public class UnityTcpClient
{
	private static TcpClient client;
	
	static ConcurrentQueue<OscMessage> messageQueue = new ConcurrentQueue<OscMessage>();

	public static event Action<OscMessage> OnMessageReceived;
	
	

	public static void StartClient() {
		int remotePort = 50001;
		
		StartConnection(0, remotePort);
	}
	static void StartConnection(int localPort, int remotePort) {
		// There is no error handling here. (Where) should it be added?

		// If localPort is zero, start a TcpClient on an arbitrary port.
		// Otherwise, start it on the given local port:
		client = localPort>0?
			new TcpClient(new IPEndPoint(IPAddress.Any,localPort)):
			new TcpClient();
		
		client.Connect("127.0.0.1", remotePort);
		Debug.Log($"Starting TCP client on {client.Client.LocalEndPoint}, connected to {client.Client.RemoteEndPoint}");

		HandleMessages();
		
		Debug.Log("Closing client");
		client.Close();
	}
	static byte[] ReadFullMessage(NetworkStream stream)
	{
		int size = 4; // 32 bit
		
		byte[] lengthBytes = new byte[size];
		int offset = 0;
		while (offset < size)
		{
			int read = stream.Read(lengthBytes, offset, size - offset);
			if (read == 0) {
				Debug.LogError("Connection closed unexpectedly :(");
				break;
			}
			offset += read;
		}
		
		
		
		size = BitConverter.ToInt32(lengthBytes, 0);

		byte[] data = new byte[size];
		offset = 0;
		while (offset < size)
		{
			int read = stream.Read(data, offset, size - offset);
			if (read == 0) {
				Debug.LogError("Connection closed unexpectedly :(");
				break;
			}
			offset += read;
		}
		
		return data;
	}
	static void HandleMessages() {
		NetworkStream stream = client.GetStream();

		while (client.Available > 0)
		{
			byte[] data = ReadFullMessage(stream);
			OscMessage message = OscMessage.Read(data, data.Length);
			messageQueue.Enqueue(message);
		}
	}
	public static void ProcessMessages()
	{
		while (messageQueue.TryDequeue(out OscMessage message))
		{
			OnMessageReceived?.Invoke(message);
			Debug.Log(message.Address + " | " + message.Count);
		}
	}
	
	public static void SendMessage(OscMessage message)
	{
		byte[] data = message.ToByteArray();
		byte[] length = BitConverter.GetBytes(data.Length);
		
		NetworkStream stream = client.GetStream();
		stream.Write(length, 0, 4); // 32 bit
		stream.Write(data, 0, data.Length);
	}
}