using Microsoft.Extensions.Logging;
using recycleAPP.Services;

namespace recycleAPP
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

            builder.Services.AddSingleton<IAuthService, MockUserService>();
            builder.Services.AddTransient<recycleAPP.Views.CadastroPage>();
            builder.Services.AddTransient<recycleAPP.Views.LoginPage>();
            builder.Services.AddTransient<recycleAPP.Views.WelcomePage>();
            builder.Services.AddTransient<recycleAPP.Views.SelecionarTipoPage>();
            builder.Services.AddTransient<recycleAPP.Views.LoginColetorPage>();
            builder.Services.AddTransient<recycleAPP.Views.CadastroColetorPage>();
            builder.Services.AddTransient<recycleAPP.Views.PerfilColetorPage>();
            builder.Services.AddTransient<recycleAPP.Views.PerfilRecicladorPage>();
            builder.Services.AddTransient<recycleAPP.Views.LojaPontosPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
