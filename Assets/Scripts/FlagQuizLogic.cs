using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagQuizLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string jsonPath = "country_codes"; 

        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonPath);

        if (jsonTextAsset != null)
        {
            string jsonString = jsonTextAsset.text;
            // Now you can parse jsonString using JSON utility classes or libraries
            // For example, you can use JsonUtility.FromJson<MyDataClass>(jsonString);
        }
        else
        {
            Debug.LogError("Failed to load JSON file: " + jsonPath);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
