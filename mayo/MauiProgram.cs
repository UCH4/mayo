// MauiProgram.cs
using Microsoft.Extensions.Logging;
using Plugin.Firebase.Crashlytics; // Necesario para CrossFirebase.Current.ConfigureForCrashlytics()
using Plugin.Firebase.Auth;        // Necesario para CrossFirebaseAuth.Current
using Plugin.Firebase.Shared;      // <-- ¡MUY IMPORTANTE! Esta es la que contiene CrossFirebase
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

            // --- INICIALIZACIÓN DE FIREBASE SEGÚN PLUGIN.FIREBASE ---
            builder.ConfigureLifecycleEvents(events =>
            {
                #if IOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Current.Configure(app, launchOptions);
                    return false;
                }));
                #elif ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                    // Para Android, la documentación de Plugin.Firebase sugiere usar
                    // ConfigureForCrashlytics para inicializar Firebase globalmente
                    // incluso si no usas Crashlytics directamente.
                    CrossFirebase.Current.ConfigureForCrashlytics();
                }));
                #endif
            });

            // Registra el servicio de autenticación de Firebase en el contenedor de inyección de dependencias
            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            // --- FIN INICIALIZACIÓN DE FIREBASE ---

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            return builder.Build();
        }
    }
}