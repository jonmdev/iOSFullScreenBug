using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using Microsoft.Extensions.Logging;

namespace iOSFullScreenBug {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

#if IOS

            builder.ConfigureLifecycleEvents(events => {
                
                events.AddiOS(iOs => {
                    iOs.OnActivated(activated => {
#pragma warning disable CA1422 // Validate platform compatibility
                        activated.SetStatusBarHidden(true, UIKit.UIStatusBarAnimation.None);
#pragma warning restore CA1422 // Validate platform compatibility
                    });
                    
                });
            });
#endif

            return builder.Build();
        }
    }
}
