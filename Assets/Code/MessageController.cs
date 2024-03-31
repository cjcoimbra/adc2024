using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    // Start is called before the first frame update
    string[] messages = {"", "", ""};
    public TextMeshProUGUI textMesh;

    public void ReceiveMessage(string newMessage) {
        bool foundVacant = false;
        for (int i = 0; i < 3; i++) {
            if (messages[i] == "") {
                messages[i] = newMessage;
                foundVacant = true;
                break;
            }
        }

        if (!foundVacant) {
            messages[0] = messages[1];
            messages[1] = messages[2];
            messages[2] = newMessage;
        }
        string rearrangedMessages = "";
        foreach (string message in messages) {
            if (message == "") continue;
            rearrangedMessages += "> " + message + "\n"; 
        }
        textMesh.text = rearrangedMessages;
    }
}
