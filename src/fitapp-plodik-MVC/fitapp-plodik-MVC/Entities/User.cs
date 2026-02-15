namespace fitapp_plodik_MVC.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        // doplŃující informace které bude uživatel zadávat až po registraci v ikoně jeho profilu

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }




    }
}
