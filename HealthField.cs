using UnityEngine;
using System.Collections;

public class HealthField : MonoBehaviour
{
    public float healthAmountPerSecond = 1;
    //---------------------------------------
    void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            if (player.health < player.vehicle.maxHealth)
            {
                player.health += healthAmountPerSecond * Time.deltaTime; // isn't that per frame or something!?
            }
        }

    }
}
