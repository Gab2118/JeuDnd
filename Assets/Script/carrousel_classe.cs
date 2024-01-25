using UnityEngine;
using UnityEngine.UI;
using TMPro; 


// CE SCRIPT EST UTILISER POUR LE CARROUSEL DU GUIDE DES JOUEUR PAR RAPPROT AU CLASSE

public class CarouselController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button leftArrow;
    public Button rightArrow;

    private int currentIndex = 0;
    private string[] titles = {
        "Guerrier",
        "Barbare",
        "Sorci�re",
        "D�moniste",
        "Barde",
        "Clerc",
        "Moine",
        "N�cromancien",
        "R�deur",
        "Assassin",
        "Paladin"
    };
    private string[] descriptions = {
        "Il peut rejoindre un groupe de mission et ainsi choisir de faire r�ussir ou �chouer (deux minimum).",
        "On ne sait pas encore.",
        "Peut apercevoir le choix de quelqu'un.",
        "Lance une mal�diction qui emp�che un joueur de partir � la prochaine mission.",
        "Annule le r�sultat d'une mission.",
        "Donner une b�n�diction pour changer le chef de camp du jour m�me.",
        "Am�ne une personne m�diter avec toi, ce dernier ne peut participer � la mission du jour.",
        "Lance un d� 20 (1 jusqu'� 10 il perd le contr�le des zombies et ceux-ci donnent une carte �chec, 11 jusqu'� 20 garde le contraire).",
        "On ne sait pas encore.",
        "Emp�che le joueur d'activer sa comp�tence.",
        "En �change de r�v�ler votre identit�, annuler le r�sultat d'une manche de votre choix et cette manche ne sera pas rejouer."
    };

    void Start()
    {
        UpdateUI();
        leftArrow.onClick.AddListener(PreviousClass);
        rightArrow.onClick.AddListener(NextClass);
    }

    void UpdateUI()
    {
        titleText.text = titles[currentIndex];
        descriptionText.text = descriptions[currentIndex];
    }

    public void NextClass()
    {
        currentIndex = (currentIndex + 1) % titles.Length;
        UpdateUI();
    }

    public void PreviousClass()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = titles.Length - 1;
        UpdateUI();
    }
}
