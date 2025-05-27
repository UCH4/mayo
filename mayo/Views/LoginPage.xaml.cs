// Views/LoginPage.xaml.cs
using Plugin.Firebase.Auth; // Importa el namespace para Firebase Authentication

namespace mayo // <-- ¡Importante: Asegúrate que este namespace sea 'mayo' para tu proyecto!
{
    public partial class LoginPage : ContentPage
    {
        private readonly IFirebaseAuth _firebaseAuth; // Instancia del servicio de autenticación

        public LoginPage()
        {
            InitializeComponent();
            // Obtenemos la instancia de IFirebaseAuth del contenedor de servicios
            _firebaseAuth = ServiceHelper.GetService<IFirebaseAuth>();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                StatusLabel.Text = "Por favor, ingresa correo y contraseña.";
                return;
            }

            try
            {
                // Intenta registrar al usuario con correo y contraseña
                var result = await _firebaseAuth.CreateUserAsync(email, password);
                StatusLabel.Text = $"Registro exitoso para: {result.User.Email}";
                // Aquí podrías navegar a la siguiente página o actualizar la UI
            }
            catch (FirebaseAuthException ex)
            {
                StatusLabel.Text = $"Error de registro: {ex.Reason}";
                // Puedes obtener más detalles del error en ex.Message o ex.Error.Code
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error inesperado: {ex.Message}";
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                StatusLabel.Text = "Por favor, ingresa correo y contraseña.";
                return;
            }

            try
            {
                // Intenta iniciar sesión con correo y contraseña
                var result = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                StatusLabel.Text = $"Inicio de sesión exitoso para: {result.User.Email}";
                // Aquí podrías navegar a la siguiente página o actualizar la UI
            }
            catch (FirebaseAuthException ex)
            {
                StatusLabel.Text = $"Error de inicio de sesión: {ex.Reason}";
                // Puedes obtener más detalles del error en ex.Message o ex.Error.Code
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error inesperado: {ex.Message}";
            }
        }
    }

    // Helper para obtener servicios (temporal, idealmente usar inyección de dependencias)
    public static class ServiceHelper
    {
        public static TService GetService<TService>() => Current.GetService<TService>();

        public static IServiceProvider Current =>
#if WINDOWS
            MauiWinUIApplication.Current.Services;
#elif ANDROID
            MauiApplication.Current.Services;
#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
            null; // Esto debería ser manejado adecuadamente en un entorno de producción
#endif
    }
}