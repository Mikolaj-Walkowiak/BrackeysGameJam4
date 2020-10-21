using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerShooting : MonoBehaviour
{
    #region Singleton
    public static PlayerShooting instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion
    
    public Transform pewPew;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public AudioSource shotAudioSource;
    public Item currentWeapon;
    private Inventory inventory;
    private PlayerController playerController;
    public bool alreadyShot = false;

    public delegate void OnShootAmmoChanged();

    public OnShootAmmoChanged onShootAmmoChangedCallback;
    private void Start()
    {
        shotAudioSource = transform.GetChild(1).GetComponent<AudioSource>();
        inventory = Inventory.instance;
        playerController = GetComponent<PlayerController>();

    }
    public void Shoot()
    {
        currentWeapon = inventory.items[inventory.activeItem];
        if (currentWeapon.shot)
            return;
        StartCoroutine(currentWeapon.ShootDelay());
        GameObject muzzle = (GameObject) Instantiate(muzzleFlash, pewPew.position, pewPew.rotation);
        StartCoroutine(Flash());
        //playerController.weaponLight.intensity = 0;
        Destroy(muzzle, 0.15f);
        shotAudioSource.clip = currentWeapon.shotSound;
        shotAudioSource.Play();
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, pewPew.position, pewPew.rotation);
        bullet.GetComponent<Bullet>().shotBy = gameObject;
        inventory.items[inventory.activeItem].currentAmmo -= 1;
        if (onShootAmmoChangedCallback != null)
            onShootAmmoChangedCallback.Invoke();
    }

    IEnumerator Flash()
    {
        playerController.weaponLight.intensity = 8f;
        yield return new WaitForSeconds(0.02f);
        playerController.weaponLight.intensity = 0;
    }
}
