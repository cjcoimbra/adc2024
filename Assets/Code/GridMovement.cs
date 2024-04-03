using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GridMovement : MonoBehaviour {

    private Vector3 gridPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private float movementSpeed = 6.0f;
    private int attackRating = 2;
    private int hitPoints = 20;
    private int maxHitPoints = 20;
    private int currentXP = 0;
    private int attackAccuracy = 70;
    private int nextXPMilestone = 10;
    public Transform cameraTransform;
    public GridManager gridManager;
    public MessageController messageController;
    public AudioManager audioManager;
    public MeshRenderer playerHit;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI xpText;
    private bool acquiredResearchBackup = false;
    void Start() {
        UpdateHP();
    }

    private void UpdateHP() {
        hpText.text = "HP " + hitPoints + "/" + maxHitPoints;
    }

    private void UpdateXP() {
        xpText.text = "XP " + currentXP + "/" + nextXPMilestone;
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

        bool didMove = MovementInput();
        if (didMove) {
            //Debug.Log("New grid position: " + gridPosition);
            UpdatePosition();
        } 
        
        if(Input.GetMouseButtonUp(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                GameObject target = hit.transform.gameObject;
                RaycastAgainstTargetGameObject(target);
            }   
        }
    }

    private bool IsInFrontOfTarget(Vector3 doorPosition) {
        if (cameraTransform.rotation.eulerAngles.y == 0) {
            if (new Vector3(gridPosition.x, gridPosition.y, gridPosition.z + 1) == doorPosition) {
                return true;
            }
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            if (new Vector3(gridPosition.x, gridPosition.y, gridPosition.z - 1) == doorPosition) {
                return true;
            }
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            if (new Vector3(gridPosition.x + 1, gridPosition.y, gridPosition.z) == doorPosition) {
                return true;
            }
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            if (new Vector3(gridPosition.x - 1, gridPosition.y, gridPosition.z) == doorPosition) {
                return true;
            }
        }

        return false;
    }

    private void UpdatePosition() {
        transform.position = gridPosition;
    }

    public void SetInitialPosition(Vector3 initialPositon) {
        gridPosition = initialPositon;
        transform.position = gridPosition;
        AdjustFOVOffset();
    }

    private bool MovementInput() {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) {
            return MoveForward();
        } else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            return MoveBackward();
        } else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            return MoveLeft();
        } else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
            return MoveRight();
        } else if (Input.GetKeyUp(KeyCode.Q)) {
            RotateLeft();
            return false;
        } else if (Input.GetKeyUp(KeyCode.E)) {
            RotateRight();
            return false;
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            //SceneManager.LoadScene("End", LoadSceneMode.Single);
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 1.0f)) {
                GameObject target = hit.transform.gameObject;
                RaycastAgainstTargetGameObject(target);
            }
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.yellow);
        } else if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.Quit();
        }
        return false;
    }

    private void RaycastAgainstTargetGameObject(GameObject target) {
        if (target.tag == "door" && IsInFrontOfTarget(target.transform.position)) {
            messageController.ReceiveMessage("Opening door - standby");
            target.GetComponent<BoxCollider>().enabled = false;        
            audioManager.PlayDoorOpen();
            target.GetComponent<DoorController>().RequestToggleDoor(true, cameraTransform.rotation.eulerAngles.y, target.transform.position);
            gridManager.ReportPartyTurnOver(gridPosition);
        } else if (target.tag == "enemy" && IsInFrontOfTarget(target.transform.position)) {
            int hitChance = Random.RandomRange(0, 100);
            if (hitChance <= attackAccuracy) {
                messageController.ReceiveMessage("Attacked enemy for " + attackRating + " hit points");
                target.GetComponent<MonsterController>().applyHit(attackRating);
            } else {
                messageController.ReceiveMessage("Your attack missed");
            }
            audioManager.PlayHit();
            gridManager.ReportPartyTurnOver(gridPosition);
        } else if (target.tag == "medikit") {
            UseMedkit();
            target.SetActive(false);
        } else if (target.tag == "terminal" && !acquiredResearchBackup) {
            audioManager.PlayTerminal();
            messageController.ReceiveMessage("Research backup files acquired");
            messageController.ReceiveMessage("Proceed to extraction");
            acquiredResearchBackup = true;
        }
    }

    private bool MoveIfPassableTo(Vector3 desiredPosition) {
        if (isMoving) {
            return false;
        }
        TargetGridPosition targetGridPosition = gridManager.IsPassable(desiredPosition);
        if (targetGridPosition.isPassable) {
            targetPosition = desiredPosition;
            isMoving = true;
            audioManager.PlayFootsteps();
            gridManager.ReportPartyTurnOver(targetPosition);
            if (targetGridPosition.code == 1) CheckFinalObjective();
            return true;
        } else {
            messageController.ReceiveMessage("Not passable");
            audioManager.PlayNotPassable();
            gridManager.ReportPartyTurnOver(gridPosition);
            return false;
        }
    }

    private void CheckFinalObjective() {
        if (acquiredResearchBackup) {
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }

    private bool MoveForward() {
        Vector3 desiredPosition = new Vector3();
        if (cameraTransform.rotation.eulerAngles.y == 0) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z + 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z - 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            desiredPosition = new Vector3(gridPosition.x + 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            desiredPosition = new Vector3(gridPosition.x - 1, gridPosition.y, gridPosition.z);
        }

        return MoveIfPassableTo(desiredPosition);
    }

    private bool MoveBackward() {
        Vector3 desiredPosition = new Vector3();
        if (cameraTransform.rotation.y == 0) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z - 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z + 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            desiredPosition = new Vector3(gridPosition.x - 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            desiredPosition = new Vector3(gridPosition.x + 1, gridPosition.y, gridPosition.z);
        }

        return MoveIfPassableTo(desiredPosition);
    }
    private bool MoveLeft() {
        Vector3 desiredPosition = new Vector3();
        if (cameraTransform.rotation.eulerAngles.y == 0) {
            desiredPosition = new Vector3(gridPosition.x - 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            desiredPosition = new Vector3(gridPosition.x + 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z + 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z - 1);
        }

        return MoveIfPassableTo(desiredPosition);
    }

    private bool MoveRight() {
        Vector3 desiredPosition = new Vector3();
        if (cameraTransform.rotation.eulerAngles.y == 0) {
            desiredPosition = new Vector3(gridPosition.x + 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            desiredPosition = new Vector3(gridPosition.x - 1, gridPosition.y, gridPosition.z);
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z - 1);
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            desiredPosition = new Vector3(gridPosition.x, gridPosition.y, gridPosition.z + 1);
        }

        return MoveIfPassableTo(desiredPosition);
    }

    private void RotateLeft() {
        cameraTransform.Rotate(0, -90, 0);
        AdjustFOVOffset();
        gridManager.ForceMonstersToFaceParty(cameraTransform.rotation.eulerAngles.y);
    }

    private void RotateRight() {
        cameraTransform.Rotate(0, 90, 0);
        AdjustFOVOffset();
        gridManager.ForceMonstersToFaceParty(cameraTransform.rotation.eulerAngles.y);
    }

    private void AdjustFOVOffset() {
        if (cameraTransform.rotation.eulerAngles.y == 0) {
            cameraTransform.localPosition = new Vector3(0, 0, -0.3f);
        } else if (cameraTransform.rotation.eulerAngles.y == 180) {
            cameraTransform.localPosition = new Vector3(0, 0, 0.3f);
        } else if (cameraTransform.rotation.eulerAngles.y == 90) {
            cameraTransform.localPosition = new Vector3(-0.3f, 0, 0);
        } else if (cameraTransform.rotation.eulerAngles.y == 270) {
            cameraTransform.localPosition = new Vector3(0.3f, 0, 0);
        }
    }

    public void AttackedByMonster(int attackRating) {
        hitPoints -= attackRating;
        if (hitPoints < 0) hitPoints = 0;
        UpdateHP();
        messageController.ReceiveMessage("Enemy attack inflicted " + attackRating + " damage");
        audioManager.PlayHit();
        playerHit.enabled = true;
        StartCoroutine(RemovePlayerHit());
        if (hitPoints <= 0) {
            Die();
        }
    }

    public void GrantXP(int xpPrize) {
        currentXP += xpPrize;
        if (currentXP >= nextXPMilestone) {
            LevelUp();
        }
        UpdateXP();
    }

    private void UseMedkit() {
        audioManager.PlayMedkit();
        int restoredHP = Random.Range(5, 21);
        hitPoints += restoredHP;
        if (hitPoints >= maxHitPoints) {
            hitPoints = maxHitPoints;
            messageController.ReceiveMessage("Medkit - HP fully restored");
        } else {
            messageController.ReceiveMessage("Medkit - " + restoredHP + " HP restored");
        }
        UpdateHP();
    }

    private void LevelUp() {
        nextXPMilestone += (int)(nextXPMilestone * 50/100) + nextXPMilestone;
        maxHitPoints += (int)(maxHitPoints * 20/100);
        attackRating += 1;
        attackAccuracy += 4;
        if (attackAccuracy >= 100) attackAccuracy = 99;
        UpdateHP();
        messageController.ReceiveMessage("Level up");
    }

    private void Die() {
        messageController.ReceiveMessage("You died. Mission has failed.");
        StartCoroutine(RestartGame());
    }

    IEnumerator RemovePlayerHit() {
        yield return new WaitForSeconds(0.2f);
        playerHit.enabled = false;
    }
    IEnumerator RestartGame() {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Defeat", LoadSceneMode.Single);
    }
}
