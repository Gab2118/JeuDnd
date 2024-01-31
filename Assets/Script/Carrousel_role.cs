using UnityEngine;
using UnityEngine.UI;
using TMPro;

// CE SCRIPT EST UTILISER POUR LE CARROUSEL DU GUIDE DES JOUEUR PAR RAPPROT AU ROLE
public class CarouselControllerCarrousel_role : MonoBehaviour
{
    public TextMeshProUGUI titleTextRole;
    public TextMeshProUGUI descriptionTextRole;
    public Button leftArrowRole;
    public Button rightArrowRole;

    private int currentIndex = 0;
    // Les rôle et leurs description
    private string[] titre_role = {
        "L'Aventurier",
        "L'Élu",
        "Le Tyran",
        "l'Occultiste",
      
    };
    private string[] descriptions_role = {
        "Le rôle de l'aventurier est d'aider l'équipe à réussir la quête de l'artefact.",
        "L'élu est au courant de l'identité du tyran et de l'occuliste, Par contre , attention à nous pas vous dévoiler, ces dernier peuvent vous assassiner.",
        "Vous devez faire échouer la quête et trouver l'élu afin de l'assasiner. Un occultiste est cacher mais vous ne savez pas son identité.",
        "Vous savez qui est le tyran et l'aider à faire échouer la quête.Le tyran ne connait pas votre identité.",

    };

    // Écoute de clique des flèches du carrousel
    void Start()
    {
        UpdateUIRole();
        leftArrowRole.onClick.AddListener(PreviousRole);
        rightArrowRole.onClick.AddListener(NextRole);
    }
    // Mettre à jour les informations afficher
    void UpdateUIRole()
    {
        titleTextRole.text = titre_role[currentIndex];
        descriptionTextRole.text = descriptions_role[currentIndex];
    }

    // Aller vers le prochain  role
    public void NextRole()
    {
        currentIndex = (currentIndex + 1) % titre_role.Length;
        UpdateUIRole();
    }

    // Aller au role précédent
    public void PreviousRole()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titre_role.Length - 1;
        UpdateUIRole();
    }
}
