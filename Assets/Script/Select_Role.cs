using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// CE SCRIPT � POUR BUT DE SELECTIONNER LES ROLE POUR CHAQUE JOUEUR ET DE LE GARDER EN M�MOIRE
public class Select_role : MonoBehaviour
{

    public TMP_Text textRole; 
    private Select_Classe selectClasseScript; // acc�der au classe et nom ( joueur_un,joueur_deux,etc)
    private Select_chef selectChefScript; // To access the chef name

    private string[] RoleBank = { "L'�lu", "Le Tyran", "L'Occultiste", "Aventurier", "Aventurier", "Aventurier" };
    private List<string> playerRoles; // To store the roles of the players

    void Start()
    {
        DontDestroyOnLoad(this);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();
        playerRoles = new List<string>();

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

        List<string> availableRoles = new List<string>(RoleBank);
        // assigner un role al�atoirement � chaque joueur
        foreach (string playerName in selectClasseScript.PlayerNames)
        {
            string playerRole = GetRandomRole(availableRoles);
            playerRoles.Add(playerRole);
        }

      
        if (textRole != null && playerRoles.Count > 0)
        {
            textRole.text = playerRoles[0];
        }

        // afficher la classe, le role de chaque joueur et pr�ciser si il est le chef
        for (int i = 0; i < selectClasseScript.PlayerNames.Count; i++)
        {
            string playerClass = selectClasseScript.PlayerNames[i];
            string playerRole = playerRoles[i];
            string isChef = selectChefScript.GetNomChef() == playerClass ? " est le chef" : "";

            Debug.Log($"Le joueur {i + 1} est {playerClass} + {playerRole} + {isChef}");

        }
        ChangerVersSceneGameChef();
    }
  

    private string GetRandomRole(List<string> roles)
    {
        int randomIndex = Random.Range(0, roles.Count);
        string selectedRole = roles[randomIndex];
        roles.RemoveAt(randomIndex); //verifier que aucun role a �t� donner 2 fois
        return selectedRole;
    }

    private void ChangerVersSceneGameChef()
    {
        Invoke("ChargerSceneGameChef", 5f);
    }
    private void ChargerSceneGameChef()
    {


        SceneManager.LoadScene("Scene_turn_TEST");
    }

}
