using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using System.Diagnostics;

namespace iOSFullScreenBug {
    public partial class App : Microsoft.Maui.Controls.Application {
        public App() {

            //https://learn.microsoft.com/en-us/dotnet/maui/ios/platform-specifics/page-safe-area-layout?view=net-maui-8.0
            //https://learn.microsoft.com/en-us/dotnet/api/microsoft.maui.controls.layout.ignoresafearea?view=net-maui-8.0#microsoft-maui-controls-layout-ignoresafearea

            ContentPage mainPage = new();
            mainPage.On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);
            mainPage.On<iOS>().SetUseSafeArea(true);
            mainPage.BackgroundColor = Colors.Red;
            MainPage = mainPage;

            AbsoluteLayout abs = new();
            abs.IgnoreSafeArea = true;
            abs.BackgroundColor = Colors.Blue;
            mainPage.Content = abs;

            mainPage.SizeChanged += delegate {
                if (mainPage.Width > 0) {
                    abs.WidthRequest = mainPage.Width;
                    abs.HeightRequest = mainPage.Height;
                }
            };

            abs.Loaded += delegate {
#if IOS
                UIKit.UIView uiView = abs.ToPlatform(abs.Handler.MauiContext);
                var constraints = uiView.Superview.Constraints;
                Debug.WriteLine("constraints " + constraints.Count());

                //https://learn.microsoft.com/en-us/dotnet/api/uikit.nslayoutconstraint.create?view=xamarin-ios-sdk-12
                Debug.WriteLineIf(uiView.Superview == null, "SUPERVIEW IS NULL");
                var constraintTop = UIKit.NSLayoutConstraint.Create(uiView, UIKit.NSLayoutAttribute.Top, UIKit.NSLayoutRelation.Equal, uiView.Superview, UIKit.NSLayoutAttribute.Top, 1, 1);
                var constraintBot = UIKit.NSLayoutConstraint.Create(uiView, UIKit.NSLayoutAttribute.Bottom, UIKit.NSLayoutRelation.Equal, uiView.Superview, UIKit.NSLayoutAttribute.Bottom, 1, 1);

                //Debug.WriteLine("ADD CONSTRAINT");
                //uiView.Superview.AddConstraint(constraintTop);
                //uiView.Superview.AddConstraint(constraintBot);
#endif
            };

        }
        protected override Window CreateWindow(IActivationState activationState) {
            //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/windows
            Debug.WriteLine("WINDOW MADE");
            Window window = base.CreateWindow(activationState);
#if IOS
            window.HandlerChanged += delegate {
                
                UIKit.UIWindow platformWindow = (UIKit.UIWindow)window.ToPlatform(window.Handler.MauiContext);
                Debug.WriteLine("HOME INDICATOR ALLOWED HIDDEN " + platformWindow.RootViewController.PrefersHomeIndicatorAutoHidden); 
            };
#endif
            return window;
        }
    }
}
