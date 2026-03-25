using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net; // For IPAddress
using System.Net.Sockets;
using System.Threading;
using NetworkConnections; // For TcpListener, TcpClient
using Rug.Osc;
using UnityEngine;

class TcpServer
{
	static void StartServerAtPort50001()
	{
		StartServer(50001);
	}

	public static void StartServer(int port)
	{
		TcpServer.ServerLoop(port);
	}
	static IEnumerator ServerLoop(int port)
	{
		TcpListener listener = new TcpListener(IPAddress.Any, port);
		listener.Start();
		
		List<TcpNetworkConnection> clients = new List<TcpNetworkConnection>();

		while (true) {
			AcceptNewClients(listener, clients);
			HandleMessages(clients);
			CleanupClients(clients);
			
			Thread.Sleep(10);
		}
		
		foreach (TcpNetworkConnection connection in clients) {
			connection.Close();
		}
		listener.Stop();
		Debug.Log("Server stopped");
	}
	static void AcceptNewClients(TcpListener listener, List<TcpNetworkConnection> clients) {
		if (listener.Pending()) {
			TcpClient newTcpClient = listener.AcceptTcpClient();
			TcpNetworkConnection connection = new TcpNetworkConnection(newTcpClient);
			
			clients.Add(connection);
		}
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
	static void HandleMessages(List<TcpNetworkConnection> clients) {
		for (int i = clients.Count - 1; i >= 0; i--) {
			TcpNetworkConnection client = clients[i];

			// Currently broken. dont care rn.
			// if (client.Available <= 0) return;
			//
			//
			// NetworkStream stream = client.GetStream();
			//
			// try
			// {
			// 	byte[] data = ReadFullMessage(stream);
			// 	OscMessage message = OscMessage.Read(data, data.Length);
			// 	SendMessage(client, message);
			// }
			// catch (IOException e)
			// {
			// 	Debug.LogWarning($"Client disconnected: {e.Message}");
			// 	client.Close();
			// 	clients.Remove(client);
			// }
		}
	}
	static void ArrangeMessage(byte[] data)
	{
		
	}
	static void SendMessage(TcpClient client, OscMessage message)
	{
		byte[] data = message.ToByteArray();
		byte[] length = BitConverter.GetBytes(data.Length);
		
		NetworkStream stream = client.GetStream();
		stream.Write(length, 0, 4); // 32 bit
		stream.Write(data, 0, data.Length);
	}
	static void CleanupClients(List<TcpNetworkConnection> clients) {
		for (int i = clients.Count - 1; i >= 0; i--) {
			// If any of our current clients are disconnected, 
			// we close the TcpClient to clean up resources, and remove it from our list:
			// (Note that this type of for loop is needed since we're modifying the collection inside the loop!)
			
			// dont work, dont care rn
			// if (!clients[i].Status.Connected) {
			// 	clients[i].Close();
			// 	clients.RemoveAt(i);
			// 	//Debug.Log($"Removing client. Number of connected clients: {clients.Count}");
			// }
		}
	}
}