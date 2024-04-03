using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTextController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textMesh;
    string [] introLines = {
        "SOMEWHERE IN THE WOLF-359 SYSTEM\n\n",
        ">>> transmission started...\n",
        ">>> SOS beacon intercepted...\n",
        ">>> 1 - retrieve research data...\n",
        ">>> 2 - evacuate immediately...\n",
        ">>> transmission ended...",
    };

    void Start() {
        StartCoroutine(UpdateTextWithDelayEffect());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator UpdateTextWithDelayEffect() {
        for(int line = 0; line < introLines.Length; line++) {
            for (int character = 0; character < introLines[line].Length; character++) {
                textMesh.text += introLines[line][character];
                yield return new WaitForSeconds(0.03f);
            }
        }
        
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
    }
}
