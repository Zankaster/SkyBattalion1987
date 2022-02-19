using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTurretController : EnemyController {
    float totalElapsedTime = 0;
    protected override void ShootPattern() {
        totalElapsedTime += Time.deltaTime;
        if (totalElapsedTime >= 7 && elapsedTime >= 3) {
            elapsedTime = -10;
            StartCoroutine(ShootBullets(3));
        }
    }

    IEnumerator ShootBullets(int nbullets) {
        for(int i = 0; i < nbullets; i++) {
                bullets[0] = ObjectPool.SharedInstance.GetPooledObject(bulletName);
            if (bullets[0] != null) {
                bullets[0].transform.position = spriteRef.position - Vector3.right * 16;
                //bullets[0].transform.rotation = spriteRef.rotation;
                bullets[0].SetActive(true);
            }
            yield return new WaitForSeconds(.25f);
        }
        elapsedTime = 0;
    }
}
