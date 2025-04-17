using UnityEngine;
using TMPro;

public class WarheadController : MonoBehaviour {
    [SerializeField] TMP_Text countdownDisplay;
    [SerializeField] bool shouldCountDown;

    const int TIME_UNTIL_DETONATION = 120;
    const string TIME_UNTIL_DETONATION_SUB = "[Zeit zur Detonation]";

    float timeUntilDetonation = TIME_UNTIL_DETONATION;

    public void EngageWarhead() {
        shouldCountDown = true;
    }

    public void CancelWarhead() {
        shouldCountDown = false;
        timeUntilDetonation = TIME_UNTIL_DETONATION;
    }

    #region Update + related
    void Update() {
        CountDown();
        UpdateCountdownDisplay();
    }

    void CountDown() {
        if (!shouldCountDown) {
            return;
        }

        timeUntilDetonation -= Time.deltaTime;
    }

    void UpdateCountdownDisplay() {
        countdownDisplay.text = RemainingTimeToString(timeUntilDetonation);
    }
    #endregion

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