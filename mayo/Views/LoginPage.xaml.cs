// Views/LoginPage.xaml.cs
using Plugin.Firebase.Auth; // Contiene IFirebaseAuth y IAuthResult
using Plugin.Firebase.Auth.FirebaseAuthException; // <-- ¡Este es el using CORRECTO para la excepción!
using Microsoft.Maui.Platform; // Necesario para MauiApplication.Current.Services si se usa el ServiceHelper

namespace mayo
{
    public partial class LoginPage : ContentPage
    {
        private readonly IFirebaseAuth _firebaseAuth;

        public LoginPage()
        {
            InitializeComponent();
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
                var result = await _firebaseAuth.CreateUserAsync(email, password);
                StatusLabel.Text = $"Registro exitoso para: {result.User.Email}";
            }
            catch (Plugin.Firebase.Auth.FirebaseAuthException.FirebaseAuthException ex) // <-- Referencia COMPLETA
            {
                StatusLabel.Text = $"Error de registro: {ex.Reason}";
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
                var result = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
                StatusLabel.Text = $"Inicio de sesión exitoso para: {result.User.Email}";
            }
            catch (Plugin.Firebase.Auth.FirebaseAuthException.FirebaseAuthException ex) // <-- Referencia COMPLETA
            {
                StatusLabel.Text = $"Error de inicio de sesión: {ex.Reason}";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Error inesperado: {ex.Message}";
            }
        }
    }

    public static class ServiceHelper
    {
        public static TService GetService<TService>()
        {
            var serviceProvider = MauiApplication.Current?.Services;
            if (serviceProvider == null)
            {
                throw new InvalidOperationException("Service Provider is not initialized.");
            }
            return serviceProvider.GetService<TService>()!;
        }
    }
}