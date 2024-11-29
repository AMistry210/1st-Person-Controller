using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunUIManager : MonoBehaviour
{
    public TextMeshProUGUI Ammo;
    public TextMeshProUGUI AmmoInMagazine;

    public void UpdateBulletCount(int currentAmmo, int reserveAmmo)
    {
        Ammo.text = "" + reserveAmmo;
        AmmoInMagazine.text = "" + currentAmmo;
    }
}


