using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber1Controller : EnemyController
{
    protected override void ShootPattern() {
        if(elapsedTime/movementTime > 0.2 && shotsFired == 0 ||
            elapsedTime / movementTime > 0.3 && shotsFired == 1 ||
            elapsedTime / movementTime > 0.4 && shotsFired == 2) {
            shotsFired++;
            bullets[0] = ObjectPool.SharedInstance.GetPooledObject(bulletName);
            if (bullets[0] != null) {
                bullets[0].transform.position = spriteRef.position - Vector3.right * 16;
                bullets[0].SetActive(true);
            }
        }
    }
}
