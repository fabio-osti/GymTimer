using Plugin.Maui.Audio;

namespace GymTimer.Services;

public sealed class Ringer
{
    private readonly Settings _settings;
    private readonly IAudioManager _audioManager;
    private readonly Task<IAudioPlayer> _finishedPlayer;
    private readonly Task<IAudioPlayer> _finishingPlayer;

    public Ringer(Settings settings)
    {
        _audioManager = AudioManager.Current;
        _finishingPlayer = LoadFile("finishing.mp3");
        _finishedPlayer = LoadFile("finished.mp3");
        _settings = settings;
    }

    private async Task<IAudioPlayer> LoadFile(string fileName) => _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));

    public async void RingFinishingBell()
    {
        // If file isn't loaded, skip this action
        if (!_finishingPlayer.IsCompleted || !_settings.PlaySounds) return;
        
        var player = _finishingPlayer.Result;
        player.Play();
        await Task.Delay(500);
        player.Stop();
    }

    public void RingFinishedBell()
    {
        // If file isn't loaded, skip this action
        if (!_finishedPlayer.IsCompleted || !_settings.PlaySounds) return;
        _finishedPlayer.Result.Play();
    }
}
