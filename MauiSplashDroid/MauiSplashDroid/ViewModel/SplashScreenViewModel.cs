namespace MauiSplashDroid.ViewModel
{
    internal class SplashScreenViewModel
    {

        public async void Init()
        {
            await Task.Delay(400); // For demonstrations
            IsReady = true;
        }

        public bool IsReady { get; private set; }
    }
}
