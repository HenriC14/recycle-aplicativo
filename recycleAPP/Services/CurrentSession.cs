
using System;
using System.Collections.Generic;
using System.Text;

namespace recycleAPP.Services;

public static class CurrentSession
{
    public static MockUser? UsuarioLogado { get; set; }

    // Controla se as notificações já foram verificadas
    // durante o login atual do coletor.
    public static bool NotificacoesColetorVerificadas { get; set; }
        = false;
}
