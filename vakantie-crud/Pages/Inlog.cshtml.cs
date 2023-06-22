using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;

namespace vakantie_crud.Pages
{
    public class InlogModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Voer een geldige gebruikersnaam in!")]
        public string gebruikersnaam { get; set; }

        [BindProperty, Required(ErrorMessage = "Voer een geldig wachtwoord in!")]
        public string wachtwoord { get; set; }

        [BindProperty, Required(ErrorMessage = "Bevestig uw wachtwoord!")]
        [Compare("wachtwoord", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string bevestigWachtwoord { get; set; }

        [BindProperty, Range(typeof(bool), "true", "true", ErrorMessage = "U moet akkoord gaan met de voorwaarden.")]
        public bool accepteerVoorwaarden { get; set; }

        SqliteConnection connection;

        public InlogModel()
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "VakantieDatabase.db";
            connection = new SqliteConnection(connectionStringBuilder.ToString());
        }

        public IActionResult OnGet()
        {
            string action = Request.Query["action"];
            if (action != null && action == "logout")
            {
                HttpContext.SignOutAsync();
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            wachtwoord = GetStringSha256Hash(wachtwoord);

            if (gebruikersnaam != "admin")
            {
                using (connection)
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM gebruikers WHERE naam = @naam AND wachtwoord = @wachtwoord";
                    command.Parameters.AddWithValue("@naam", gebruikersnaam);
                    command.Parameters.AddWithValue("@wachtwoord", wachtwoord);
                    SqliteDataReader gebruikerReader = command.ExecuteReader();

                    if (gebruikerReader.HasRows)
                    {
                        List<Claim> myClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, gebruikersnaam),
                            new Claim(ClaimTypes.Email, gebruikersnaam+"@bedrijf.nl"),
                            new Claim(ClaimTypes.Role, "gebruiker")
                        };

                        ClaimsIdentity myIdentity = new ClaimsIdentity(myClaims, "GebruikerClaim");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { myIdentity });
                        HttpContext.SignInAsync(userPrincipal);
                    }

                    gebruikerReader.Close();
                }

                return RedirectToPage("Index");
            }
            else
            {
                gebruikersnaam = System.Web.HttpUtility.HtmlEncode(gebruikersnaam);

                using (connection)
                {
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = $"SELECT * FROM admins WHERE naam = @gebruikersnaam AND wachtwoord = @wachtwoord";
                    command.Parameters.AddWithValue("@gebruikersnaam", gebruikersnaam);
                    command.Parameters.AddWithValue("@wachtwoord", wachtwoord);
                    SqliteDataReader adminReader = command.ExecuteReader();

                    if (adminReader.HasRows)
                    {
                        List<Claim> myClaims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, gebruikersnaam),
                            new Claim(ClaimTypes.Email, gebruikersnaam+"@bedrijf.nl"),
                            new Claim(ClaimTypes.Role, "admin")
                        };

                        ClaimsIdentity myIdentity = new ClaimsIdentity(myClaims, "AdminClaim");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { myIdentity });
                        HttpContext.SignInAsync(userPrincipal);
                    }

                    adminReader.Close();
                }
            }
            connection.Close();
            return RedirectToPage("Index");
        }

        private string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}

