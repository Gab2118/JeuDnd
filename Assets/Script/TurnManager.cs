using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string description;
    }

    [System.Serializable]
    public class ClassImage
    {
        public string className;
        public Sprite classSprite;
    }
    public Button yesButton;
    public Button noButton;
    public TMP_Text turnText;
    public TMP_Text skillNameText; // Texte TMP pour afficher le nom de la comp�tence
    public Button nextTurnButton;
    public Image classImageDisplay;
    public GameObject panelCompetence; // R�f�rence au panneau de comp�tences
    public List<ClassImage> classImages; // Liste des paires classe-image d�finies dans l'inspecteur
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la comp�tence a �t� utilis�e pour chaque classe

    private Select_Classe selectClasseScript;
    private Select_chef selectChefScript;
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les comp�tences des classes

    void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();

        if (selectClasseScript == null || selectChefScript == null || classImageDisplay == null || panelCompetence == null || skillNameText == null)
        {
            Debug.LogError("Required components or scripts not found in the scene!");
            return;
        }

        InitializeClassColors();
        InitializeClassImagesDictionary();
        InitializeSkillBank(); // Initialiser la banque de comp�tences
        SetStartingPlayerAsChef();
        nextTurnButton.onClick.AddListener(OnNextTurnButtonClicked);
        UpdateTurnTextAndImage();
    }


    private void InitializeClassColors()
    {
        classColors.Add("Guerrier", Color.red);
        classColors.Add("Barbare", Color.blue);
        classColors.Add("Sorci�re", Color.magenta);
        classColors.Add("D�moniste", Color.black);
        classColors.Add("Barde", Color.green);
        classColors.Add("Clerc", Color.yellow);
        classColors.Add("Moine", Color.gray);
        classColors.Add("N�cromancien", Color.cyan);
        classColors.Add("R�deur", Color.white);
        classColors.Add("Assassin", new Color(1f, 0.5f, 0)); // Orange
        classColors.Add("Paladin", Color.white);
    }

    private void InitializeClassImagesDictionary()
    {
        foreach (ClassImage classImage in classImages)
        {
            if (!classImageDictionary.ContainsKey(classImage.className))
            {
                classImageDictionary.Add(classImage.className, classImage.classSprite);
            }
        }
    }

    private void InitializeSkillBank()
    {
        skillBank.Add("Guerrier", new Skill { skillName = "Leadership", description = "Il peut rejoindre un groupe de mission et ainsi choisir de faire r�ussir ou �chouer la mission." });
        skillBank.Add("Barbare", new Skill { skillName = "Rage int�grante", description = "On ne sait pas encore ce qu'il peut faire." });
        skillBank.Add("Sorci�re", new Skill { skillName = "Vision secr�te", description = "Peut apercevoir le choix de quelqu'un." });
        skillBank.Add("D�moniste", new Skill { skillName = "Mal�diction sur la vie", description = "Lance une mal�diction qui emp�che un joueur de partir � la prochaine mission." });
        skillBank.Add("Barde", new Skill { skillName = "Musique de la m�moire", description = "Annule le r�sultat d'une mission." });
        skillBank.Add("Clerc", new Skill { skillName = "D�mocratie pur", description = "Donner une b�n�diction pour changer le chef de camp du jour m�me." });
        skillBank.Add("Moine", new Skill { skillName = "Relaxation", description = "Am�ne une personne m�diter avec toi, ce dernier ne peut participer � la mission du jour." });
        skillBank.Add("N�cromancien", new Skill { skillName = "Zombieland", description = "Lance un d� 20 (1 jusqu'� 10 il perd le contr�le des zombies et ceux-ci donnent une carte �chec) (11 jusqu'� 20 garde le contr�le et le contraire se produit)." });
        skillBank.Add("R�deur", new Skill { skillName = "Chasse de loup", description = "On ne sait pas encore." });
        skillBank.Add("Assassin", new Skill { skillName = "La discretion", description = "Emp�che le joueur d'activer sa comp�tence." });
        skillBank.Add("Paladin", new Skill { skillName = "Jet de lumi�re", description = "En �change de d�voiler son r�le, annule le r�sultat d'une manche. ON NE REFAIT PAS CETTE MANCHE." });
    }

    private void SetStartingPlayerAsChef()
    {
        string chefName = selectChefScript.GetNomChef();
        currentPlayerIndex = selectClasseScript.PlayerNames.IndexOf(chefName);
        if (currentPlayerIndex == -1)
        {
            Debug.LogError("Le chef n'a pas �t� trouv� dans la liste des joueurs !");
            currentPlayerIndex = 0;
        }
    }

    private void OnNextTurnButtonClicked()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % selectClasseScript.PlayerNames.Count;
        UpdateTurnTextAndImage();
        turnCounter++;

        if (turnCounter >= selectClasseScript.PlayerNames.Count)
        {
            LoadEmptyScene();
        }
    }

    private void UpdateTurnTextAndImage()
    {
        if (selectClasseScript.PlayerNames.Count > currentPlayerIndex)
        {
            string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
            string colorHex = ColorUtility.ToHtmlStringRGB(classColors.ContainsKey(currentPlayerClass) ? classColors[currentPlayerClass] : Color.white);
            turnText.text = $"C'est le tour du <color=#{colorHex}>{currentPlayerClass}</color>";

            if (classImageDictionary.ContainsKey(currentPlayerClass))
            {
                classImageDisplay.sprite = classImageDictionary[currentPlayerClass];
            }
            else
            {
                Debug.LogWarning($"Aucune image trouv�e pour la classe {currentPlayerClass}");
            }

            // V�rifier si la comp�tence a d�j� �t� utilis�e pour cette classe
            if (!skillUsed.ContainsKey(currentPlayerClass) || !skillUsed[currentPlayerClass])
            {
                // Si la comp�tence n'a pas �t� utilis�e, afficher le panneau
                panelCompetence.SetActive(true);
                if (skillBank.ContainsKey(currentPlayerClass))
                {
                    skillNameText.text = skillBank[currentPlayerClass].skillName;
                }
                else
                {
                    skillNameText.text = "Comp�tence inconnue";
                }
            }
            else
            {
                // Si la comp�tence a �t� utilis�e, ne pas afficher le panneau
                panelCompetence.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Player index is out of range!");
        }
    }

        
    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_vide");
    }
    private void OnYesButtonClicked()
    {
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
        Debug.Log(currentPlayerClass + " a utilis� sa comp�tence.");
        skillUsed[currentPlayerClass] = true; // Mettre � jour le statut de la comp�tence comme utilis�e
        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences
    }

    private void OnNoButtonClicked()
    {
        Debug.Log("Comp�tence non utilis�e.");
        skillUsed[selectClasseScript.PlayerNames[currentPlayerIndex]] = false; // Mettre � jour le statut de la comp�tence comme non utilis�e
        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences
    }
}
