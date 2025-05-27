// Views/LoginPage.xaml.cs
using Plugin.Firebase.Auth; // Contiene IFirebaseAuth y IAuthResult
using Plugin.Firebase.Auth.FirebaseAuthException; // <-- ¡Este es el using CORRECTO para la excepción del plugin!
using Microsoft.Maui.Platform; // Necesario para MauiApplication.Current.Services si se usa el ServiceHelper

namespace mayo
{
    public partial class LoginPage : ContentPage
    {
        private readonly IFirebaseAuth _firebaseAuth;

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
                // CreateUserAsync devuelve un IAuthResult, que tiene una propiedad User de tipo IFirebaseUser
                var result = await _firebaseAuth.CreateUserAsync(email, password);
                StatusLabel.Text = $"Registro exitoso para: {result.User.Email}";
            }
            catch (Plugin.Firebase.Auth.FirebaseAuthException.FirebaseAuthException ex) // Referencia completa a la excepción del plugin
            {
                StatusLabel.Text = $"Error de registro: {ex.Reason}";
                // ex.Error.Code (si existe) y ex.Message pueden dar más detalles.
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
                // SignInWithEmailAndPasswordAsync también devuelve un IAuthResult
                var result = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                StatusLabel.Text = $"Inicio de sesión exitoso para: {result.User.Email}";
            }
            catch (Plugin.Firebase.Auth.FirebaseAuthException.FirebaseAuthException ex) // Referencia completa a la excepción del plugin
            {
                StatusLabel.Text = $"Error de inicio de sesión: {ex.Reason}";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error inesperado: {ex.Message}";
            }
        }
    }

    // --- SERVICE HELPER AJUSTADO PARA EVITAR WARNINGS ---
    public static class ServiceHelper
    {
        public static TService GetService<TService>()
        {
            // La mejor práctica es inyectar servicios en el constructor de páginas/ViewModels.
            // Para un ServiceHelper global, esta es una forma de acceder al IServiceProvider
            // y suprimir el warning de nulabilidad.
            var serviceProvider = MauiApplication.Current?.Services; // Usamos ?. para seguridad contra nulos
            if (serviceProvider == null)
            {
                // Manejar el caso donde el ServiceProvider no está disponible (ej. durante la inicialización temprana)
                throw new InvalidOperationException("Service Provider is not initialized.");
            }
            return serviceProvider.GetService<TService>()!; // Usamos '!' para suprimir el warning CS8603
        }
    }
}