using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Data.Sqlite;
using vakantie_crud.Models;

using Microsoft.AspNetCore.Authorization;

namespace vakantie_crud.Pages
{
    public class BestemmingenModel : PageModel
    {
        SqliteConnection connection;
        public List<Bestemming> Bestemmingen = new List<Bestemming>();

        public BestemmingenModel()
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "VakantieDatabase.db";
            connection = new SqliteConnection(connectionStringBuilder.ToString());
        }
        public void OnGet()
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            // Bestemmingen lezen
            command.CommandText = "SELECT * FROM bestemmingen";
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Bestemming bestemming = new Bestemming();
                bestemming.id = reader.GetInt32(0);
                bestemming.bestemmingNaam = reader.GetString(1);
                bestemming.bestemmingBeschrijving = reader.GetString(2);
                bestemming.bestemmingPrijs = reader.GetInt32(3);
                bestemming.afbeelding = reader.GetString(4);
                bestemming.modalBeschrijving = reader.GetString(5);

                Bestemmingen.Add(bestemming);
            }
            reader.Close();
        }
    }
}
