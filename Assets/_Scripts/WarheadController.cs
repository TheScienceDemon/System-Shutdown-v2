using UnityEngine;
using TMPro;

public class WarheadController : MonoBehaviour {
    [SerializeField] TMP_Text countdownDisplay;

    const int TIME_UNTIL_DETONATION = 120;
    const string TIME_UNTIL_DETONATION_SUB = "[Zeit zur Detonation]";

    [SerializeField] float temp = TIME_UNTIL_DETONATION;

    void Update() {
        temp -= Time.deltaTime;

        countdownDisplay.text = RemainingTimeToString(temp);
    }

    string RemainingTimeToString(float remainingTime) {
        if (remainingTime <= 0f) {
            return
                $"00:00:000\n" +
                $"{TIME_UNTIL_DETONATION_SUB}";
        }

        var mins = Mathf.Floor(remainingTime / 60 % 60);
        var secs = Mathf.Floor(remainingTime % 60);
        var millisecs = Mathf.Floor(remainingTime * 1000 % 1000);

        return
            $"{mins:00}:{secs:00}:{millisecs:000}\n" +
            $"{TIME_UNTIL_DETONATION_SUB}";
    }
}