using Plugin.Maui.Audio;

namespace GymTimer.Helpers;

public class Ringer
{
    private readonly Settings _appSettings;
    private readonly IAudioManager _audioManager;
    private readonly Task<IAudioPlayer> _overPlayer;
    private readonly Task<IAudioPlayer> _runningOutPlayer;

    public Ringer(Settings settings)
    {
        _audioManager = AudioManager.Current;
        _runningOutPlayer = LoadFile("runningout.mp3");
        _overPlayer = LoadFile("over.mp3");
        _appSettings = settings;
    }

    private async Task<IAudioPlayer> LoadFile(string fileName) => _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));

    public void RingFinishingBell()
    {
        if (!_runningOutPlayer.IsCompleted || !_appSettings.PlaySounds) return;
        var player = _runningOutPlayer.Result;
        player.Play();
        Task.Delay(500).ContinueWith(_ => player.Stop());
    }

    public void RingFinishedBell()
    {
        if (!_overPlayer.IsCompleted || !_appSettings.PlaySounds) return;
        _overPlayer.Result.Play();
    }
}
