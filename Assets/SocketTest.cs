using KyleDulce.SocketIo;
using System.Collections;
using UnityEngine;

public class SocketTest : MonoBehaviour
{
    Socket s;
    private bool isConnected = false;
    private float reconnectDelay = 5f;
    private string serverUrl = "ws://10.1.180.30:3006";

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        s = SocketIo.establishSocketConnection(serverUrl);
        s.connect();
        s.on("connect", (dummy) => { isConnected = true; Debug.Log("Connected to server"); });
        s.on("disconnect", (dummy) => { isConnected = false; Debug.Log("Disconnected from server"); StartCoroutine(Reconnect()); });
        s.on("HELLO", call);
    }

    IEnumerator Reconnect()
    {
        yield return new WaitForSeconds(reconnectDelay);
        if (!isConnected)
        {
            Debug.Log("Attempting to reconnect...");
            ConnectToServer();
        }
    }

    void call(string d)
    {
        Debug.Log($"HELLO received -> {d}");
        s.emit("TEST", "YEAH");
    }

    void OnApplicationQuit()
    {
        if (s != null)
        {
            s.disconnect();
        }
    }
}