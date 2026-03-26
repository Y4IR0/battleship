using UnityEngine;

/// <summary>
/// The scene starts with a SessionManager, which allows use to choose whether this instance
/// will be client or host (both client and server).
/// </summary>
public class SessionManager : MonoBehaviour
{
    MoveMaker controller;

    bool IsClient = false;
    bool IsServer = false;

    
    
    public void SelectSessionType(bool isHost) {
        if (isHost)
        {
            StartServer();
            StartClient();
        }
        else
        {
            StartClient();
        }
    }

    void StartServer() {
        Debug.Log("Starting server: creating board");

        Server server = GetComponent<Server>();
        server.enabled = true;

        IsServer = true;
    }
    void StartClient() {
        Debug.Log($"Starting client: enabling controller");

        Client client = GetComponent<Client>();
        client.enabled = true;

        controller = FindFirstObjectByType<MoveMaker>();
        controller.enabled = true;

        IsClient = true;
    }
}
