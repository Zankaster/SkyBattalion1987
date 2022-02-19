using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IShip {
    public int hp;
    public int score;
    public int powerUpDropPercent;
    protected int currentHp;
    public float movementTime;
    protected float elapsedTime;
    public string bulletName = "EnemyBullet";
    public string explosionName = "Explosion1";
    protected GameObject instantiatedBullet;
    protected int shotsFired;
    protected Transform spriteRef;
    public Animator anim;

    protected SpriteRenderer rend;
    protected bool flashing;
    protected float flashTimer;
    protected GameObject[] bullets = new GameObject[10];

    void Awake() {
        rend = transform.Find("Enemy").GetComponent<SpriteRenderer>();
        if (spriteRef == null) {
            spriteRef = transform.Find("Enemy").transform;
        }
    }

    private void OnEnable() {
        elapsedTime = 0;
        currentHp = hp;
        shotsFired = 0;
        rend.material.SetFloat("_Flash", 0f);
        CustomSetup();
    }

    public void SetDamage(int damage) {
        currentHp -= damage;
        if(currentHp <= 0) {
            //explosion
            GameManager.instance.AddScore(score);
            GameObject explosion = ObjectPool.SharedInstance.GetPooledObject(explosionName);
            if (explosion != null) {
                explosion.transform.position = spriteRef.position;
                explosion.SetActive(true);
            }
            BeforeDestruction();
            gameObject.SetActive(false);
            if(Random.Range(1, 100) <= powerUpDropPercent) {
                GameObject powerUp = ObjectPool.SharedInstance.GetPooledObject("PowerUp");
                if (powerUp != null) {
                    powerUp.transform.position = spriteRef.position;
                    powerUp.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
        rend.material.SetFloat("_Flash", 1f);
        flashTimer = 0;
        flashing = true;
    }

    protected void SetDamageWrapper(int damage) {
        SetDamage(damage);
    }

    private void Update() {
        if (GameManager.instance.GetGameStatus() == GameStatus.Continue) {
            anim.enabled = false;
            return;
        }
        anim.enabled = true;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= movementTime)
            transform.gameObject.SetActive(false);
        ShootPattern();

        if (flashing) {
            flashTimer += Time.deltaTime;
            if (flashTimer >= .05f) {
                flashTimer = 0;
                flashing = false;
                rend.material.SetFloat("_Flash", 0f);
            }
        }
    }

    protected virtual void ShootPattern() {

    }

    protected virtual void CustomSetup() {

    }

    protected virtual void BeforeDestruction() {

    }
}
