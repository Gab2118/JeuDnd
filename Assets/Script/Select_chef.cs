using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Select_chef : MonoBehaviour
{
    public TMP_Text textField;
    private List<string> banqueChef; // liste des chef trouver 

    private string nom_chef; // stocker le role qui sera le chef
    private int index_chef = -1;

    // Start est appelé avant la première mise à jour de frame
    void Start()
    {

        // Trouve l'instance de RandomWordDisplay et récupère la liste des noms des classe
        RandomWordDisplay randomWordDisplay = FindObjectOfType<RandomWordDisplay>();
        if (randomWordDisplay != null)
        {
            banqueChef = new List<string>(randomWordDisplay.PlayerNames);
            foreach (string nom in banqueChef)
            {
                Debug.Log("Nom du joueur dans banqueChef: " + nom);
            }
        }
        else
        {
            Debug.LogError("RandomWordDisplay not found!");
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
    }

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

}
