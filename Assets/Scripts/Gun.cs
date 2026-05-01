using RadiantTools.AudioSystem;
using TMPro;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float fireRate = 0.1f;
    public bool isAutomatic = true;
    public int bulletsPerShot = 1;
    public float bulletSpread = 2f;
    public float reloadTime = 2f;

    [Header("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] TextMeshProUGUI ammoText;
    AudioPlayer gunSFX;

    [Header("Damage")]
    public float damage = 10f;
    public float range = 100f;

    private float lastFireTime;

    void Awake()
    {
        currentAmmo = magazineSize;
        firePoint = transform.Find("FirePoint");
        if (ammoText)
        {
            ammoText.text = currentAmmo.ToString() + "/" + magazineSize.ToString();
        }
    }

    // ?? This is the ONLY method external systems call
    public bool TryShoot()
    {
        if (Time.time - lastFireTime < fireRate) return false;
        if (currentAmmo <= 0) return false;
        PlayGunSound();
        MuzzleFlash();
        Shoot();
        if (ammoText) { 
            ammoText.text = currentAmmo.ToString() + "/" + magazineSize.ToString();
        }
        lastFireTime = Time.time;
        return true;
    }

    void PlayGunSound()
    {
        if (!gunSFX) { 
            gunSFX = AudioManager.Instance.GetAudioPlayer("gunSFX"); 
        }
        gunSFX.SetAudioSettings(pitch: Random.Range(0.25f,0.75f));
        gunSFX.PlayAudioOnce(SoundTypes.GunSound);
    }

    void MuzzleFlash()
    {
        GameObject mz = Instantiate(muzzleFlash.gameObject, firePoint);
        mz.transform.localPosition = Vector3.zero;
        mz.transform.localScale = Vector3.one * 3;
    }

    void Shoot()
    {
        currentAmmo--;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 direction = GetSpreadDirection();
            print("Shot Reaching");
            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, range))
            {
                var damageable = hit.collider.GetComponent<Health>();
                print("Shot : " + damageable);

                if (damageable != null)
                    damageable.TakeDamage(damage);
            }
        }
    }

    Vector3 GetSpreadDirection()
    {
        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        return Quaternion.Euler(spreadX, spreadY, 0) * firePoint.forward;
    }

    public void Reload()
    {
        StartCoroutine(StartReloading());
    }
    IEnumerator StartReloading()
    {
        gunSFX.PlayAudioOnce(SoundTypes.ReloadSound);
        if(ammoText)
        {
            ammoText.text = "Reloading...";
        }
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        if (ammoText)
        {
            ammoText.text = currentAmmo.ToString() + "/" + magazineSize.ToString();
        }
    }
}