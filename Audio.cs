class Audio
{
    public static void PlayGreeting()
    {
        try
        {
            var player = new System.Media.SoundPlayer("WelcomeMessage.wav");
            player.Play();
        }
        catch
        {
        }
    }
}