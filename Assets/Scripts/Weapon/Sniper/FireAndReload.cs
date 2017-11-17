using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FireAndReload : MonoBehaviour {

    private AudioSource _AudioSource;

    public float range = 100f;          //Maximum range bullets can fly
    public int bulletsPerMag = 30;      //Bullets per mag
    public int bulletsLeft = 200;       //Total bullets we have
    public int currentBullets;          //The current bullets in our mag

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

    private void OnEnable()
    {
        // Update when active
        UpdateAmmoText(); //Update UI text
    }

    void Start()
    { 
        currentBullets = bulletsPerMag;

        _AudioSource = GetComponent<AudioSource>();

        UpdateAmmoText(); //Update UI text
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentBullets > 0)
                Fire();
            else if (bulletsLeft > 0)
                Reload();
        }

        /*if (Input.GetAxis("Fire4") > 0)
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0)
                Reload();
        }*/

        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
    }

    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;
        Debug.Log("Shiet");
        //Casting raycast to shoot
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            //makes a SQL query to the database
            StartCoroutine(DoQuery());

            GameObject hitParticlesEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            //Adds effects as child to object that got hit
            hitParticlesEffect.transform.SetParent(hit.transform);
            bulletHole.transform.SetParent(hit.transform);

            Destroy(hitParticlesEffect, 1f);
            Destroy(bulletHole, 2f);
        }
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
        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;

        UpdateAmmoText(); //Update UI text
    }

    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
    }

    private void UpdateAmmoText()
    {
        ammoText.text = currentBullets + " / " + bulletsLeft;
    }

    /*IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(2);
        Reload();
    }*/


    //passes information to the database
    IEnumerator DoQuery()
    {
        float t_x = transform.position.x;
        float t_y = transform.position.y;
        float t_z = transform.position.z;
        WWW request = new WWW("http://22005.hosts.ma-cloud.nl/Jaar2/GPR/DB/Hello_World/GPR_DB2.php?t_x=" + t_x + "&t_y=" + t_y + "&t_z=" + t_z + "&p_id=20");
        yield return request;
    }
}
