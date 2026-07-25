using UnityEngine;

public class shooting : MonoBehaviour
{
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots = 1f;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shootin = true, readyToShoot = true, reloading = false;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shootin = Input.GetKeyDown(KeyCode.Space);
        else shootin = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shootin && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shootin && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Fire straight along the camera's forward direction
        Vector3 directionWithoutSpread = fpsCam.transform.forward;
        Debug.Log("Camera forward: " + directionWithoutSpread + " | Camera rotation: " + fpsCam.transform.rotation.eulerAngles + " | Camera name: " + fpsCam.gameObject.name);

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Apply spread as a small rotation offset
        Quaternion spreadRotation = Quaternion.Euler(y, x, 0);
        Vector3 directionWithSpread = spreadRotation * directionWithoutSpread;

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        Debug.Log("AttackPoint position: " + attackPoint.position + " | AttackPoint forward: " + attackPoint.forward);

        //Ignore collision between the bullet's colliders and the mortar's Box Collider
        BoxCollider mortarCol = GetComponent<BoxCollider>();
        if (mortarCol != null)
        {
            Collider[] bulletColliders = currentBullet.GetComponentsInChildren<Collider>();
            foreach (Collider bulletCol in bulletColliders)
            {
                Physics.IgnoreCollision(bulletCol, mortarCol);
            }
        }

        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        Debug.Log("Bullet forward after set: " + currentBullet.transform.forward + " | Bullet rotation: " + currentBullet.transform.rotation.eulerAngles);

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}