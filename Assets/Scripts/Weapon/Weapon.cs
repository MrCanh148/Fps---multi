using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    [SerializeField] private float fireRate;
    [SerializeField] private Camera cam;

    [Header("==========Vfx")]
    [SerializeField] private GameObject hitVfx;

    [Header("==========Ammo")]
    [SerializeField] private int mag = 5;
    [SerializeField] private int ammo = 30;
    [SerializeField] private int magAmmo = 30;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI magText;

    private Animator anim;
    const string RELOAD = "Reload";
    const string SHOOT = "Shoot";
    private bool canShoot = false;
    private float nextRate;

    [Header("Recoil")]
    [Range(0, 1)]
    [SerializeField] private float recoilPercent = 0.3f;
    [Range(0, 2)]
    [SerializeField] private float recoverPercent = 0.7f;
    [SerializeField] private float recoilUp = 1f;
    [SerializeField] private float recoilBack = 0f;
    private Vector3 originalRecoil;
    private Vector3 recoilVelocity = Vector3.zero;
    private float recoilLength;
    private float recoverLength;
    private bool recoiling;
    public bool recovering;

    private void Start()
    {
        anim = GetComponent<Animator>();
        recoilLength = 1 / fireRate * recoilPercent;
        recoverLength = 1 / fireRate * recoverPercent;
        originalRecoil = transform.localPosition;
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    private void Update()
    {
        if (nextRate > 0)
            nextRate -= Time.deltaTime;

        if (Input.GetButton("Fire1") && nextRate <= 0 && ammo > 0 && !canShoot)
        {
            anim.SetTrigger(SHOOT);
            nextRate = 1 / fireRate;
            ammo--;
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0)
            StartCoroutine(ReLoad());

        if (recoiling)
            Recoil();

        if (recovering)
            Recovering();
    }

    private IEnumerator ReLoad()
    {
        canShoot = true;
        anim.SetTrigger(RELOAD);
        if (mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
        yield return new WaitForSeconds(3f);
        canShoot = false;
    }


    public void Fire()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.Instantiate(hitVfx.name, hit.point, Quaternion.identity);
            if(hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }

    private void Recoil()
    {
        Vector3 finalPos = new Vector3(originalRecoil.x, originalRecoil.y + recoilUp, originalRecoil.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoilLength); 

        if (transform.position == finalPos)
        {
            recoiling = false;
            recovering = true;
        }
    }

    private void Recovering()
    {
        Vector3 finalPos = originalRecoil;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPos, ref recoilVelocity, recoverLength);

        if (transform.position == finalPos)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
