
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform doorTransform;
    private bool isOpening = false;
    private float speed = 20.0f;
    private float animationStep = 0.1f;
    private float currentCameraOrientationAngle = 0;
    private Vector3 doorGridPosition;
    void Start() {
        doorTransform = gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update() {
        if (isOpening) {
            if (currentCameraOrientationAngle == 0) {
                doorTransform.Translate(new Vector3(animationStep, 0, 0) * speed * Time.deltaTime);
                StopAnimationIfFinished(doorTransform.localPosition.x);
            } else if (currentCameraOrientationAngle == 180) {
                doorTransform.Translate(new Vector3(animationStep, 0, 0) * speed * Time.deltaTime);
                StopAnimationIfFinished(doorTransform.localPosition.x);
            } else if (currentCameraOrientationAngle == 90) {
                doorTransform.Translate(new Vector3(0, 0, animationStep) * speed * Time.deltaTime);
                StopAnimationIfFinished(doorTransform.localPosition.z);
            } else if (currentCameraOrientationAngle == 270) {
                doorTransform.Translate(new Vector3(0, 0, animationStep) * speed * Time.deltaTime);
                StopAnimationIfFinished(doorTransform.localPosition.z);
            }
        }
    }

    private void StopAnimationIfFinished(float pivot) {
        if (pivot >= 1.0f) {
            isOpening = false;
            GameObject.Find("GridManager").GetComponent<GridManager>().ReportOpenedDoor(doorGridPosition);
        }
    }
    public void RequestToggleDoor(bool toggleRequest, float cameraOrientationAngle, Vector3 doorGridPosition) {
        currentCameraOrientationAngle = cameraOrientationAngle;
        isOpening = toggleRequest;
        this.doorGridPosition = doorGridPosition;
    }
}
