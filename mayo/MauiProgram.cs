// MauiProgram.cs
using Microsoft.Extensions.Logging;
using Plugin.Firebase.Auth;
using Microsoft.Maui.LifecycleEvents; // Asegúrate que esto esté si lo usas en el ciclo de vida

namespace mayo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp() // <-- ¡Esta es la sintaxis correcta!
        { // <-- ¡Asegúrate de que esta llave de apertura esté aquí!
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseFirebaseAuthentication(); // Esto inicializa Firebase Auth por ti

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            return builder.Build();
        } // <-- ¡Asegúrate de que esta llave de cierre esté aquí!
    }
}