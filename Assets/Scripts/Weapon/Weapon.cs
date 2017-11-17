using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour {

    //private Animator anim;
    private AudioSource _AudioSource;
        
    public float range = 100f;          //Maximum range bullets can fly
    public int bulletsPerMag = 30;      //Bullets per mag
    public int bulletsLeft = 200;       //Total bullets we have
    public int currentBullets;          //The current bullets in our mag

    public enum ShootMode { Auto, Semi, ShotGun}
    public ShootMode shootingMode;

    public Text ammoText;

    public Transform shootPoint;        //The point from which the bullets come from
    public GameObject hitParticles;
    public GameObject bulletImpact;

    public ParticleSystem muzzleFlash;  //Muzzleflash
    public AudioClip shootSound;        //Sound for shooting

    public float fireRate = 0.5f;       //Delay between each shot
    public float damage = 20f;          //Damage dealt with each shot

    float fireTimer;

    private bool isReloading;
    //public bool isAiming;
    private bool shootInput;

    private Vector3 origianalPosition;
    public Vector3 aimPosition;
    public float adsSpeed;

    private void OnEnable()
    {
        // Update when active
        UpdateAmmoText(); //Update UI text
    }

    // Use this for initialization
    void Start () {
        //anim = GetComponent<Animator>();
        currentBullets = bulletsPerMag;

        _AudioSource = GetComponent<AudioSource>();
        origianalPosition = transform.localPosition;

        UpdateAmmoText(); //Update UI text
    }
	
	// Update is called once per frame
	void Update () {

        switch (shootingMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;

            case ShootMode.ShotGun:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }
		if(shootInput)
        {
            if (currentBullets > 0)
                Fire();
            else if(bulletsLeft > 0)
                DoReload();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentBullets < bulletsPerMag && bulletsLeft > 0)
            DoReload();
        }

        if(fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        //aimDownSights();
	}

    void FixedUpdate()
    {
        //AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        //isReloading = info.IsName("Reload");
        //anim.SetBool("Scoped", isAiming);

        //if (info.IsName("Fire")) anim.SetBool("Fire", false);
    }

    /*private void aimDownSights()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * adsSpeed);
            //isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, origianalPosition, Time.deltaTime * adsSpeed);
           // isAiming = false;
        }
    }*/

    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

        //Casting raycast to shoot
        RaycastHit hit;
        if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            GameObject hitParticlesEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            //Adds effects as child to object that got hit
            hitParticlesEffect.transform.SetParent(hit.transform);
            bulletHole.transform.SetParent(hit.transform);

            Destroy(hitParticlesEffect, 1f);
            Destroy(bulletHole, 2f);
        }

        //anim.CrossFadeInFixedTime("Fire", 0.01f);   //Play fire animation  
        muzzleFlash.Play();                         //Show muzzleflash
        PlayShootSound();                           //Play shoot sound

        currentBullets--; //Removes one bullet

        UpdateAmmoText(); //Update UI text

        fireTimer = 0.0f;

        if (hit.transform.GetComponent<HealthController>())
        {
            hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
        }
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;

        UpdateAmmoText(); //Update UI text
    }

    private void DoReload()
    {
        //AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;

        //anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
       // _AudioSource.clip = shootSound;
      //  _AudioSource.Play();
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentBullets + " / " + bulletsLeft;
    }
}
