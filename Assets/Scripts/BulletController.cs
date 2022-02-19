using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float bulletSpeed = 10f;
    public int bulletDamage;
    BoxCollider2D boxCollider;
    public LayerMask collisionMask;
    public Vector3 bulletDirection = Vector3.right;
    public int directionAngle = 0;
    public bool leftToRight = true;
    public bool topToBottom = true;
    public bool playerDirection;
    private Transform player;
    bool hitSomething = false;
    Vector2 displacement;
    int xDirection, yDirection;
    void Awake() {
        //Debug.Log("Start bullet");
        if (playerDirection)
            player = FindObjectOfType<ShipController>().transform;
        boxCollider = GetComponent<BoxCollider2D>();
        displacement = transform.localPosition;
        xDirection = leftToRight ? 1 : -1;
        yDirection = topToBottom ? -1 : 1;
    }

    private void OnEnable() {
        transform.localPosition = displacement;
        if(playerDirection)
            bulletDirection = (player.position - transform.position).normalized;
        else
            bulletDirection = new Vector2(Mathf.Cos(directionAngle*Mathf.Deg2Rad) * xDirection, Mathf.Sin(directionAngle * Mathf.Deg2Rad)*yDirection);
    }

    private void OnDisable() {
        hitSomething = false;
    }

    void Update() {
        if (GameManager.instance.GetGameStatus() == GameStatus.Continue)
            return;

        if (DisposeIfHitSomething())
            return;
        RayCollisionCheck();
        if(!hitSomething)
            transform.position =  transform.position + bulletDirection * bulletSpeed * Time.deltaTime;
    }

    void RayCollisionCheck() {
       var hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, bulletDirection, bulletSpeed * Time.deltaTime, collisionMask);
        if(hit) {
            ColliderDistance2D colliderDistance = hit.collider.Distance(boxCollider);
            transform.Translate(colliderDistance.pointA.x - colliderDistance.pointB.x, 0, 0, Space.World);
            hitSomething = true;
            GameObject bulletHit = ObjectPool.SharedInstance.GetPooledObject("BulletHit");
            var  enemy = hit.transform.gameObject.GetComponent<IShip>();
            if (enemy != null) {
                enemy.SetDamage(bulletDamage);
            }
            else if(hit.transform.parent != null){
                enemy = hit.transform.parent.gameObject.GetComponent<IShip>();
                if (enemy != null)
                    enemy.SetDamage(bulletDamage);
            }

            if (bulletHit != null) {
                bulletHit.transform.position = transform.position +Vector3.right*24;
                bulletHit.transform.rotation = transform.rotation;
                bulletHit.SetActive(true);
            }
        }
    }

    bool DisposeIfHitSomething() {
        if (hitSomething) {
            hitSomething = false;
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
