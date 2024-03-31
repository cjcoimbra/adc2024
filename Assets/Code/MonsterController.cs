using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    private Vector3 gridPosition;
    private int hitPoints;
    public Texture normalTexture;
    public Texture hitTexture;
    public Renderer sprite;
    // Start is called before the first frame update
    private MessageController messageController;
    private AudioManager audioManager;
    private GridManager gridManager;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float movementSpeed = 4.0f;
    private int attackRating = 2;
    private int xpPrize = 5;
    private Vector3 movementNotAllowed = new Vector3(-1, -1, -1);
    private Vector3 attackParty = new Vector3(-2, -2, -2);
    private GridMovement party;
    public Texture[] animationFrames;
    private bool isAnimatingRunning;
    private int currentAnimationFrame;
    private Color originalColor;
    void Start() {
        gridPosition = transform.position;
        gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        messageController = GameObject.Find("InGameMessages").GetComponent<MessageController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        party = GameObject.Find("Party").GetComponent<GridMovement>();
        hitPoints = 8;
        isAnimatingRunning = true;
        currentAnimationFrame = 1;
        originalColor = sprite.material.color;
        StartCoroutine(AnimationLoop());
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f) {
                transform.position = targetPosition;
                gridPosition = targetPosition;
                isMoving = false;
            }
            return;
        }
    }

    IEnumerator AnimationLoop() {
        yield return new WaitForSeconds(0.1f);
        Animate();
    }
    private void Animate() {
        currentAnimationFrame++;
        if (currentAnimationFrame >= animationFrames.Length) currentAnimationFrame = 0;
        sprite.material.mainTexture = animationFrames[currentAnimationFrame];
        StartCoroutine(AnimationLoop());
    }

    private void MoveMonster() {
        if (isMoving) {
            return;
        }

        Vector3 emptyGridPosition =  gridManager.GetEmptyGridPositionAround(gridPosition);
        if (emptyGridPosition == attackParty) {
            AttackParty();
            return;
        }

        if (emptyGridPosition != movementNotAllowed) {
            targetPosition = emptyGridPosition;
            isMoving = true;
        }
    }

    private void AttackParty() {
        int hitChance = Random.RandomRange(0, 100);
        if (hitChance <= 40) {
            party.AttackedByMonster(attackRating);
        } else {
            messageController.ReceiveMessage("Enemy attacks but misses");
        }
    }

    public void ReportPartyDidMove(Vector3 partyPosition) {
        MoveMonster();
    }

    private void Die() {
        gridManager.ReportMonsterDied(gridPosition);
        audioManager.PlayEnemyDeath();
        party.GrantXP(xpPrize);
        Destroy(gameObject);
    }

    IEnumerator RestoreNormalTextureAfterHit() {
        yield return new WaitForSeconds(0.2f);
        sprite.material.color = originalColor;
        //sprite.material.mainTexture = normalTexture;
    }

    public void applyHit(int damage) {
        hitPoints -= damage;
        //sprite.material.mainTexture = hitTexture;
        sprite.material.color = Color.red;

        if (hitPoints <= 0) {
            messageController.ReceiveMessage("Monster falls dead");
            Die();
        } else {
            StartCoroutine(RestoreNormalTextureAfterHit());
        }
    }

    public void FaceParty(float partyRotation) {
        float intendedRotation = 0;
        if (partyRotation == 0) {
            intendedRotation = 180.0f;
        } else if (partyRotation == 90) {
            intendedRotation = 270.0f;
        } else if (partyRotation == 270) {
            intendedRotation = 90.0f;
        } else if (partyRotation == 180) {
            intendedRotation = 0;
        }

        gameObject.transform.eulerAngles = new Vector3(0, intendedRotation, 0);
    }
}
