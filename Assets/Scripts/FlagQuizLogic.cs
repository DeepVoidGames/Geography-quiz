using GoogleMobileAds.Api;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Country
{
    public string code;
    public string name;
}

[System.Serializable]
public class CountryList
{
    public List<Country> countries;
}

public class FlagQuizLogic : MonoBehaviour
{
    [Header("Ads")]
    public GameObject ads;

    [Header("UI Elements")]
    public Image flagImage;
    public TMP_Text scoreText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    
    [Header("Lose Panel UI")]
    public GameObject losePanel;
    public TMP_Text bestScoreText;
    public TMP_Text loseScoreText;

    [Header("Stats")]
    public int score = 0;
    public int highScore = 0;
    private CountryList countryList;
    
    void Awake(){
        losePanel.SetActive(false);
    }

    void Start()
    {
        string jsonPath = "country_codes";

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        var countryCodesAsset = Resources.Load(jsonPath, typeof(TextAsset)) as TextAsset;
        if (countryCodesAsset == null)
        {
            Debug.LogError("Resources.Load() failed");
        }
        else
        {
            string jsonString = countryCodesAsset.text;
            Debug.Log("Loaded json: " + jsonString);

            // Deserialize JSON string into CountryList object
            countryList = JsonUtility.FromJson<CountryList>(jsonString);

            if (countryList == null || countryList.countries == null)
            {
                Debug.LogError("Failed to deserialize JSON into CountryList");
            }

            Debug.Log($"JSON file loaded from path: {jsonPath}");
            Debug.Log($"Length of JSON string: {jsonString.Length}");
            Debug.Log($"Length of CountryList: {countryList.countries.Count}");

            StartGame();
        }
    }

    void StartGame()
    {
        scoreText.SetText($"Score: {score}");
        // Pick a random country
        int randomIndex = Random.Range(0, countryList.countries.Count);
        Country randomCountry = countryList.countries[randomIndex];
        

        // Pick 3 other random countries
        List<Country> otherCountries = new List<Country>();
        otherCountries.Add(randomCountry);
        while (otherCountries.Count < 4)
        {
            int otherRandomIndex = Random.Range(0, countryList.countries.Count);
            Country otherRandomCountry = countryList.countries[otherRandomIndex];

            if (otherRandomCountry != randomCountry)
            {
                otherCountries.Add(otherRandomCountry);
            }
        }
        // Suffle the list of countries
        for (int i = 0; i < otherCountries.Count; i++)
        {
            Country temp = otherCountries[i];
            int randomIndex2 = Random.Range(i, otherCountries.Count);
            otherCountries[i] = otherCountries[randomIndex2];
            otherCountries[randomIndex2] = temp;
        }

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        button4.onClick.RemoveAllListeners();

        button1.onClick.AddListener(() => CheckAnswer(otherCountries[0].code, randomCountry.code));
        button2.onClick.AddListener(() => CheckAnswer(otherCountries[1].code, randomCountry.code));
        button3.onClick.AddListener(() => CheckAnswer(otherCountries[2].code, randomCountry.code));
        button4.onClick.AddListener(() => CheckAnswer(otherCountries[3].code, randomCountry.code));

        button1.GetComponentInChildren<TMP_Text>().SetText(otherCountries[0].name);
        button2.GetComponentInChildren<TMP_Text>().SetText(otherCountries[1].name);
        button3.GetComponentInChildren<TMP_Text>().SetText(otherCountries[2].name);
        button4.GetComponentInChildren<TMP_Text>().SetText(otherCountries[3].name);

        // Load the flag image
        string flagPath = $"Flags/{randomCountry.code}";
        Sprite flagSprite = Resources.Load<Sprite>(flagPath);
        float ratio = (float)flagSprite.texture.width / (float)flagSprite.texture.height;
        float dispWidth = 216 - 32;
        float imW = dispWidth;
        float imH = imW / ratio;
        if (imH > 150)
        {
            imH = 150;
            imW = imH / ratio;
        }

        flagImage.GetComponent<RectTransform>().sizeDelta = new Vector2(imW, imH);
        if (flagSprite == null)
        {
            Debug.LogError($"Failed to load flag image from path: {flagPath}");
        }
        else
        {
            flagImage.sprite = flagSprite;
        }
    }

    void CheckAnswer(string answer, string correctAnswer)
    {
        if (answer == correctAnswer)
        {
            Debug.Log("Correct!");
            AddScore();
        }
        else
        {
            Debug.Log("Incorrect!");
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
            }
            losePanel.SetActive(true);
            bestScoreText.SetText($"Best Score: {highScore}");
            loseScoreText.SetText($"Actual Score: {score}");
            IntersitialAd ad = ads.GetComponent<IntersitialAd>();
            ad.Show();
        }
    }

    void AddScore()
    {
        score++;
        scoreText.SetText($"Score: {score}");
        StartGame();
    }

    void ResetScore()
    {
        score = 0;
        scoreText.SetText($"Score: {score}");
    }
        
    public void ResetGame()
    {
        ResetScore();
        losePanel.SetActive(false);
        StartGame();
    }
}
