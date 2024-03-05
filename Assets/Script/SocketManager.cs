using UnityEngine;
using KyleDulce.SocketIo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SocketManager : MonoBehaviour
{

    public delegate void OnPlayerInfoUpdated(PlayerInfo playerInfo);
    public static OnPlayerInfoUpdated OnChefAssigned;
    public static OnPlayerInfoUpdated OnRoleAssigned;




    public static SocketManager Instance { get; private set; }
    private readonly string serverUrl = "ws://10.1.180.30:3006";
    private Socket _socket;
    private bool isConnected = false;
    private readonly float reconnectDelay = 5f;
    private string _socketid;
    public string Socketid => _socketid;

    private string _chefid;
    public string Chefid => _chefid;

    private string _roleName;
    public string RoleName => _roleName;

    public Socket Socket
    {
        get { return _socket; }
    }

    public delegate void ChefUpdated();
    public event ChefUpdated OnChefUpdated;


    [System.Serializable]
    public class PlayerInfo
    {
        public string playerId;
        public string playerName;
        public string playerClass;
        public string playerRoleName;
        public string PlayerRoleDescription;
        public bool isChief;
    }

    [System.Serializable]
    public class PlayerData
    {
        public List<PlayerInfo> players;
        public string currentChefId;
    }

    public List<PlayerInfo> playerInfos = new List<PlayerInfo>();
    public string currentChefId;

  


    void Start()
    {
        // Assurez-vous d'avoir une référence à votre Socket correctement initialisée ici
        _socket.on("changeScene", (E) => {
            // Convertir E en une chaîne contenant le nom de la scène
            string sceneName = E.ToString();
            // Utiliser SceneManager pour changer de scène
            SceneManager.LoadScene(sceneName);
        });
    }

    [System.Serializable]
    public class ChefAssignedData
    {
        public string chefId;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ConnectToServer();
            Debug.Log("Awake dans SocketManager");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void ConnectToServer()
    {

        try
        {
            _socket = SocketIo.establishSocketConnection(serverUrl).connect();

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        //_socket.on("test",( msg) => Debug.Log(msg));



        _socket.on("surConnexion", (msg) => {
            _socketid = msg;
            isConnected = true;
            Debug.Log("Connect� au serveur: " + msg);
        });


        /*_socket.on("updateChief", (data) => {
                Debug.Log("Nouveau chef re�u: " + data);
                currentChefId = data.ToString();
                OnChefUpdated?.Invoke();
            });*/

        _socket.on("updatePlayersInfos", (data) =>
        {
            string jsonData = data.ToString();

            Debug.Log("Donn�es re�ues de updatePlayersInfos: " + jsonData);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            playerInfos.Clear();
            foreach (var player in playerData.players)
            {
                playerInfos.Add(new PlayerInfo
                {
                    playerId = player.playerId,
                    playerName = player.playerName,
                    playerClass = player.playerClass,
                    playerRoleName = player.playerRoleName,
                    PlayerRoleDescription = player.PlayerRoleDescription,
                    isChief = player.isChief
                });
            }
        });

        _socket.on("chefAssigned", OnChefAssignedReceived);
        _socket.on("RoleAssigned", OnRoleAssignedReceived);
    }

    void OnRoleAssignedReceived(string data)
    {
        var playerRole = JsonUtility.FromJson<PlayerInfo>(data);
        if (playerRole != null)
        {
            Debug.Log("R�le joueur : " + playerRole.playerRoleName);
            var fullPlayerInfo = playerInfos.Find(p => p.playerId == playerRole.playerId);
            if (fullPlayerInfo != null)
            {
                OnRoleAssigned?.Invoke(fullPlayerInfo);
            }
        }
        else
        {
            Debug.LogError("Pas de role data deserialize");
        }
    }
    public bool IsPlayerChief(string playerId)
    {
        var player = playerInfos.Find(p => p.playerId == playerId);
        return player != null && player.isChief;
    }


    void OnChefAssignedReceived(string data)
    {
        ChefAssignedData chefInfo = JsonUtility.FromJson<ChefAssignedData>(data);
        if (chefInfo != null)
        {
            Debug.Log("Chef ID: " + chefInfo.chefId);
            _chefid = chefInfo.chefId;


            PlayerInfo chefPlayerInfo = playerInfos.Find(player => player.playerId == _chefid);
            if (chefPlayerInfo != null)
            {
                OnChefAssigned?.Invoke(chefPlayerInfo);
            }
            else
            {
                Debug.LogError("Les informations du chef ne sont pas trouv�es.");
            }
        }
        else
        {
            Debug.LogError("Pas de chef data deserialize");
        }
    }
    public PlayerInfo GetCurrentPlayerInfo(string playerId)
    {
        return playerInfos.Find(player => player.playerId == playerId);
    }

    // M�thode appel�e quand une sc�ne est charg�e
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Je regarde la sc�ne");
        if (scene.name == "Scene_Select_classe")
        {
            // sc�ne charg�e est "Scene_Select_classe" informez le serveur
            NotifyServerSceneLoaded(scene.name);
        }
        else if (scene.name == "Scene_choix_chef")
        {
            OnChefAssigned?.Invoke(GetCurrentPlayerInfo(_socketid));
        }
        else if (scene.name == "Scene_choix_role")
        {
            OnRoleAssigned?.Invoke(GetCurrentPlayerInfo(_socketid));
        }
    }

    // indiquer que la sc�ne est charg�e
    private void NotifyServerSceneLoaded(string sceneName)
    {
        if (_socket != null && _socket.connected)
        {
            _socket.emit("sceneLoaded", JsonUtility.ToJson(new { sceneName = sceneName }));
        }
    }

    IEnumerator Reconnect()
    {
        while (!isConnected)
        {
            yield return new WaitForSeconds(reconnectDelay);
            Debug.Log("Tentative de reconnexion...");
            _socket.connect();
        }
    }

    public void SendData(string actionType, object data)
    {
        if (_socket != null && _socket.connected)
        {
            var actionData = new { type = actionType, data = data };
            string jsonData = JsonUtility.ToJson(actionData);
            _socket.emit("gameAction", jsonData);
        }
        else
        {
            Debug.LogError("Tentative d'envoi de donn�es alors que le socket n'est pas connect�.");
        }
    }

    public void UpdateCurrentChef(string newChefId)
    {
        currentChefId = newChefId;
        OnChefUpdated?.Invoke();
    }

    public string GetCurrentChefId()
    {
        return currentChefId;
    }

    public string GetCurrentPlayerClass(int currentPlayerIndex)
    {
        if (playerInfos.Count > currentPlayerIndex)
        {
            return playerInfos[currentPlayerIndex].playerClass;
        }
        else
        {
            Debug.LogError("Index du joueur actuel hors des limites de la liste des infos des joueurs.");
            return "";
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("Socket Off");
    }

    void OnApplicationQuit()
    {
        if (_socket != null)
        {
            _socket.disconnect();
            _socket.disableSocket();
        }
    }

    public void SendPlayerReadyState(bool isReady)
    {
        // Construction d'un simple objet JSON pour l'envoi
        var data = new Dictionary<string, bool> {
        { "isReady", isReady }
    };

        // Convertir en JSON
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);

        // Envoyer au serveur
        // Assurez-vous que votre instance socket est bien connectée et que le nom d'événement correspond à celui attendu côté serveur
        _socket.emit("playerReady", jsonData);
    }
}