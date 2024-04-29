namespace KarpicentroWeb.Models
{
    public class UserModel
    {
        public readonly KarpicentroDB _contextDB;

        public UserModel(KarpicentroDB contextDB)
        {
            _contextDB = contextDB;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public static string Message { get; set; }
        public static string Name { get; set; }
        public static string UserType { get; set; }
        public static string Image { get; set; }

        public bool Login()
        {
            List<User> ListaUsuarios = _contextDB.User.ToList();

            foreach (var user in ListaUsuarios)
            {
                if (Email == user.Email)
                {
                    if (Password == user.Password)
                    {
                        UserType = user.UserType;

                        Image = user.Image;

                        Name = user.Name;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool Register()
        {
            if (Password == Password2)
            {
                int i = 0;

                List<User> ListaUsuarios = _contextDB.User.ToList();

                foreach (var user in ListaUsuarios)
                {
                    if (user.Email == Email)
                        break;
                    else
                        i++;
                }
                if (i == ListaUsuarios.Count)
                {
                    var u = new User[]
                        {
                            new User() {Email = Email, Password = Password, UserType = "Cliente", Name = Name, Image = "../Images/Usuarios/Usuario.png"}
                        };

                    foreach (var us in u)
                    {
                        _contextDB.User.Add(us); 
                    }
                    _contextDB.SaveChanges();

                    return true;
                }
                else
                {
                    Message = "El correo ya esta registrado";
                    return false;
                }
            }
            else
            {
                Message = "La contraseña no coincide";
                return false;
            }
        }
    }
}
