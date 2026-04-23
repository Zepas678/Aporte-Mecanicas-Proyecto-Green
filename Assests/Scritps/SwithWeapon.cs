using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwithWeapon : MonoBehaviour
{

    PlayerInput playerInput;

    InputAction a_switchWeaponLeft;

    InputAction a_switchWeaponRight;

    Animator animator;

    SkinnedMeshRenderer SKR;

    Material[] shadersWeapons;

    bool Switch;

    public List<Image> weaponsUI;

    public List<TMP_Text> bulletsUI;

    int weaponCurrent;

    [SerializeField] Green_Controller green_Controller;

    [SerializeField] List<Weapon_template> Weapons;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        SKR = transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();

        shadersWeapons = SKR.sharedMaterials;

        animator = transform.Find("Green_Model").GetComponent<Animator>();

        a_switchWeaponLeft = playerInput.actions["SwitchWeaponLeft"];
        a_switchWeaponRight = playerInput.actions["SwitchWeaponRight"];

        if (weaponCurrent == 0)
        {
            for (int i = 0; i < weaponsUI.Count; i++)
            {
                //weaponsUI[i].SetActive(false);
                weaponsUI[i].color = new Color(1, 1, 1, 0.17f);
                bulletsUI[i].color = new Color(221, 221, 221, 0.17f);
            }
        }

        bulletsUI[1].text = Weapons[2].bulletCurrent == 0 ? "0" : Weapons[2].bulletCurrent.ToString();
        bulletsUI[0].text = Weapons[1].bulletCurrent == 0 ? "0" : Weapons[1].bulletCurrent.ToString();
        bulletsUI[2].text = Weapons[3].bulletCurrent == 0 ? "0" : Weapons[3].bulletCurrent.ToString();

        Switch = true;
    }

    void Update()
    {
        green_Controller.bulletWeapon = Weapons[weaponCurrent].bulletCurrent;

        if ((animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_attack") ||
        (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_attack_2") ||
        (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.ToString() == "green_attack_2_move"))
        {
            Switch = false;
        }
        else
        {
            Switch = true;
        }

    }

    void OnEnable()
    {
        a_switchWeaponLeft.started += onSwitchWeaponLeft;
        a_switchWeaponRight.started += onSwitchWeaponRight;
    }

    void OnDisable()
    {
        a_switchWeaponLeft.started -= onSwitchWeaponLeft;
        a_switchWeaponRight.started -= onSwitchWeaponRight;
    }

    void onSwitchWeaponLeft(InputAction.CallbackContext context)
    {
        if (Switch)
        {
            if (weaponCurrent <= 0)
            {
                weaponCurrent = Weapons.Count - 1;
                shadersWeapons[0] = Weapons[weaponCurrent].suitWeapon;
                shadersWeapons[1] = Weapons[weaponCurrent].suitWeapon;
                SKR.sharedMaterials = shadersWeapons;
            }
            else
            {
                shadersWeapons[0] = Weapons[--weaponCurrent].suitWeapon;
                shadersWeapons[1] = Weapons[weaponCurrent].suitWeapon;
                SKR.sharedMaterials = shadersWeapons;
            }

            isPower();

            updateWeaponsUI();
        }
    }

    void onSwitchWeaponRight(InputAction.CallbackContext context)
    {
        if (Switch)
        {
            if (weaponCurrent >= Weapons.Count - 1)
            {
                weaponCurrent = 0;
                shadersWeapons[0] = Weapons[weaponCurrent].suitWeapon;
                shadersWeapons[1] = Weapons[weaponCurrent].suitWeapon;
                SKR.sharedMaterials = shadersWeapons;
            }
            else
            {
                shadersWeapons[0] = Weapons[++weaponCurrent].suitWeapon;
                shadersWeapons[1] = Weapons[weaponCurrent].suitWeapon;
                SKR.sharedMaterials = shadersWeapons;
            }

            isPower();

            updateWeaponsUI();
        }
    }

    void updateWeaponsUI()
    {
        if (weaponCurrent != 0)
        {
            for (int i = 0; i < weaponsUI.Count; i++)
            {
                if (i + 1 == weaponCurrent)
                {
                    //weaponsUI[i].SetActive(true);
                    weaponsUI[i].color = new Color(1, 1, 1, 1);
                    bulletsUI[i].color = new Color(221, 221, 221, 1);
                }
                else
                {
                    //weaponsUI[i].SetActive(false);
                    weaponsUI[i].color = new Color(1, 1, 1, 0.17f);
                    bulletsUI[i].color = new Color(221, 221, 221, 0.17f);
                }
            }
        }
        else
        {
            for (int i = 0; i < weaponsUI.Count; i++)
            {
                //weaponsUI[i].SetActive(false);
                weaponsUI[i].color = new Color(1, 1, 1, 0.17f);
                bulletsUI[i].color = new Color(221, 221, 221, 0.17f);
            }
        }
    }

    void isPower()
    {
        if (Weapons[weaponCurrent].isPower)
        {
            green_Controller.isPower = true;
            green_Controller.weapon = Weapons[weaponCurrent].weapon;
        }
        else
        {
            green_Controller.isPower = false;
        }

        green_Controller.clicAttack = Weapons[weaponCurrent].audioAttack;

        green_Controller.timeFire = Weapons[weaponCurrent].timeFire;
    }

    public void reloadBulletWeapon(Weapon_template weapon)
    {
        switch (weapon.name)
        {
            case "Weapon_Electric":
                Weapons[2].bulletCurrent += weapon.getBullet;
                bulletsUI[1].text = Weapons[2].bulletCurrent.ToString();
                break;
            case "Weapon_Fire":
                Weapons[1].bulletCurrent += weapon.getBullet;
                bulletsUI[0].text = Weapons[1].bulletCurrent.ToString();
                break;
            case "Weapon_Ice":
                Weapons[3].bulletCurrent += weapon.getBullet;
                bulletsUI[2].text = Weapons[3].bulletCurrent.ToString();
                break;
        }
    }

    public void shotBulletWeapon(int bulletCurrent, GameObject weapon)
    {
        switch (weapon.name)
        {
            case "Weapon_Electric":
                Weapons[2].bulletCurrent = bulletCurrent;
                bulletsUI[1].text = Weapons[2].bulletCurrent.ToString();
                break;
            case "Weapon_Fire":
                Weapons[1].bulletCurrent = bulletCurrent;
                bulletsUI[0].text = Weapons[1].bulletCurrent.ToString();
                break;
            case "Weapon_Ice":
                Weapons[3].bulletCurrent = bulletCurrent;
                bulletsUI[2].text = Weapons[3].bulletCurrent.ToString();
                break;
        }
    }
}
