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
    private Select_Classe selectClasseScript; // Acc�der aux classes et noms
    private Select_chef selectChefScript; // Acc�der au nom du chef
    private string classeDuTyran = ""; // Pour stocker la classe du joueur ayant le r�le du Tyran


    // Bank des r�les avec descriptions
    private Role[] RoleBank = {
        new Role("L'�lu", "Vous �tes l'�lu, vous �tes au courant de l'identit� du tyran et de l'occuliste, Par contre , attention � nous pas vous d�voiler, ces dernier peuvent vous assassiner."),
        new Role("Le Tyran", "Vous �tes le Tyran,vous devez faire �chouer la qu�te et trouver l'�lu afin de l'assasiner. Un occultiste vous adorant est cacher mais vous ne savez pas son identit�. Soyez prudent"),
        new Role("L'Occultiste", "Vous �tes l'occultiste, Vous savez qui est le Tyran et devez l'aider � faire �chouer la qu�te. Le Tyran ne connait pas votre identit�. Le tyran est : "),
        new Role("Aventurier", "Vous �tes un aventurier.Le r�le de l'aventurier est d'aider l'�quipe � r�ussir la qu�te de l'artefact. Vous ne savez pas qui est l'�lu ni l'occultiste ni le tyran. Faite votre possible pour aider l'�lu � r�ussir les missions."),
        new Role("Aventurier", "Vous �tes un aventurier.Le r�le de l'aventurier est d'aider l'�quipe � r�ussir la qu�te de l'artefact. Vous ne savez pas qui est l'�lu ni l'occultiste ni le tyran. Faite votre possible pour aider l'�lu � r�ussir les missions."),
        new Role("Aventurier", "Vous �tes un aventurier.Le r�le de l'aventurier est d'aider l'�quipe � r�ussir la qu�te de l'artefact. Vous ne savez pas qui est l'�lu ni l'occultiste ni le tyran. Faite votre possible pour aider l'�lu � r�ussir les missions.")
    };
    private List<Role> playerRoles; // Pour stocker les r�les des joueurs

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
        // Assigner un r�le al�atoirement � chaque joueur
        foreach (string playerName in selectClasseScript.PlayerNames)
        {
            Role playerRole = GetRandomRole(availableRoles);
            if (playerRole.name == "Le Tyran")
            {
                classeDuTyran = playerName; // Ici, assurez-vous que playerName repr�sente bien la classe du joueur
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

        // Modifier ici pour n'afficher que le nom du r�le
        if (textRole != null && playerRoles.Count > 0)
        {
            textRole.text = playerRoles[0].name;
        }

        // Afficher la classe, le r�le de chaque joueur et pr�ciser si il est le chef
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
        roles.RemoveAt(randomIndex); // S'assurer qu'aucun r�le n'a �t� donn� 2 fois
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

    // M�thode publique pour obtenir le r�le d'un joueur par index
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
