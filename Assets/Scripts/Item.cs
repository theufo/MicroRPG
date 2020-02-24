using UnityEngine;

public class Item : MonoBehaviour
{
    public string Name;

    public void PickupItem()
    {
        FindObjectOfType<PlayerScript>().AddToInventory(Name);
        Destroy(gameObject);
    }
}