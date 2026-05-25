using UnityEngine;
using UnityEngine.UI;

public class RecipeIngredientIconUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private void Awake()
    {
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
        }
    }

    public void Setup(Sprite sprite)
    {
        if (iconImage == null)
        {
            return;
        }

        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
    }
}