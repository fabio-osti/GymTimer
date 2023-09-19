using Plugin.Maui.Audio;

namespace GymTimer.Services;

public sealed class Ringer
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

    public async void RingFinishingBell()
    {
        if (!_runningOutPlayer.IsCompleted || !_appSettings.PlaySounds) return;
        
        var player = await _runningOutPlayer;
        player.Play();
        await Task.Delay(500);
        player.Stop();
    }

    public async void RingFinishedBell()
    {
        if (!_overPlayer.IsCompleted || !_appSettings.PlaySounds) return;
        (await _overPlayer).Play();
    }
}
