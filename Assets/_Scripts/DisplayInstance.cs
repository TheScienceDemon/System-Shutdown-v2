using UnityEngine;
using TMPro;

public class DisplayInstance : MonoBehaviour {
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text infobox;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] TMP_Text countdownDisplay;

    // e.g.: displayIndex = 1 => Display 2
    public void SetupInstance(int displayIndex) {
        cam.targetDisplay = displayIndex;
        canvas.targetDisplay = displayIndex;
    }

    public TMP_Text Infobox {
        get => infobox;
    }

    public TMP_Text CountdownText {
        get => countdownText;
    }

    public TMP_Text CountdownDisplay {
        get => countdownDisplay;
    }
}