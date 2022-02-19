using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TurretController : EnemyController {
    GameObject player;
    public Sprite[] sprites;
    public float angle;
    float totalElapsedTime = 0;
    protected override void CustomSetup() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void ShootPattern() {
        totalElapsedTime += Time.deltaTime;
        Debug.DrawLine(transform.position, player.transform.position, Color.red);
        angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg +180;
        rend.sprite = sprites[15-(int)((angle-11.25f)/22.5f)];
        if (totalElapsedTime >= 7 && elapsedTime >= 3) {
            elapsedTime = -10;
            StartCoroutine(ShootBullets(3, angle * Mathf.Deg2Rad));
        }
    }

    IEnumerator ShootBullets(int nbullets, float angle) {
        for (int i = 0; i < nbullets; i++) {
            bullets[0] = ObjectPool.SharedInstance.GetPooledObject(bulletName);
            if (bullets[0] != null) {
                bullets[0].transform.rotation = spriteRef.rotation;
                bullets[0].transform.position = spriteRef.position - Vector3.right * Mathf.Cos(angle) * 8- Vector3.up * Mathf.Sin(angle) * 8;
                bullets[0].SetActive(true);
            }
            yield return new WaitForSeconds(.25f);
        }
        elapsedTime = 0;
    }
}
