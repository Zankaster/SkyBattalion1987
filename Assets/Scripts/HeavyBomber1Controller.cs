using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBomber1Controller : EnemyController {
    int shootCount;
    protected override void ShootPattern() {
        if (elapsedTime >= 4 && shootCount == 0 ||
            elapsedTime >= 6 && shootCount == 1 ||
            elapsedTime >= 8 && shootCount == 2 ||
            elapsedTime >= 10 && shootCount == 3) {
            shootCount++;
            StartCoroutine(ShootBullets(3));
        }
    }

    IEnumerator ShootBullets(int n) {
        for(int i = 0; i < n; i++) {
            bullets[0] = ObjectPool.SharedInstance.GetPooledObject(bulletName);
            if (bullets[0] != null) {
                bullets[0].transform.position = spriteRef.position - Vector3.right * 16;
                bullets[0].SetActive(true);
            }
            yield return new WaitForSeconds(.33f);
        }
    }

    protected override void CustomSetup() {
        shootCount = 0;
    }
}
