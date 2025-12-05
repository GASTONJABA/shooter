using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Mode")]
    public bool shootProjectile = true;
    public GameObject bullet;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    [Header("Raycast Mode")]
    public float rayDistance = 100f;
    public float rayForce = 20f;

    private bool shootPressed;

    public Camera playerCamera;

    private Vector3 lastRayStart;
    private Vector3 lastRayEnd;
    private bool hasRay;

    private void Update()
    {
        if (shootPressed)
        {
            if (shootProjectile)
            {
                ShootProjectile();
            }
            else
            {
                ShootRay();
            }

            shootPressed = false;
        }
    }

    // ---- PROJECTILE ----
    private void ShootProjectile()
    {
        GameObject bulletObj = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody rb = bulletObj.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);
        Debug.Log("Shoot Projectile!");
    }

    // ---- RAY ----
    private void ShootRay()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance);

        if (hasHit)
        {
            Debug.Log("Ray Hit!");

            Rigidbody hitRb = hit.collider.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
                hitRb.AddForce(firePoint.forward * rayForce, ForceMode.Impulse);
                Debug.Log("Push Object!");
            }

            lastRayStart = firePoint.position;
            lastRayEnd = hit.point;
            hasRay = true;
        }
        else
        {
            lastRayStart = firePoint.position;
            lastRayEnd = firePoint.position + firePoint.forward * rayDistance;
            hasRay = true;
        }
    }

    // ---- INPUT ----
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shootPressed = true;
        }
    }

    // ---- GIZMOS ----
    private void OnDrawGizmos()
    {
        if (!hasRay) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(lastRayStart, lastRayEnd);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(lastRayEnd, 0.1f);
    }
}
