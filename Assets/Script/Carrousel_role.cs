using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CarouselControllerCarrousel_role : MonoBehaviour
{
    public TextMeshProUGUI titleTextRole;
    public TextMeshProUGUI descriptionTextRole;
    public Button leftArrowRole;
    public Button rightArrowRole;

    private int currentIndex = 0;
    private string[] titre_role = {
        "Aventurier",
        "L'Élu",
        "Le tyran",
        "Occultis",
      
    };
    private string[] descriptions_role = {
        "Le rôle de l'aventurier est d'aider l'équipe à réussir la quête. ",
        "L'élu est au courant de qui sont le tyran et l'occuliste, Par contre , attention à nous pas vous dévoiler, ces dernier peuvent vous assassiner.",
        "Vous devez faire échouer la quête et trouver l'élu afin de l'assasiner",
        "Vous savez qui est le tyran et l'aider à faire échouer la quête.",

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
