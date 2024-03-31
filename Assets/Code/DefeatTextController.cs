using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatTextController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textMesh;
    string [] introLines = {
        ">>> You died...\n\n",
        ">>> mission failed...\n"
    };

    void Start() {
        StartCoroutine(UpdateTextWithDelayEffect());
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator UpdateTextWithDelayEffect() {
        for(int line = 0; line < introLines.Length; line++) {
            for (int character = 0; character < introLines[line].Length; character++) {
                textMesh.text += introLines[line][character];
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
