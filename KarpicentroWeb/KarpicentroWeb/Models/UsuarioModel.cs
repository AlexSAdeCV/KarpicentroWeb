namespace KarpicentroWeb.Models
{
    public class UsuarioModel
    {
        public readonly KarpicentroDB _contextDB;

        public UsuarioModel(KarpicentroDB contextDB)
        {
            _contextDB = contextDB;
        }

        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public static string TipoUsuario { get; set; }

        public bool Login()
        {
            List<Usuario> ListaUsuarios = _contextDB.Usuario.ToList();

            foreach (var user in ListaUsuarios)
            {
                if (Correo == user.Correo)
                    if (Contrasena == user.Contrasena)
                        TipoUsuario = user.TipoUsuario; return true;
            }

            return false;
        }
    }
}
