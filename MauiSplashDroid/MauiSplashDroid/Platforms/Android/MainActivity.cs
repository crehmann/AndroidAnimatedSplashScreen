using Android.Animation;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views.Animations;
using AndroidX.Core.SplashScreen;
using MauiSplashDroid.ViewModel;
using static Android.Views.ViewTreeObserver;
using static AndroidX.Core.SplashScreen.SplashScreen;

namespace MauiSplashDroid;

[Activity(Theme = "@style/Theme.App.Starting", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity, IOnExitAnimationListener, IOnPreDrawListener
{
    private readonly SplashScreenViewModel _viewModel = new();
    private Android.Views.View contentView;


    protected override void OnCreate(Bundle savedInstanceState)
    {
        var splash = InstallSplashScreen(this);
        base.OnCreate(savedInstanceState);

        // Set up an OnPreDrawListener to the root view.
        contentView = FindViewById<Android.Views.View>(Android.Resource.Id.Content);
        contentView.ViewTreeObserver.AddOnPreDrawListener(this);

        // Add a callback that's called when the splash screen is animating to
        // the app content.
        splash.SetOnExitAnimationListener(this);
        
        _viewModel.Init();
    }

    public bool OnPreDraw()
    {
        if (_viewModel.IsReady) // Check ViewModel if ready
        {
            // The content is ready; start drawing.
            contentView.ViewTreeObserver.RemoveOnPreDrawListener(this);
            return true;
        }
        else
        {
            // The content is not ready; suspend.
            return false;
        }
    }

    public void OnSplashScreenExit(SplashScreenViewProvider splashScreenViewProvider)
    {
        var slideUp = ObjectAnimator.OfFloat(splashScreenViewProvider.View, nameof(Android.Views.View.TranslationY), 0f, - splashScreenViewProvider.View.Height);
        slideUp.SetInterpolator(new AnticipateInterpolator());
        slideUp.SetDuration(500L);

        // Call SplashScreenView.remove at the end of your custom animation.
        slideUp.AnimationEnd += (args, e) =>
        {
            splashScreenViewProvider.Remove();
        };

        // Run your animation.
        slideUp.Start();
    }
}