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
    private string[] titre_role = {
        "L'Aventurier",
        "L'�lu",
        "Le Tyran",
        "l'Occultiste",
      
    };
    private string[] descriptions_role = {
        "Le r�le de l'aventurier est d'aider l'�quipe � r�ussir la qu�te de l'artefact.",
        "L'�lu est au courant de l'identit� du tyran et de l'occuliste, Par contre , attention � nous pas vous d�voiler, ces dernier peuvent vous assassiner.",
        "Vous devez faire �chouer la qu�te et trouver l'�lu afin de l'assasiner. Un occultiste est cacher mais vous ne savez pas son identit�.",
        "Vous savez qui est le tyran et l'aider � faire �chouer la qu�te.Le tyran ne connait pas votre identit�.",

    };

    void Start()
    {
        UpdateUIRole();
        leftArrowRole.onClick.AddListener(PreviousRole);
        rightArrowRole.onClick.AddListener(NextRole);
    }

    void UpdateUIRole()
    {
        titleTextRole.text = titre_role[currentIndex];
        descriptionTextRole.text = descriptions_role[currentIndex];
    }

    public void NextRole()
    {
        currentIndex = (currentIndex + 1) % titre_role.Length;
        UpdateUIRole();
    }

    public void PreviousRole()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titre_role.Length - 1;
        UpdateUIRole();
    }
}
