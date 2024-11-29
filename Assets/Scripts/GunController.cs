using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 0.5f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 200;
    public bool _canShoot;
    public int _currentAmmoClip;
    public int _ammoInReserve;

    //Muzzle flash
    public Image muzzleFlashImage;
    public Sprite[] flash;

    //Aim in
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

    public float aimSmoothing = 10;

    //Audio
    public float ClipLength = 1f;
    public GameObject AudioClip;

    public Animator animator;
    public LayerMask whatIsEnemy;
    public int damageAmount = 40;

    public GunUIManager gunUIManager;

    void Start()
    {
        _currentAmmoClip = clipSize;
        _ammoInReserve = reservedAmmoCapacity;
        _canShoot = true;
        AudioClip.SetActive(false);
    }

    void Update()
    {
        DetermainAim();
        if (Input.GetMouseButton(0) && _canShoot && _currentAmmoClip > 0)
        {
            _canShoot = false;
            _currentAmmoClip--;
            StartCoroutine(ShootGun());
            animator.SetBool("Reloading", false);
            UpdateGunUI(); 
            RayCastForEnemy();
        }
        else if (Input.GetKeyDown(KeyCode.R) && _currentAmmoClip < clipSize && _ammoInReserve > 0)
        {
            int amountNeeded = clipSize - _currentAmmoClip;
            animator.SetBool("Reloading", true);
            if (amountNeeded >= _ammoInReserve)
            {
                _currentAmmoClip += _ammoInReserve;
                _ammoInReserve = 0;
            }
            else
            {
                _currentAmmoClip = clipSize;
                _ammoInReserve -= amountNeeded;
            }
            UpdateGunUI(); 
        }
    }


    public void UpdateGunUI()
    {
        gunUIManager.UpdateBulletCount(_currentAmmoClip, _ammoInReserve);
    }

    void DetermainAim()
    {
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredPosition;
    }

    IEnumerator ShootGun()
    {
        StartCoroutine(MuzzleFlash());
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;

        AudioClip.SetActive(true);
        yield return new WaitForSeconds(ClipLength);
        AudioClip.SetActive(false);
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlashImage.sprite = flash[Random.Range(0, flash.Length)];
        muzzleFlashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlashImage.sprite = null;
        muzzleFlashImage.color = new Color(0, 0, 0, 0);
    }

    private void RayCastForEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, whatIsEnemy))
        {
            Debug.Log("Hit an enemy");
            EnemyAi enemy = hit.transform.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit an enemy");
        }
    }
}
