// MauiProgram.cs
using Microsoft.Extensions.Logging;
using Plugin.Firebase.Crashlytics; // Necesario para CrossFirebase.Current.ConfigureForCrashlytics()
using Plugin.Firebase.Auth;        // Necesario para CrossFirebaseAuth.Current
using Plugin.Firebase.Shared;      // <-- ¡Vuelve a incluir este! Es necesario en la v3.1.4 para CrossFirebase
using Microsoft.Maui.LifecycleEvents; // Necesario para builder.ConfigureLifecycleEvents
namespace mayo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // --- INICIALIZACIÓN DE FIREBASE SEGÚN PLUGIN.FIREBASE (Versión 3.1.4) ---
            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Current.Configure(app, launchOptions); // CrossFirebase DEBE existir ahora
                    return false;
                }));
#elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                    CrossFirebase.Current.ConfigureForCrashlytics(); // CrossFirebase DEBE existir ahora
                }));
#endif
            });

            // Registra el servicio de autenticación de Firebase en el contenedor de inyección de dependencias
            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            // --- FIN DE LA INICIALIZACIÓN DE FIREBASE ---

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}