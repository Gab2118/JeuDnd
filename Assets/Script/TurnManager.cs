using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class TurnManager : MonoBehaviour
{
    [Serializable]
    public class ClassImage
    {
        public string className;
        public Sprite classSprite;
    }

    public TMP_Text turnText;
    public Button nextTurnButton;
    public Image classImageDisplay; // Image UI pour afficher l'image de la classe
    public List<ClassImage> classImages; // Liste des paires classe-image définies dans l'inspecteur

    private Select_Classe selectClasseScript;
    private Select_chef selectChefScript;
    private int currentPlayerIndex = 0;
    private int turnCounter = 0;
    private Dictionary<string, Sprite> classImageDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Color> classColors = new Dictionary<string, Color>(); // Pour stocker les couleurs des classes

    void Start()
    {
        selectClasseScript = FindObjectOfType<Select_Classe>();
        selectChefScript = FindObjectOfType<Select_chef>();

        if (selectClasseScript == null || selectChefScript == null || classImageDisplay == null)
        {
            Debug.LogError("Required components or scripts not found in the scene!");
            return;
        }

        InitializeClassColors();
        InitializeClassImagesDictionary();
        SetStartingPlayerAsChef();
        nextTurnButton.onClick.AddListener(OnNextTurnButtonClicked);
        UpdateTurnTextAndImage();
    }

    private void InitializeClassColors()
    {
        // Assigner des couleurs au hasard aux classes
        classColors.Add("Assassin", Color.red);
        classColors.Add("Rôdeur", Color.green);
        classColors.Add("Barbare", Color.blue);
        classColors.Add("Moine", Color.cyan);
        classColors.Add("Barde", Color.magenta);
        classColors.Add("Occultiste", Color.yellow);
        classColors.Add("Sorcière", Color.grey);
        classColors.Add("Paladin", Color.white);
        classColors.Add("Guerrier", new Color(0.5f, 0.25f, 0)); // Marron
        classColors.Add("Clerc", new Color(1f, 0.5f, 0)); // Orange
        classColors.Add("Nécromancien", Color.black);
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

    private void SetStartingPlayerAsChef()
    {
        string chefName = selectChefScript.GetNomChef();
        currentPlayerIndex = selectClasseScript.PlayerNames.IndexOf(chefName);
        if (currentPlayerIndex == -1)
        {
            Debug.LogError("Le chef n'a pas été trouvé dans la liste des joueurs !");
            currentPlayerIndex = 0; // Utilisez 0 par défaut si le chef n'est pas trouvé
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
                Debug.LogWarning($"Aucune image trouvée pour la classe {currentPlayerClass}");
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
}
