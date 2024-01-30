using GoogleMobileAds.Api;
using System.Collections.Generic;
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
    public Text scoreText;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    [Header("Debug")]
    public int score = 0;
    private CountryList countryList;

    void Start()
    {
        string jsonPath = "Assets/Resources/country_codes.json";

        if (System.IO.File.Exists(jsonPath))
        {
            string jsonString = System.IO.File.ReadAllText(jsonPath);

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
        else
        {
            Debug.LogError("JSON file not found at path: " + jsonPath);
        }
    }

    void StartGame()
    {
        scoreText.text = $"Score: {score}";
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

        button1.GetComponentInChildren<Text>().text = otherCountries[0].name;
        button2.GetComponentInChildren<Text>().text = otherCountries[1].name;
        button3.GetComponentInChildren<Text>().text = otherCountries[2].name;
        button4.GetComponentInChildren<Text>().text = otherCountries[3].name;

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
            ResetScore();
            StartGame();

            IntersitialAd ad = ads.GetComponent<IntersitialAd>();
            ad.Show();
        }
    }

    void AddScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
        StartGame();
    }

    void ResetScore()
    {
        score = 0;
        scoreText.text = $"Score: {score}";
    }
        

}
