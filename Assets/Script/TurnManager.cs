using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    // page de jeu principal ( panel_jeu_principal
    public Button btnChoixChef;
    public Button btnUtiliseCompetence; // bouton pour utilisé sa compétence
    private Dictionary<string, bool> competenceUtiliseePourClasse = new Dictionary<string, bool>();
    public TMP_Text roleDescriptionText; // description du role d'un joueur
    public TMP_Text turnText; // texte pou afficher la classe du joueur
    public TMP_Text skillDescriptionText; // description de la compétence
    public Image classImageDisplay; // image utilisé pour afficher l'image de chaque classe (les sprites des classes)
    public Button nextTurnButton; // bouton prochain tour

    // Pour la gestion des couleurs des boutons de choix pour cible et ce qui à rapport avec le Panel_competence_choix_joueur
    public GameObject panelCompetenceChoixJoueur;
    public Button[] choiceButtons; // les six boutons pour choisir une cible aprés avoir accepter d'utiliser sa compétence
    private Color32 defaultButtonColor = new Color32(190, 190, 190, 255); // Couleur initiale des boutons (parmit les 6)
    private Color32 selectedButtonColor = new Color32(255, 0, 0, 255); // Couleur lorsqu'un bouton est sélectionné
    private int selectedButtonCount = 0; // nombre de bouton sélectionner
    private List<Button> selectedButtons = new List<Button>(); // list des bouton sélectionner
    public Button btn_confirm_choix; // bouton de confirmation pour la cible du panel : Panel_competence_choix_joueur
    public TMP_Text text_confirm; // texte du bouton afin de afficher le nombre de sélectionner (0/1 ou 1/1)

    // pour contenir les informations de chaque compétence pour chaque classe
    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string description;
    }

    // contenir les sprites par rapport à chaque classes
    [System.Serializable]
    public class ClassImage
    {
        public string className;
        public Sprite classSprite;
    }

    // panel compétence afin de permettre au joueur de choisir d'utiliser ou non sa compétence
    public GameObject panelCompetence; // Le panel panel_competence
    public Button yesButton; // accepter d'utiliser sa compétence
    public Button noButton; // refuser d'utiliser sa compétence
    public TMP_Text skillNameText;

   // récupération des données des autres script 
    private Select_role selectRoleScript; // script pour la distribution des rôle
    private Select_Classe selectClasseScript; // script pour la sélection des classes
    private Select_chef selectChefScript;

    // autre divers
    public List<ClassImage> classImages; // Liste des paires classe-image (voir l'inspecteur)
    private Dictionary<string, bool> skillUsed = new Dictionary<string, bool>(); // Pour stocker si la compétence a été utilisée pour chaque classe
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes
    private Dictionary<string, Skill> skillBank = new Dictionary<string, Skill>(); // Dictionnaire pour stocker les compétences des classes

    private int completeTurnCount = 0; // Pour compter le nombre de tours complets

    // déclaration pour les compétence coté assassin
    private string classeCibleAssassin; // stock le nom de la classe cibler
    private bool isAssassinSkillUsedThisTurn = false; // vérifie si la compétence de l'assassin a été utilisé
    private string targetClassForTextChange = null;// stocke le nom de la classe pour le texte du bouton d'utilisation de la compétence doit être modifié 
    private int turnsSinceAssassinSkillUsed = 0; // compte le nombre de tours écoulés depuis que la compétence de l'assassin a été utilisée.
    private int TourDepuisAssassina = 0; // enregistre le tour pendant lequel la compétence de l'assassin a été utilisée pour la dernière fois.
    // clerc 
    private string classeCibleClerc;
    // Démoniste
    private string classeCibleDemoniste;





    void Start()
    {
        selectChefScript = FindObjectOfType<Select_chef>(); // recherche dans la scene le script select_chef
        selectRoleScript = FindObjectOfType<Select_role>();// recherche dans la scene le script select_role
        selectClasseScript = FindObjectOfType<Select_Classe>(); // recherche dans la scene le script select_classe

        // void start du panel_competence
        yesButton.onClick.AddListener(OnYesButtonClicked); // écouteur de clique du bouton oui
        noButton.onClick.AddListener(OnNoButtonClicked);// écouteur de clique du bouton non


        // void start du Panel_competence_choix_joueur
        btn_confirm_choix.onClick.AddListener(OnConfirmButtonClicked);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // aller chercher les text de chaque bouton et y placer le nom de classe ( 1 bouton pour une classe donc 6 en tout)
            TextMeshProUGUI buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            // et le changer par le nom de classe
            buttonText.text = selectClasseScript.PlayerNames[i];
        }

        btn_confirm_choix.interactable = false; // logique de false ou true pour activer ou désactiver pour le bouton de confirmation du choix de la cible
        // pour chaque des six bouton lance une fonctione
        foreach (var button in choiceButtons)
        {
            // lancer la fonction
            button.onClick.AddListener(() => OnChoiceButtonClicked(button));
        }
        // void start page jeu principal
        InitializeClassColors(); // définie les couleurs de chaque clase de personnage ( couleur de nom)
        InitializeClassImagesDictionary(); //fonctione pour associe une image pour chaque classe
        InitializeSkillBank(); // fonction pour Initialiser la banque de compétences
        SetStartingPlayerAsChef(); // fonction faire débuter le chef en premier
        nextTurnButton.onClick.AddListener(OnNextTurnButtonClicked); // écouteur de clique pour le bouton tour suivant
        UpdateTurnTextAndImage(); // mettre à jour les text et images
      
    }


    // initialiser les couleur pour chaque classe
    private void InitializeClassColors()
    {
        classColors.Add("Guerrier", Color.red);
        classColors.Add("Barbare", Color.blue);
        classColors.Add("Sorcière", Color.magenta);
        classColors.Add("Démoniste", Color.black);
        classColors.Add("Barde", Color.green);
        classColors.Add("Clerc", Color.yellow);
        classColors.Add("Moine", Color.gray);
        classColors.Add("Nécromancien", Color.cyan);
        classColors.Add("Rôdeur", Color.white);
        classColors.Add("Assassin", new Color(1f, 0.5f, 0)); // Orange
        classColors.Add("Paladin", Color.white);
    }
    //fonction pour associe une image pour chaque classe
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

    // banque de donnée des classes relié à leurs compétences et leurs descriptions
    private void InitializeSkillBank()
    {
        skillBank.Add("Guerrier", new Skill { skillName = "Leadership", description = "Votre compétences vous permet de rejoindre un groupe de mission et ainsi d'y aller à trois. La majorité l'emporte." });
        skillBank.Add("Barbare", new Skill { skillName = "Rage intégrante", description = "On ne sait pas encore ce qu'il peut faire." });
        skillBank.Add("Sorcière", new Skill { skillName = "Vision secrète", description = "Votre compétence vous permet d'apercevoir le choix de quelqu'un de la mission actuel." });
        skillBank.Add("Démoniste", new Skill { skillName = "Malédiction sur la vie", description = "Votre compétence vous permet de lancer une malédiction sur un joueur, il ne pourra pas participer" });
        skillBank.Add("Barde", new Skill { skillName = "Musique de la mémoire", description = "Annule le résultat d'une mission." });
        skillBank.Add("Clerc", new Skill { skillName = "Démocratie pur", description = "Donner une bénédiction pour changer le chef de camp du jour même." });
        skillBank.Add("Moine", new Skill { skillName = "Relaxation", description = "Amène une personne méditer avec toi, ce dernier ne peut participer à la mission du jour." });
        skillBank.Add("Nécromancien", new Skill { skillName = "Zombieland", description = "Lance un dé 20 (1 jusqu'à 10 il perd le contrôle des zombies et ceux-ci donnent une carte échec) (11 jusqu'à 20 garde le contrôle et le contraire se produit)." });
        skillBank.Add("Rôdeur", new Skill { skillName = "Chasse de loup", description = "On ne sait pas encore." });
        skillBank.Add("Assassin", new Skill { skillName = "La discretion", description = "Empêche le joueur d'activer sa compétence." });
        skillBank.Add("Paladin", new Skill { skillName = "Jet de lumière", description = "En échange de dévoiler son rôle, annule le résultat d'une manche. ON NE REFAIT PAS CETTE MANCHE." });
    }


    // fonction pour que le chef commence en premier
    private void SetStartingPlayerAsChef()
    {
        string chefName = selectChefScript.GetNomChef();
        currentPlayerIndex = selectClasseScript.PlayerNames.IndexOf(chefName);
    }

    //fonction à chaque fois que on clique sur tour suivant
    private void OnNextTurnButtonClicked()
    {
        // Incrementer l'index du joueur actuel et le compteur de tour
        currentPlayerIndex = (currentPlayerIndex + 1) % selectClasseScript.PlayerNames.Count;
        turnCounter++;

        // Verifier si tous les joueurs ont joué une fois
        if (turnCounter >= selectClasseScript.PlayerNames.Count)
        {
            turnCounter = 0; // Réinitialiser le compteur de tours pour le prochain tour
            completeTurnCount++; // Incrementer le compteur de tours complets

            // Vérifier si l'assassin a utilisé sa compétence ce tour
            if (isAssassinSkillUsedThisTurn)
            {
                DisableTargetClassCompetenceButton(classeCibleAssassin);
                isAssassinSkillUsedThisTurn = false;
            }

            // Si le Clerc a désigné un nouveau chef, faire de cette cible le chef
            if (!string.IsNullOrEmpty(classeCibleClerc))
            {
                // Trouver l'index du nouveau chef parmi les joueurs
                int newChefIndex = selectClasseScript.PlayerNames.IndexOf(classeCibleClerc);
                if (newChefIndex != -1)
                {
                    selectChefScript.SetNomChef(classeCibleClerc);
                    currentPlayerIndex = newChefIndex; // Faire commencer le nouveau chef
                }
                classeCibleClerc = null; // Réinitialiser la cible du Clerc
            }
            else
            {
                // Déterminer et mettre à jour le prochain chef de manière habituelle si le Clerc n'a pas choisi de cible
                int currentChefIndex = selectClasseScript.PlayerNames.IndexOf(selectChefScript.GetNomChef());
                int nextChefIndex = (currentChefIndex + 1) % selectClasseScript.PlayerNames.Count;
                selectChefScript.SetNomChef(selectClasseScript.PlayerNames[nextChefIndex]);
                currentPlayerIndex = nextChefIndex;
            }

            // Réinitialiser le compteur de tours depuis l'utilisation de la compétence de l'assassin
            turnsSinceAssassinSkillUsed++;

            // Vérifier si deux tours se sont écoulés depuis l'utilisation de la compétence de l'assassin
            if (turnsSinceAssassinSkillUsed >= 2)
            {
                EnableTargetClassCompetenceButton(classeCibleAssassin);
                turnsSinceAssassinSkillUsed = 0; // Réinitialiser le compteur
            }
        }

        UpdateTurnTextAndImage(); // Mettre à jour l'affichage pour le prochain tour
    }






    // changer les informations à chaque tour
    private void UpdateTurnTextAndImage()
    {
        if (selectClasseScript.PlayerNames.Count > currentPlayerIndex)
        {

            string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex]; // chercher les données du script
            // convertie les couleur en format hexadécimal RGB
            string colorHex = ColorUtility.ToHtmlStringRGB(classColors.ContainsKey(currentPlayerClass) ? classColors[currentPlayerClass] : Color.white);
            turnText.text = $"<color=#{colorHex}>{currentPlayerClass}</color>"; // changer la couleur

            // changer l'image
            if (classImageDictionary.ContainsKey(currentPlayerClass))
            {
                classImageDisplay.sprite = classImageDictionary[currentPlayerClass];
            }
            // changer le text de la description et nom de la compétence
            if (skillBank.ContainsKey(currentPlayerClass))
            {
                skillNameText.text = skillBank[currentPlayerClass].skillName;
                skillDescriptionText.text = skillBank[currentPlayerClass].description;
            }
            // aller chercher la description du role du joueur et l'afficher
            Select_role.Role currentPlayerRole = selectRoleScript.GetPlayerRole(currentPlayerIndex);
            if (currentPlayerRole != null)
            {
                roleDescriptionText.text = currentPlayerRole.description;
            }

            // quand cest le tour du chef changer le text du btn : Btn_choix_chef
            string currentPlayerName = selectClasseScript.PlayerNames[currentPlayerIndex];
            if (currentPlayerName == selectChefScript.GetNomChef())
            {
                btnChoixChef.GetComponentInChildren<TMP_Text>().text = "Faire votre choix";
                btnChoixChef.interactable = true;
            }
            else
            {
                // si le joueur en cour nest pas le chef changer le text du bouton de choix pour la mission et le désactivé
                btnChoixChef.GetComponentInChildren<TMP_Text>().text = "En attente du chef";
                btnChoixChef.interactable = false;
            }

            // Gérer l'activation du bouton Btn_utilise_competence
            if (competenceUtiliseePourClasse.ContainsKey(currentPlayerClass) && competenceUtiliseePourClasse[currentPlayerClass])
            {
                // Si la compétence a été utilisée pour la classe en cours, désactiver le bouton et mettre à jour le texte
                btnUtiliseCompetence.interactable = false;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Compétence utilisée";
            }
            else
            {
                // Sinon, activer le bouton et réinitialiser le texte par défaut
                btnUtiliseCompetence.interactable = true;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Utiliser compétence"; // Mettez ici le texte que vous voulez par défaut
            }
            // changer le text du bouton pour utiliser la compétence et le désactiver
            if (targetClassForTextChange == currentPlayerClass)
            {
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Compétence bloquer par l'assassin";
                targetClassForTextChange = null;
            }

        }
    }





    // charger la scène suivante  À ABSOLUMENT GARDER DONC PAS TOUCHE PLEASE :)
    private void LoadEmptyScene()
    {
        SceneManager.LoadScene("scene_vide");
    }

    // si le joueur accepte d'utilisé sa compétence
    private void OnYesButtonClicked()
    {
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];
        Debug.Log(currentPlayerClass + " a utilisé sa compétence.");
        skillUsed[currentPlayerClass] = true; // Mettre à jour le statut de la compétence comme utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences

        btnUtiliseCompetence.interactable = false;
        btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Compétence utilisée";
        competenceUtiliseePourClasse[currentPlayerClass] = true;

        // Si l'assassin accepte t'utiliser sa compétence
        if (currentPlayerClass == "Assassin")
        {
            // Activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

            // Désactivez le bouton avec le texte "Assassin"
            Button[] buttons = panelCompetenceChoixJoueur.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null && buttonText.text == "Assassin")
                {
                    button.interactable = false; // Désactivez le bouton
                    break; // Quittez la boucle car vous avez trouvé le bouton
                }
            }
        }
        // si la sorcière accepte d'utiliser sa compétence
        if (currentPlayerClass == "Sorcière")
        {

            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);
        }
        // si le démoniste refuse d'utilisé sa compétence
        if (currentPlayerClass == "Démoniste")
        {
            panelCompetenceChoixJoueur.SetActive(true);
        }
        // si le clerc accepte d'utiliser sa compétence
        if (currentPlayerClass == "Clerc")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }
        // si le moine accepte d'utiliser sa compétence
        if (currentPlayerClass == "Moine")
        {
            // Si c'est le cas, activez le panel Panel_competence_choix_joueur afin que le joueur choisit une cible
            panelCompetenceChoixJoueur.SetActive(true);

        }

        // NOTE : AJOUTER LES AUTRES CLASSES PLUS TARD
    }


    // si le joueur refuse d'utiliser sa compétence
    private void OnNoButtonClicked()
    {
        Debug.Log("Compétence non utilisée.");
        skillUsed[selectClasseScript.PlayerNames[currentPlayerIndex]] = false; // Mettre à jour le statut de la compétence comme non utilisée
        panelCompetence.SetActive(false); // Fermer le panneau de compétences
    }
    private void OnChoiceButtonClicked(Button clickedButton)
    {

        if (selectedButtons.Contains(clickedButton))
        {
            // Désélectionner le bouton
            clickedButton.image.color = defaultButtonColor;
            selectedButtons.Remove(clickedButton);
            selectedButtonCount--; // diminuer de -1, le nombre de bouton sélectionner
        }
        else if (selectedButtonCount < 2) // si le nombre de bouton sélectionner est plus petit que 2 faire ceci
        {
            // Sélectionner le bouton
            clickedButton.image.color = selectedButtonColor;
            selectedButtons.Add(clickedButton);
            selectedButtonCount++; // ajouter + 1 au nombre de bouton
        }

        // Mettre à jour le texte confirm
        text_confirm.text = $"{selectedButtonCount}/1";

        // Activer ou désactiver le bouton de confirmation
        btn_confirm_choix.interactable = (selectedButtonCount == 1);

        // Récupérer le nom de la classe du joueur actuel en utilisant currentPlayerIndex
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];

        // Réactiver ou désactiver les autres boutons si nécessaire
        foreach (var button in choiceButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            // Vérifier si le bouton est celui de l'Assassin et si c'est le tour de l'Assassin
            if (buttonText != null && buttonText.text == "Assassin" && currentPlayerClass == "Assassin")
            {
                button.interactable = false; // Gardez le bouton de l'Assassin désactivé
                continue; // Continuez avec le prochain bouton dans la boucle
            }

            if (!selectedButtons.Contains(button) && selectedButtonCount < 1)
            {
                button.interactable = true; // Réactiver les bouton
            }
            else if (!selectedButtons.Contains(button) && selectedButtonCount == 1)
            {
                button.interactable = false; // Désactiver les bouton
            }
        }
    }

    private void UpdateConfirmButtonState()
    {
        // mettre à jour le statut du bouton
        text_confirm.text = $"{selectedButtonCount}/1";
        btn_confirm_choix.interactable = (text_confirm.text == "1/1");
    }

    private void OnConfirmButtonClicked()
    {
        // récupérer le tour du joueur en cour
        string currentPlayerClass = selectClasseScript.PlayerNames[currentPlayerIndex];

        // si tel classes clique sur btn_confirm_choice pour confirmer sa cible
        switch (currentPlayerClass)
        {
            // si c'est la sorcière
            case "Sorcière":
               
                Debug.Log("La sorcière aperçoit le choix de quelqu'un");
                break;

                // si c'est le démoniste
            case "Démoniste":

                if (selectedButtons.Count > 0)
                {
                    classeCibleDemoniste = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    Debug.Log($"Le Démoniste à maudit la cible suivant: {classeCibleDemoniste}");

                    // Réinitialiser la sélection après avoir confirmé la cible
                    foreach (var button in choiceButtons)
                    {
                        button.image.color = defaultButtonColor;
                    }
                    selectedButtons.Clear(); // Vider la liste des boutons sélectionnés
                    selectedButtonCount = 0; // Réinitialiser le compte des boutons sélectionnés
                    UpdateConfirmButtonState(); // Mettre à jour l'état du bouton de confirmation

                    // Ajoutez ici toute autre logique nécessaire pour la compétence du Clerc
                }
                break;

            //si c'est la clerc
            case "Clerc":
                // Implement the logic for Clerc here
                if (selectedButtons.Count > 0)
                {
                    classeCibleClerc = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    Debug.Log($"Le Clerc a choisi sa cible: {classeCibleClerc}");

                    // Réinitialiser la sélection après avoir confirmé la cible
                    foreach (var button in choiceButtons)
                    {
                        button.image.color = defaultButtonColor;
                    }
                    selectedButtons.Clear(); // Vider la liste des boutons sélectionnés
                    selectedButtonCount = 0; // Réinitialiser le compte des boutons sélectionnés
                    UpdateConfirmButtonState(); // Mettre à jour l'état du bouton de confirmation

                    // Ajoutez ici toute autre logique nécessaire pour la compétence du Clerc
                }
                break;

                // si c'est le moine
            case "Moine":
                // Implement the logic for Moine here
                Debug.Log("Le moine à amener un joueur à méditer");
                break;

                // si c'est le moin
            case "Assassin":
                if (selectedButtons.Count > 0)
                {
                    classeCibleAssassin = selectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                    Debug.Log($"L'assassin a ciblé la classe: {classeCibleAssassin}");

                    // Réinitialiser la couleur de tous les boutons à leur couleur initiale
                    foreach (var button in choiceButtons)
                    {
                        button.image.color = defaultButtonColor;
                    }
                    selectedButtons.Clear(); // Vider la liste des boutons sélectionnés
                    selectedButtonCount = 0; // Réinitialiser le compte des boutons sélectionnés
                    UpdateConfirmButtonState(); // Mettre à jour l'état du bouton de confirmation
                    isAssassinSkillUsedThisTurn = true;

                    // Enregistrez le tour actuel dans TourDepuisAssassina
                    TourDepuisAssassina = completeTurnCount;
                }
                break;
        }
    }

    // fonction compétence assassin pour désactiver le bouton compétence de sa cible
    private void DisableTargetClassCompetenceButton(string targetClassName)
    {
        // trouver l'index de la cible
        int targetPlayerIndex = selectClasseScript.PlayerNames.IndexOf(targetClassName);
        // si trouver 
        if (targetPlayerIndex != -1)
        {
            // si il exist, considerer que l'assassin a utiliser sa compétence
            competenceUtiliseePourClasse[targetClassName] = true;
            if (targetPlayerIndex == currentPlayerIndex) // si la cible est en train de jouer, désactiver son bouton compétence
            {
                btnUtiliseCompetence.interactable = false;
            }
            // Stockez la classe ciblée pour le changement de texte au prochain tour
            targetClassForTextChange = targetClassName;
        }
        

    }
    // réactiver le bouton de la cible
    private void EnableTargetClassCompetenceButton(string targetClassName)
    {
        // trouver l'index de la cible
        int targetPlayerIndex = selectClasseScript.PlayerNames.IndexOf(targetClassName);
        if (targetPlayerIndex != -1)
        {
            competenceUtiliseePourClasse[targetClassName] = false;
            if (targetPlayerIndex == currentPlayerIndex)
            {
                btnUtiliseCompetence.interactable = true;
                btnUtiliseCompetence.GetComponentInChildren<TMP_Text>().text = "Utiliser compétence";
            }
            turnsSinceAssassinSkillUsed = 0; // Réinitialisez le compteur de tours depuis l'utilisation de la compétence de l'assassin
        }
       
    }




}


