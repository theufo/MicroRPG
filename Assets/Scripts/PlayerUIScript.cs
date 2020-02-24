using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI InventoryText;
    public TextMeshProUGUI InteractText;
    public Image HealthFill; 
    public Image XpFill;

    private PlayerScript player;
    
    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    public void UpdateLevelText()
    {
        LevelText.text = "lvl\n" + player.currentLevel;
    }
    public void UpdateHealthBarFill()
    {
        HealthFill.fillAmount = (float)player.curHp / (float)player.maxHp;
    }
    public void UpdateXpBarFill()
    {
        XpFill.fillAmount = (float)player.currentXp / (float)player.xpToNextLevel;
    }

    public void SetInteractText(Vector3 pos, string text)
    {
        InteractText.gameObject.SetActive(true);
        InteractText.text = text;

        InteractText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
    }

    public void DisableInteractText()
    {
        if (InteractText.gameObject.activeInHierarchy)
        {
            InteractText.gameObject.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        InventoryText.text = string.Empty;

        foreach (var item in player.Inventory)
        {
            InventoryText.text += item + "\n";
        }
    }
}