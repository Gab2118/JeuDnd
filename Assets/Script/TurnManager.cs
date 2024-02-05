using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{

    // Pour la gestion des couleurs des boutons
    public Button[] choiceButtons; // Assurez-vous d'assigner vos boutons dans l'inspecteur Unity
    private Color32 defaultButtonColor = new Color32(190, 190, 190, 255); // Couleur initiale des boutons
    private Color32 selectedButtonColor = new Color32(255, 0, 0, 255); // Couleur lorsqu'un bouton est s�lectionn�
    private Button selectedButton;
    private int selectedButtonCount = 0;
    private List<Button> selectedButtons = new List<Button>();
    public TMP_Text text_confirm;
    public Button btn_confirm_choix;


    public GameObject panelCompetenceChoixJoueur;

    public Button btnChoixChef;



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

    // panel comp�tence dans scene_game_chef
    public GameObject panelCompetence;
    public Button yesButton;
    public Button noButton;
    public TMP_Text skillNameText;

    // panel_jeu_principal
    public TMP_Text roleDescriptionText;
    public TMP_Text turnText;
    public TMP_Text skillDescriptionText;
    public Image classImageDisplay;
    public Button nextTurnButton;

    // r�cup�ration des donn�es des autres script 
    private Select_role selectRoleScript;
    private Select_Classe selectClasseScript;
    private Select_chef selectChefScript;

    // autre divers
    public List<ClassImage> classImages; // Liste des paires classe-image (voir l'inspecteur)
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la comp�tence a �t� utilis�e pour chaque classe
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les comp�tences des classes

    private int completeTurnCount = 0; // Pour compter le nombre de tours complets

  







    void Start()
    {
        selectChefScript = FindObjectOfType<Select_chef>();
        if (selectChefScript == null)
        {
            Debug.LogError("Select_chef script not found!");
        }
        selectRoleScript = FindObjectOfType<Select_role>();
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // Obtenez le composant TextMeshProUGUI du bouton et d�finissez-le au nom de classe du joueur
            TextMeshProUGUI buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = selectClasseScript.PlayerNames[i];
        }

        btn_confirm_choix.interactable = false;
        foreach (var button in choiceButtons)
        {
            button.onClick.AddListener(() => OnChoiceButtonClicked(button));
        }


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
        btn_confirm_choix.onClick.AddListener(OnConfirmButtonClicked);
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

    // banque de donn�e des classes reli� � leurs comp�tences et leurs descriptions
    private void InitializeSkillBank()
    {
        skillBank.Add("Guerrier", new Skill { skillName = "Leadership", description = "Votre comp�tences vous permet de rejoindre un groupe de mission et ainsi d'y aller � trois. La majorit� l'emporte." });
        skillBank.Add("Barbare", new Skill { skillName = "Rage int�grante", description = "On ne sait pas encore ce qu'il peut faire." });
        skillBank.Add("Sorci�re", new Skill { skillName = "Vision secr�te", description = "Votre comp�tence vous permet d'apercevoir le choix de quelqu'un de la mission actuel." });
        skillBank.Add("D�moniste", new Skill { skillName = "Mal�diction sur la vie", description = "Votre comp�tence vous permet de lancer une mal�diction sur un joueur, il ne pourra pas participer" });
        skillBank.Add("Barde", new Skill { skillName = "Musique de la m�moire", description = "Annule le r�sultat d'une mission." });
        skillBank.Add("Clerc", new Skill { skillName = "D�mocratie pur", description = "Donner une b�n�diction pour changer le chef de camp du jour m�me." });
        skillBank.Add("Moine", new Skill { skillName = "Relaxation", description = "Am�ne une personne m�diter avec toi, ce dernier ne peut participer � la mission du jour." });
        skillBank.Add("N�cromancien", new Skill { skillName = "Zombieland", description = "Lance un d� 20 (1 jusqu'� 10 il perd le contr�le des zombies et ceux-ci donnent une carte �chec) (11 jusqu'� 20 garde le contr�le et le contraire se produit)." });
        skillBank.Add("R�deur", new Skill { skillName = "Chasse de loup", description = "On ne sait pas encore." });
        skillBank.Add("Assassin", new Skill { skillName = "La discretion", description = "Emp�che le joueur d'activer sa comp�tence." });
        skillBank.Add("Paladin", new Skill { skillName = "Jet de lumi�re", description = "En �change de d�voiler son r�le, annule le r�sultat d'une manche. ON NE REFAIT PAS CETTE MANCHE." });
    }


    // Le chef commence en premier
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

    // v�rifier si tout les tour sont fait
    private void OnNextTurnButtonClicked()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % selectClasseScript.PlayerNames.Count;
        turnCounter++;

        // Si tous les joueurs ont jou� une fois, c'est la fin d'un tour complet
        if (turnCounter >= selectClasseScript.PlayerNames.Count)
        {
            turnCounter = 0; // R�initialiser le compteur de tours pour le prochain tour
            completeTurnCount++; // Incr�menter le compteur de tours complets
            Debug.Log($"Tour {completeTurnCount} fini"); // Afficher le message dans la console

            // D�terminer et mettre � jour le prochain chef
            int currentChefIndex = selectClasseScript.PlayerNames.IndexOf(selectChefScript.GetNomChef());
            int nextChefIndex = (currentChefIndex + 1) % selectClasseScript.PlayerNames.Count;
            selectChefScript.SetNomChef(selectClasseScript.PlayerNames[nextChefIndex]);

            // R�initialiser currentPlayerIndex pour que le chef soit le premier joueur du prochain tour
            currentPlayerIndex = nextChefIndex;

            UpdateTurnTextAndImage(); // Mettre � jour l'affichage pour le prochain tour
        }
        else
        {
            UpdateTurnTextAndImage();
        }
    }



    // changer les informations � chaque tour
    private void UpdateTurnTextAndImage()
    {
        if (selectClasseScript.PlayerNames.Count > currentPlayerIndex)
        {
            string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
            string colorHex = ColorUtility.ToHtmlStringRGB(classColors.ContainsKey(currentPlayerClass) ? classColors[currentPlayerClass] : Color.white);
            turnText.text = $"<color=#{colorHex}>{currentPlayerClass}</color>";

            if (classImageDictionary.ContainsKey(currentPlayerClass))
            {
                classImageDisplay.sprite = classImageDictionary[currentPlayerClass];
            }
            else
            {
                Debug.LogWarning($"Aucune image trouv�e pour la classe {currentPlayerClass}");
            }

            // Mettre � jour le texte de la comp�tence et sa description
            if (skillBank.ContainsKey(currentPlayerClass))
            {
                skillNameText.text = skillBank[currentPlayerClass].skillName;
                skillDescriptionText.text = skillBank[currentPlayerClass].description;

                // Afficher le panel de comp�tence seulement si la comp�tence n'a pas encore �t� utilis�e
                if (!skillUsed.ContainsKey(currentPlayerClass) || !skillUsed[currentPlayerClass])
                {
                    panelCompetence.SetActive(true);
                }
                else
                {
                    panelCompetence.SetActive(false);
                }
            }
            else
            {
                skillNameText.text = "Comp�tence inconnue";
                skillDescriptionText.text = ""; // R�initialiser le texte de la description de la comp�tence
                panelCompetence.SetActive(false);
            }

            // R�cup�rer la description du r�le du joueur actuel et la mettre � jour
            Select_role.Role currentPlayerRole = selectRoleScript.GetPlayerRole(currentPlayerIndex);
            if (currentPlayerRole != null)
            {
                roleDescriptionText.text = currentPlayerRole.description;
            }
            else
            {
                roleDescriptionText.text = "Description du r�le inconnue";
            }

            // R�cup�rer le nom du joueur actuel et activer le bouton Btn_choix_chef si le joueur actuel est le chef
            string currentPlayerName = selectClasseScript.PlayerNames[currentPlayerIndex];
            btnChoixChef.interactable = (currentPlayerName == selectChefScript.GetNomChef());
        }
        else
        {
            Debug.LogError("Player index is out of range!");
        }
    }



    // charger la sc�ne suivante
    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_vide");
    }

    // si le joueur accepte d'utilis� sa comp�tence
    private void OnYesButtonClicked()
    {
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
        Debug.Log(currentPlayerClass + " a utilis� sa comp�tence.");
        skillUsed[currentPlayerClass] = true; // Mettre � jour le statut de la comp�tence comme utilis�e
        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences

        // Si l'assassin accepte t'utiliser sa comp�tence
        if (currentPlayerClass == "Assassin")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        if (currentPlayerClass == "Sorci�re")
        {

            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        if (currentPlayerClass == " D�moniste")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        if (currentPlayerClass == "Clerc")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        if (currentPlayerClass == "Moine")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
    }



    private void OnNoButtonClicked()
    {
        Debug.Log("Comp�tence non utilis�e.");
        skillUsed[selectClasseScript.PlayerNames[currentPlayerIndex]] = false; // Mettre � jour le statut de la comp�tence comme non utilis�e
        panelCompetence.SetActive(false); // Fermer le panneau de comp�tences
    }
    private void OnChoiceButtonClicked(Button clickedButton)
    {
        if (selectedButtons.Contains(clickedButton))
        {
            // D�s�lectionner le bouton
            clickedButton.image.color = defaultButtonColor;
            selectedButtons.Remove(clickedButton);
            selectedButtonCount--;
        }
        else if (selectedButtonCount < 2) // Assurez-vous de ne pas s�lectionner plus de 2 boutons
        {
            // S�lectionner le bouton
            clickedButton.image.color = selectedButtonColor;
            selectedButtons.Add(clickedButton);
            selectedButtonCount++;
        }

        // Mettre � jour le texte confirm
        text_confirm.text = $"{selectedButtonCount}/1";

        // Activer ou d�sactiver le bouton de confirmation
        btn_confirm_choix.interactable = (selectedButtonCount == 1);

        // R�activer ou d�sactiver les autres boutons si n�cessaire
        foreach (var button in choiceButtons)
        {
            if (!selectedButtons.Contains(button) && selectedButtonCount < 1)
            {
                button.interactable = true; // R�activer le bouton
            }
            else if (!selectedButtons.Contains(button) && selectedButtonCount == 1)
            {
                button.interactable = false; // D�sactiver le bouton
            }
        }
    }
    private void UpdateConfirmButtonState()
    {
        text_confirm.text = $"{selectedButtonCount}/1";
        btn_confirm_choix.interactable = (text_confirm.text == "1/1");
    }

    private void OnConfirmButtonClicked()
    {
        // r�cup�rer le tour du joueur en cour
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];

        // si tel classes clique sur btn_confirm_choice
        switch (currentPlayerClass)
        {
            case "Sorci�re":
                // Implement the logic for Sorci�re here
                Debug.Log("La sorci�re aper�oit le choix de quelqu'un");
                break;

            case "D�moniste":
                // Implement the logic for D�moniste here
                Debug.Log("Le d�moniste � maudit un joueur");
                break;
            case "Clerc":
                // Implement the logic for Clerc here
                Debug.Log("La clerc � nomm� un nouveau chef");
                break;

            case "Moine":
                // Implement the logic for Moine here
                Debug.Log("Le moine � amener un joueur � m�diter");
                break;

            case "Assassin":
                // Implement the logic for Assassin here
                Debug.Log("L'assassin vole la comp�tence d'un joueur");
                break;

 
                // si aucune classe en cour ne fait partie de cette liste 
            default:
                Debug.LogWarning($"Unknown class: {currentPlayerClass}");
                break;
        }
    }


}


