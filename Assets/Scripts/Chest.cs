using UnityEngine;

public class Chest : MonoBehaviour
{
    public int xpToGive;

    public void OpenChest()
    {
        FindObjectOfType<PlayerScript>().AddXp(xpToGive);
        Destroy(gameObject);
    }
}