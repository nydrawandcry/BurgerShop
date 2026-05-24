using UnityEngine;

public class MeatDispenserSelector : MonoBehaviour
{
    [Header("Meat Dispensers")]
    [SerializeField] private GameObject chickenDispenser;
    [SerializeField] private GameObject beefDispenser;

    private void Start()
    {
        ApplyMeatSetting();
    }

    public void ApplyMeatSetting()
    {
        if (chickenDispenser == null)
        {
            Debug.LogError("MeatDispenserSelector: не назначен Chicken Dispenser.");
            return;
        }

        if (beefDispenser == null)
        {
            Debug.LogError("MeatDispenserSelector: не назначен Beef Dispenser.");
            return;
        }

        switch (GameSettings.Meat)
        {
            case MeatType.Chicken:
                chickenDispenser.SetActive(true);
                beefDispenser.SetActive(false);
                Debug.Log("Выбран Chicken. Активен chicken dispenser.");
                break;

            case MeatType.Beef:
                chickenDispenser.SetActive(false);
                beefDispenser.SetActive(true);
                Debug.Log("Выбран Beef. Активен beef dispenser.");
                break;
        }
    }
}