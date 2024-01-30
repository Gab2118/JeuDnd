using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Select_role : MonoBehaviour
{
    [System.Serializable]
    public class Role
    {
        public string name;
        public string description;

        public Role(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }

    public TMP_Text textRole;
    private Select_Classe selectClasseScript; // Accéder aux classes et noms
    private Select_chef selectChefScript; // Accéder au nom du chef

    // Bank des rôles avec descriptions
    private Role[] RoleBank = {
        new Role("L'Élu", "Vous êtes l'Élu, votre but est fe),"),
        new Role("Le Tyran", "Description du Tyran..."),
        new Role("L'Occultiste", "Description de L'Occultiste..."),
        new Role("Aventurier", "Description de l'Aventurier..."),
        new Role("Aventurier", "Description de l'Aventurier..."),
        new Role("Aventurier", "Description de l'Aventurier...")
    };
    private List<Role> playerRoles; // Pour stocker les rôles des joueurs

    void Start()
    {
        DontDestroyOnLoad(this);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();
        playerRoles = new List<Role>();

        if (selectClasseScript != null && selectChefScript != null)
        {
            StartCoroutine(AssignRolesAndDisplay());
        }
        else
        {
            Debug.LogError("Required scripts not found!");
        }
    }

    IEnumerator AssignRolesAndDisplay()
    {
        yield return new WaitForSeconds(5);

        List<Role> availableRoles = new List<Role>(RoleBank);
        // Assigner un rôle aléatoirement à chaque joueur
        foreach (string playerName in selectClasseScript.PlayerNames)
        {
            Role playerRole = GetRandomRole(availableRoles);
            playerRoles.Add(playerRole);
        }

        // Modifier ici pour n'afficher que le nom du rôle
        if (textRole != null && playerRoles.Count > 0)
        {
            textRole.text = playerRoles[0].name;
        }

        // Afficher la classe, le rôle de chaque joueur et préciser si il est le chef
        for (int i = 0; i < selectClasseScript.PlayerNames.Count; i++)
        {
            string playerClass = selectClasseScript.PlayerNames[i];
            Role playerRole = playerRoles[i];
            string isChef = selectChefScript.GetNomChef() == playerClass ? " est le chef" : "";

            Debug.Log($"Le joueur {i + 1} est {playerClass}, {playerRole.name} - {playerRole.description}{isChef}");
        }
        ChangerVersSceneGameChef();
    }

    private Role GetRandomRole(List<Role> roles)
    {
        int randomIndex = Random.Range(0, roles.Count);
        Role selectedRole = roles[randomIndex];
        roles.RemoveAt(randomIndex); // S'assurer qu'aucun rôle n'a été donné 2 fois
        return selectedRole;
    }

    private void ChangerVersSceneGameChef()
    {
        Invoke("ChargerSceneGameChef", 5f);
    }

    private void ChargerSceneGameChef()
    {
        SceneManager.LoadScene("scene_game_chef");
    }

    // Méthode publique pour obtenir le rôle d'un joueur par index
    public Role GetPlayerRole(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerRoles.Count)
        {
            return playerRoles[playerIndex];
        }
        else
        {
            Debug.LogError("Index du joueur hors limites!");
            return null;
        }
    }
}
