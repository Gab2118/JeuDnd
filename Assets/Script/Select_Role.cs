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
    private string classeDuTyran = ""; // Pour stocker la classe du joueur ayant le rôle du Tyran


    // Bank des rôles avec descriptions
    private Role[] RoleBank = {
        new Role("L'Élu", "Vous êtes l'élu, vous êtes au courant de l'identité du tyran et de l'occuliste, Par contre , attention à nous pas vous dévoiler, ces dernier peuvent vous assassiner."),
        new Role("Le Tyran", "Vous êtes le Tyran,vous devez faire échouer la quête et trouver l'élu afin de l'assasiner. Un occultiste vous adorant est cacher mais vous ne savez pas son identité. Soyez prudent"),
        new Role("L'Occultiste", "Vous êtes l'occultiste, Vous savez qui est le Tyran et devez l'aider à faire échouer la quête. Le Tyran ne connait pas votre identité. Le tyran est : "),
        new Role("Aventurier", "Vous êtes un aventurier.Le rôle de l'aventurier est d'aider l'équipe à réussir la quête de l'artefact. Vous ne savez pas qui est l'élu ni l'occultiste ni le tyran. Faite votre possible pour aider l'Élu à réussir les missions."),
        new Role("Aventurier", "Vous êtes un aventurier.Le rôle de l'aventurier est d'aider l'équipe à réussir la quête de l'artefact. Vous ne savez pas qui est l'élu ni l'occultiste ni le tyran. Faite votre possible pour aider l'Élu à réussir les missions."),
        new Role("Aventurier", "Vous êtes un aventurier.Le rôle de l'aventurier est d'aider l'équipe à réussir la quête de l'artefact. Vous ne savez pas qui est l'élu ni l'occultiste ni le tyran. Faite votre possible pour aider l'Élu à réussir les missions.")
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
            if (playerRole.name == "Le Tyran")
            {
                classeDuTyran = playerName; // Ici, assurez-vous que playerName représente bien la classe du joueur
            }
            playerRoles.Add(playerRole);
        }
        foreach (Role role in playerRoles)
        {
            if (role.name == "L'Occultiste")
            {
                role.description += classeDuTyran;
            }
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
