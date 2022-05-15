using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireDelay = 0.5f;
    [SerializeField] float range = 5;

    float timeSinceFire = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (timeSinceFire >= fireDelay)
        {
            if (!targetTransform)
            {
                targetTransform = GameManager.Instance.player.gameObject.transform;
            }
            timeSinceFire = 0.0f;
            var dir = targetTransform.position - transform.position;

            var hit = Physics2D.Raycast(transform.position, dir, range, GameManager.Instance.playerLayerMask);

            Debug.DrawRay(transform.position, dir, Color.green, 0.25f);

            if (hit.collider != null)
            {
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Instantiate(projectilePrefab, gameObject.transform.position, rotation);
            }
        }

        timeSinceFire += Time.deltaTime;
    }
}
