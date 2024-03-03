using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour
{
    // Rendez cette classe publique pour résoudre l'erreur CS0051
    public class PlayerData
    {
        public string playerName;
        public string playerClass;
        public int avatarIndex; // Index pour l'avatar dans playerAvatars

        public PlayerData(string name, string playerClass, int avatarIndex)
        {
            this.playerName = name;
            this.playerClass = playerClass;
            this.avatarIndex = avatarIndex;
        }
    }

    // Cette liste doit être mise à jour lorsque de nouveaux joueurs rejoignent ou quittent le lobby
    private List<PlayerData> connectedPlayers = new List<PlayerData>();

}

 /*   void Start()
    {
        // Initialise l'UI  UpdateLobbyUI();
}

public void UpdateLobbyUI()
{
   for (int i = 0; i < playerImages.Length; i++)
   {
       if (i < connectedPlayers.Count)
       {
           playerImages[i].gameObject.SetActive(true);
           playerImages[i].sprite = playerAvatars[connectedPlayers[i].avatarIndex];
           playerClassesTexts[i].text = connectedPlayers[i].playerClass;
       }
       else
       {
           playerImages[i].gameObject.SetActive(false);
           playerClassesTexts[i].text = "";
       }
   }
}

// Appelée lorsque de nouveaux joueurs rejoignent ou quittent
public void OnPlayerListUpdated(List<PlayerData> updatedPlayers)
{
   connectedPlayers = updatedPlayers;
   UpdateLobbyUI();
}
}*/