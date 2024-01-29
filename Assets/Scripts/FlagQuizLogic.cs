using System.Collections;
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
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;


    void Start()
    {
        string jsonPath = "Assets/Resources/country_codes.json";

        if (System.IO.File.Exists(jsonPath))
        {
            string jsonString = System.IO.File.ReadAllText(jsonPath);

            // Deserialize JSON string into CountryList object
            CountryList countryList = JsonUtility.FromJson<CountryList>(jsonString);

            if (countryList != null && countryList.countries != null)
            {
                // Now you can access the list of countries
                foreach (Country country in countryList.countries)
                {
                    Debug.Log($"Country Code: {country.code}, Name: {country.name}");
                }
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON into CountryList");
            }

            Debug.Log($"JSON file loaded from path: {jsonPath}");
            Debug.Log($"Length of JSON string: {jsonString.Length}");
            Debug.Log($"Leength of CountryList: {countryList.countries.Count}");
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + jsonPath);
        }
    }


}
