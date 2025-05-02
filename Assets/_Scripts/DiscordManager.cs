using UnityEngine;

public class DiscordManager : MonoBehaviour {
    public static DiscordManager Singleton { get; private set; }

    const long GAME_ID = 933014456802344961;

    Discord.Discord discord;
    long time;

    [Header("Startup Activity")]
    [SerializeField] string details;
    [SerializeField] string state;
    [SerializeField] string largeImage;

    void Awake() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        discord = new Discord.Discord(GAME_ID, (ulong) Discord.CreateFlags.NoRequireDiscord);
        Debug.Log($"[{nameof(DiscordManager)}] Initialised Discord");

        UpdateTime();
        SetStartupActivity();
    }

    void Update() {
        try {
            discord.RunCallbacks();
        } catch {
            Destroy(gameObject);
        }
    }

    public void SetStartupActivity() {
        UpdateActivity(details, state, largeImage, false);
    }

    public void SetDetonationActivity() {
        var displayedTime = Mathf.Floor(Mathf.Max(
            WarheadController.Singleton.GetTimeUntilDetonation(),
            0f));

        try {
            var activityManager = discord.GetActivityManager();
            var newActivity = new Discord.Activity {
                Details = "Detonation in Progress", // Upper Text
                State = $"In T-{displayedTime}", // Lower Text
                Timestamps = {
                    Start = time
                }
            };

            activityManager.UpdateActivity(newActivity, (res) => {
                if (res != Discord.Result.Ok) {
                    Debug.LogWarning($"[{nameof(DiscordManager)}] Couldn't update activity! Error: {res}");
                }
            });
        } catch {
            Destroy(gameObject);
        }
    }

    public void UpdateActivity(string details, string state, string largeImage, bool updateTime) {
        if (updateTime) {
            UpdateTime();
        }

        try {
            var activityManager = discord.GetActivityManager();
            var newActivity = new Discord.Activity {
                Details = details, // Upper Text
                State = state, // Lower Text
                Assets = {
                    LargeImage = largeImage.ToLower(),
                    LargeText = "discord.gg/c7cb5MRbth",
                },
                Timestamps = {
                    Start = time
                }
            };

            activityManager.UpdateActivity(newActivity, (res) => {
                if (res != Discord.Result.Ok) {
                    Debug.LogWarning($"[{nameof(DiscordManager)}] Couldn't update activity! Error: {res}");
                }
            });
        } catch {
            Destroy(gameObject);
        }
    }

    void UpdateTime() {
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void OnApplicationQuit() {
        try {
            discord.Dispose();
            Debug.Log($"[{nameof(DiscordManager)}] Shutting down...");
        } catch {
            Destroy(gameObject);
        }
    }
}