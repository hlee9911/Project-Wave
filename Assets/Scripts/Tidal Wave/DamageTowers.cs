using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTowers : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Heavy Turret"))
        {
            //Debug.Log("Water damaged HT tower!");
            collision.GetComponent<HeavyTurret>().SetDamage();
        }
        if (collision.CompareTag("Basic Turret"))
        {
            //Debug.Log("Water damaged basic tower!");
            collision.GetComponent<BasicTurret>().SetDamage();
        }
        if (collision.CompareTag("Piercing Turret"))
        {
            //Debug.Log("Water damaged piercring tower!");
            collision.GetComponent<PiercingTower>().SetDamage();
        }
        if (collision.CompareTag("Oil Turret"))
        {
            //Debug.Log("Water damaged Oil tower!");
            collision.GetComponent<OilTurretScript>().SetDamage();
        }
        if (collision.CompareTag("Water Turret"))
        {
            //Debug.Log("Water damaged water tower!");
            collision.GetComponent<WaterTurretScript>().SetDamage();
        }
    }
}
