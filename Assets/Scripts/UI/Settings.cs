using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static int _previousSceneIndex = 0;

    [Header("Time Slider")] 
    [SerializeField] public Slider timeSlider; 
    [SerializeField] public TMP_Text timeText;
    
    [Header("Ingredients Slider")] 
    [SerializeField] public Slider ingredientsSlider; 
    [SerializeField] public TMP_Text ingredientsText;
    
    [Header("Meat Dropdown")] 
    [SerializeField] public TMP_Dropdown meatDropdown;

    private void Awake()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        SetupDropdown();
        SetupSliders();
        LoadSettingsToUI();
        RefreshLabels();
        SaveCurrentUIValues();
    }
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ReturnToPreviousScene();
        }
    }
    
    private void OnEnable()
    {
        if (!ValidateReferences())
        {
            return;
        }

        meatDropdown.onValueChanged.AddListener(OnMeatTypeChanged);
        ingredientsSlider.onValueChanged.AddListener(OnIngredientsChanged);
        timeSlider.onValueChanged.AddListener(OnTimeChanged);
    }

    private void OnDisable()
    {
        if (meatDropdown != null)
        {
            meatDropdown.onValueChanged.RemoveListener(OnMeatTypeChanged);
        }

        if (ingredientsSlider != null)
        {
            ingredientsSlider.onValueChanged.RemoveListener(OnIngredientsChanged);
        }

        if (timeSlider != null)
        {
            timeSlider.onValueChanged.RemoveListener(OnTimeChanged);
        }
    }
    
    private bool ValidateReferences()
    {
        bool isValid = true;

        if (timeSlider == null)
        {
            Debug.LogError("Settings: не назначен Time Slider.");
            isValid = false;
        }

        if (timeText == null)
        {
            Debug.LogError("Settings: не назначен Time Text.");
            isValid = false;
        }

        if (ingredientsSlider == null)
        {
            Debug.LogError("Settings: не назначен Ingredients Slider.");
            isValid = false;
        }

        if (ingredientsText == null)
        {
            Debug.LogError("Settings: не назначен Ingredients Text.");
            isValid = false;
        }

        if (meatDropdown == null)
        {
            Debug.LogError("Settings: не назначен Meat Dropdown.");
            isValid = false;
        }

        return isValid;
    }

    private void SetupDropdown()
    {
        meatDropdown.ClearOptions();

        meatDropdown.AddOptions(new List<string>
        {
            "Chicken",
            "Beef"
        });

        meatDropdown.value = (int)GameSettings.Meat;
        meatDropdown.RefreshShownValue();
    }

    private void SetupSliders()
    {
        ingredientsSlider.minValue = 3;
        ingredientsSlider.maxValue = 5;
        ingredientsSlider.wholeNumbers = true;

        timeSlider.minValue = 5;
        timeSlider.maxValue = 60;
        timeSlider.wholeNumbers = true;
    }

    private void LoadSettingsToUI()
    {
        ingredientsSlider.value = GameSettings.IngredientCount;
        timeSlider.value = GameSettings.TimePerBurger;
        meatDropdown.value = (int)GameSettings.Meat;

        meatDropdown.RefreshShownValue();
    }

    private void OnMeatTypeChanged(int index)
    {
        SaveCurrentUIValues();
    }

    private void OnIngredientsChanged(float value)
    {
        ingredientsText.text = Mathf.RoundToInt(value).ToString();
        SaveCurrentUIValues();
    }

    private void OnTimeChanged(float value)
    {
        timeText.text = Mathf.RoundToInt(value) + "";
        SaveCurrentUIValues();
    }

    private void RefreshLabels()
    {
        ingredientsText.text = Mathf.RoundToInt(ingredientsSlider.value).ToString();
        timeText.text = Mathf.RoundToInt(timeSlider.value) + "";
    }

    private void SaveCurrentUIValues()
    {
        MeatType selectedMeat = (MeatType)meatDropdown.value;
        int selectedIngredientCount = Mathf.RoundToInt(ingredientsSlider.value);
        int selectedTime = Mathf.RoundToInt(timeSlider.value);

        GameSettings.Set(selectedMeat, selectedIngredientCount, selectedTime);
    }
    
    public void ReturnToPreviousScene()
    {
        SceneManager.LoadScene(_previousSceneIndex);
    }
}