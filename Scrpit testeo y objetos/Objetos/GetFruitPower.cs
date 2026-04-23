using Unity.VisualScripting;
using UnityEngine;

public class GetFruitPower : MonoBehaviour
{

    [SerializeField] Weapon_template weapon;

    void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("Player") || GameObject.FindGameObjectWithTag("Tongue_attack"))
        {
            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out SwithWeapon swithWeapon))
            {
                swithWeapon.reloadBulletWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
