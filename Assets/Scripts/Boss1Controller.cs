using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : EnemyController {
    public int autoDestructAfterSeconds;
    float totalElapsedTime = 0;
    protected override void ShootPattern() {
        totalElapsedTime += Time.deltaTime;
        if (elapsedTime >= autoDestructAfterSeconds)
            SetDamageWrapper(hp);
    }

    protected override void BeforeDestruction() {
        //level clear
        GameManager.instance.StageCompleted();
    }

    protected override void CustomSetup() {
        StartCoroutine(DisableAndEnableHitbox());
        if(GameManager.instance != null)
            GameManager.instance.StartBossMusic();
    }

    IEnumerator DisableAndEnableHitbox() {
        Transform hitbox = transform.GetChild(1);
        hitbox.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        hitbox.gameObject.SetActive(true);

    }
}
