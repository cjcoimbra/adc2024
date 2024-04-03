using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTextController : MonoBehaviour {

    public TextMeshProUGUI endText;
    string [] introLines = {
        "Dungeon Crawler Jam 2024...\n\n",
        ">>> MISSION COMPLETED...\n",
        ">>> thank you for playing...\n\n",
        ">>> created by...\n",
        ">>> Joaquim Coimbra...\n",
        ">>> Caio Coimbra...\n",
        ">>> Christian Coimbra...\n\n",
        ">>> transmission ended\n"
    };

    void Start() {
        StartCoroutine(UpdateTextWithDelayEffect());
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    IEnumerator UpdateTextWithDelayEffect() {
        for(int line = 0; line < introLines.Length; line++) {
            for (int character = 0; character < introLines[line].Length; character++) {
                endText.text += introLines[line][character];
                yield return new WaitForSeconds(0.03f);
            }
        }
        
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
