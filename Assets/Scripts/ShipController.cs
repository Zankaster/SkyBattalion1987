using UnityEngine;

public class ShipController : MonoBehaviour, IShip {
    public float moveSpeed = 1f;
    public float acceleration = 3f;
    public float deceleration = 5f;
    public float stopThreshold = 0.1f;
    public float collisionOverlapThreshold = 0.01f;
    public float shotsPerSecond = 10;
    public LayerMask collisionMask;
    public Vector2 input;
    public Vector2 velocity;
    public int lives = 2;
    BoxCollider2D boxCollider;
    private float shotTimer = 0f;
    Animator animator;
    Vector3 simulatedPosition;
    bool respawning;
    bool invincibility;
    float invincibilityDuration;
    float invincibilityElapsed;
    float respawnDuration;
    float respawnElapsed;
    Vector3 respawnPoint;
    int powerUpLevel;
    GameObject deathExplosion;

    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        respawning = false;
        invincibility = false;
        respawnElapsed = 0;
        invincibilityElapsed = 0;
        invincibilityDuration = 3;
        respawnPoint = new Vector3(-256, 0, 0);
        respawnDuration = 1;
        powerUpLevel = 0;
    }

    void Update() {
        if (GameManager.instance.GetGameStatus() != GameStatus.Playing)
            return;
        HandleRespawn();
        if (respawning)
            return;
        GetMovementInput();
        GetShootInput();
        CalculateVelocity();
        Move();
        CollisionCheck();
    }

    private void HandleRespawn() {
        if (respawning) {
            respawnElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(respawnPoint, respawnPoint + Vector3.right * 64, Mathf.Clamp(respawnElapsed / respawnDuration, 0, 1));
            if (respawnElapsed >= respawnDuration) {
                respawnElapsed = 0;
                respawning = false;
            }
        }
        else if (invincibility) {
            invincibilityElapsed += Time.deltaTime;
            if (invincibilityElapsed >= invincibilityDuration) {
                invincibilityElapsed = 0;
                invincibility = false;
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
    }

    void Move() {
        simulatedPosition = transform.position + new Vector3(velocity.x, velocity.y, 0) * moveSpeed * Time.deltaTime;
        transform.position = simulatedPosition;
    }

    void CalculateVelocity() {
        velocity += acceleration * input * Time.deltaTime;

        if (input.x == 0 && velocity.x != 0) {
            if (Mathf.Sign(velocity.x) != Mathf.Sign(velocity.x - deceleration * Mathf.Sign(velocity.x) * Time.deltaTime))
                velocity.x = 0;
            else
                velocity.x -= deceleration * Mathf.Sign(velocity.x) * Time.deltaTime;
        }

        if (input.y == 0 && velocity.y != 0) {
            if (Mathf.Sign(velocity.y) != Mathf.Sign(velocity.y - deceleration * Mathf.Sign(velocity.y) * Time.deltaTime))
                velocity.y = 0;
            else
                velocity.y -= deceleration * Mathf.Sign(velocity.y) * Time.deltaTime;
        }

        if (Mathf.Abs(velocity.x) <= stopThreshold && input.x == 0)
            velocity.x = 0;
        if (Mathf.Abs(velocity.y) <= stopThreshold && input.y == 0)
            velocity.y = 0;

        if (velocity.magnitude > 1)
            velocity.Normalize();
    }

    void GetMovementInput() {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetInteger("Tilt", (int)input.y);
        if (input.magnitude > 1)
            input.Normalize();
    }

    void GetShootInput() {
        shotTimer -= Time.deltaTime;
        if (Input.GetButton("Fire1") && shotTimer <= 0) {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullet" + powerUpLevel);
            if(bullet != null) {
                bullet.transform.position = transform.position + Vector3.right*16;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
                shotTimer = -shotTimer +  1/shotsPerSecond;
            }
        }
        else if(shotTimer < 0)
            shotTimer = 0f;
            
    }

    public float GetShipHorizontalSpeed() {
        return input.x * moveSpeed;
    }

    void CollisionCheck() {
        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0, collisionMask);

        foreach (Collider2D hit in hits) {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;
            if(hit.tag == "PowerUp") {
                powerUpLevel = Mathf.Clamp(powerUpLevel + 1, 0, 2);
                hit.gameObject.GetComponent<PowerupController>().ConsumePowerup();
                continue;
            }

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped) {
                if (Mathf.Abs(colliderDistance.pointA.x - colliderDistance.pointB.x) > collisionOverlapThreshold)
                    velocity.x = 0;
                if (Mathf.Abs(colliderDistance.pointA.y - colliderDistance.pointB.y) > collisionOverlapThreshold)
                    velocity.y = 0;
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB, Space.World);
            }
        }
    }

    void IShip.SetDamage(int damage) {
        if (respawning || invincibility || GameManager.instance.GetGameStatus() != GameStatus.Playing)
            return;
        deathExplosion = ObjectPool.SharedInstance.GetPooledObject("MegaExplosion");
        if(deathExplosion != null) {
            deathExplosion.transform.position = transform.position;
            deathExplosion.SetActive(true);
        }
        GameManager.instance.PlayerDied();
    }

    public void Respawn() {
        powerUpLevel = 0;
        respawning = true;
        invincibility = true;
        gameObject.layer = LayerMask.NameToLayer("Default");
        transform.position = respawnPoint;
    }

    public void StayDead() {
        transform.position = respawnPoint - Vector3.right*64;
    }
}
