using Microsoft.Extensions.Logging;
using Plugin.Firebase.Crashlytics; // Necesario para la configuración de Crashlytics (opcional pero útil)
using Plugin.Firebase.Auth;        // Necesario para la autenticación
//using Plugin.Firebase.Shared;      // Funcionalidades compartidas del plugin
using Microsoft.Maui.LifecycleEvents; // Necesario para configurar eventos de ciclo de vida

namespace mayo // <-- ¡Importante: Asegúrate que este namespace sea 'mayo' para tu proyecto!
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
                })
                // Llama a nuestro método de configuración de Firebase
                .ConfigureFirebaseServices();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        // Nuevo método de extensión para configurar los servicios de Firebase
        public static MauiAppBuilder ConfigureFirebaseServices(this MauiAppBuilder builder)
        {
            // Configura los eventos del ciclo de vida para la inicialización de Firebase
            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
                // Configuración específica para iOS
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                    CrossFirebase.Current.Configure(app, launchOptions);
                    return false; // Devuelve false para que el sistema operativo maneje el resto
                }));
#elif ANDROID
                // Configuración específica para Android
                events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                    CrossFirebase.Current.ConfigureForCrashlytics(); // Configura Firebase para Android (incluye Crashlytics)
                }));
#endif
            });

            // Registra el servicio de autenticación de Firebase en el contenedor de inyección de dependencias
            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);

            return builder;
        }
    }
}