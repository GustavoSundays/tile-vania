using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyBoxCollider;

    void Start() {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Ground") {
            moveSpeed = -moveSpeed;
            FlipSprite();
        }
    }

    void FlipSprite() {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)), 1f);
    }
}
