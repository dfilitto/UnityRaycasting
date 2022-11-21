using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    //falta adicionar novas coisas !!!!!
    //som da arma
    private AudioSource gunAudio;
    //inicio do tiro
    public Transform gunEnd;
    //Camera do player
    private Camera fpsCam;
    //raycast
    private LineRenderer laserLine;

    //controla os tiros!!!!
    private float nextFire = 0;
    private float fireRate = 0.3f;
    private float weaponRange = 50;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.4f);

    //danos ao alvo
    [SerializeField]
    private int gunDamage = 1;
    [SerializeField]
    private float hitForce = 100f;
    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
        gunAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            //tiro
            nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f));
            //tratar a colisão
            RaycastHit hit;
            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                //pos final 
                laserLine.SetPosition(1, hit.point);

                ShootableBox helth = hit.collider.GetComponent<ShootableBox>();

                if (helth != null)
                {
                    helth.Damage(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect()

    {
        gunAudio.Play();
        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;

    }
}
