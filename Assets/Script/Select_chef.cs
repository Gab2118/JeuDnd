using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// cE SCRIPT À POUR BUT DE SÉLECTIONNER LE CHEF PARMIT LES SIX JOUEUR ET LA FONCTION "indexChefPlus" PERMET DE CHANGER LE CHEF POUR LE SUIVANT
public class Select_chef : MonoBehaviour
{
    public void SetNomChef(string nouveauChef)
    {
        nom_chef = nouveauChef;
        // Mise à jour de l'affichage du chef si nécessaire...
    }
    public TMP_Text textField;
    private List<string> banqueChef; // liste des chef trouver 

    private string nom_chef; // stocker le role qui sera le chef
    private int index_chef = -1;

    // Start est appelé avant la première mise à jour de frame
    void Start()
    {

        // Trouve l'instance de Select_Classe et récupère la liste des noms des classe
        Select_Classe Select_Classe = FindObjectOfType<Select_Classe>();
        if (Select_Classe != null)
        {
            banqueChef = new List<string>(Select_Classe.PlayerNames);
            foreach (string nom in banqueChef)
            {
                Debug.Log("Nom du joueur dans banqueChef: " + nom);
            }
        }
        else
        {
            Debug.LogError("Select_Classe not found!");
        }
        DontDestroyOnLoad(this);
        StartCoroutine(ChangeTextPeriodically());
    }

    // Coroutine pour défiler le texte de chaque mot parmit la liste des classes tirer
    IEnumerator ChangeTextPeriodically()
    {
        int iterations = 0;
        while (iterations < 5 / 0.05f) // le faire pendant 5 seconde
        {
            if (banqueChef != null && banqueChef.Count > 0)
            {
                index_chef = Random.Range(0, banqueChef.Count);
                nom_chef = banqueChef[index_chef]; // Tirer un mot de la banque
                textField.text = nom_chef; // Assigner nom_chef au mot tirer

            }
            else
            {
                Debug.LogError("banqueChef is null or empty!");
            }

            yield return new WaitForSeconds(0.05f); // Attend 0.05 seconde avant de continuer
            iterations++;
        }

        AfficherChef();
    }


    void AfficherChef()
    {
        Debug.Log("Le chef est : " + nom_chef);
        ChangerVersSceneChoixChef();
    }


    // changer pour le prochain chef
    public void indexChefPlus()
    {
        if (banqueChef != null && banqueChef.Count > 0)
        {
            // Incrémente index_chef, mais assurez-vous qu'il reste dans les limites de la liste
            index_chef = (index_chef + 1) % banqueChef.Count;

            // Mettre à jour nom_chef avec le nouveau chef
            nom_chef = banqueChef[index_chef];

            // Afficher le nouveau chef dans la console
            Debug.Log("Le nouveau chef est : " + nom_chef);
        }
        else
        {
            Debug.LogError("banqueChef is null or empty!");
        }
    }

    public string GetNomChef()
    {
        return nom_chef;
    }

    private void ChangerVersSceneChoixChef()
    {
        Invoke("ChargerSceneChoixRole", 5f);
    }
    private void ChargerSceneChoixRole()
    {


        SceneManager.LoadScene("Scene_choix_role");
    }


}