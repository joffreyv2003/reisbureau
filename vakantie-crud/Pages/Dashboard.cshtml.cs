using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Data.Sqlite;
using vakantie_crud.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using Microsoft.AspNetCore.Authorization;

namespace vakantie_crud.Pages
{
    [Authorize(Roles = "admin")]
    public class DashboardModel : PageModel
    {
        private readonly SqliteConnection connection;
        public List<Bestemming> Bestemmingen { get; set; }

        public DashboardModel()
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "VakantieDatabase.db";
            connection = new SqliteConnection(connectionStringBuilder.ToString());
            Bestemmingen = new List<Bestemming>();
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

        public IActionResult OnPostUpdateDestination(int destinationId, string destinationName, string destinationDescription, string destinationImage, int destinationPrice, string modalDescription)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            // Bestemming bijwerken in de database
            command.CommandText = "UPDATE bestemmingen SET bestemmingNaam = @name, bestemmingBeschrijving = @description, afbeelding = @image, bestemmingPrijs = @price, modalBeschrijving = @modal WHERE id = @id";
            command.Parameters.AddWithValue("@name", destinationName);
            command.Parameters.AddWithValue("@description", destinationDescription);
            command.Parameters.AddWithValue("@image", destinationImage);
            command.Parameters.AddWithValue("@price", destinationPrice);
            command.Parameters.AddWithValue("@modal", modalDescription);
            command.Parameters.AddWithValue("@id", destinationId);
            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteDestination(int destinationId)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            // Bestemming verwijderen uit de database
            command.CommandText = "DELETE FROM bestemmingen WHERE id = @id";
            command.Parameters.AddWithValue("@id", destinationId);
            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage();
        }

        public IActionResult OnPostAddDestination(string destinationName, string destinationDescription, string destinationImage, int destinationPrice, string modalDescription)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            // Nieuwe bestemming toevoegen aan de database
            command.CommandText = "INSERT INTO bestemmingen (bestemmingNaam, bestemmingBeschrijving, afbeelding, bestemmingPrijs, modalBeschrijving) VALUES (@name, @description, @image, @price, @modal)";
            command.Parameters.AddWithValue("@name", destinationName);
            command.Parameters.AddWithValue("@description", destinationDescription);
            command.Parameters.AddWithValue("@image", destinationImage);
            command.Parameters.AddWithValue("@price", destinationPrice);
            command.Parameters.AddWithValue("@modal", modalDescription);
            command.ExecuteNonQuery();

            connection.Close();

            return RedirectToPage();
        }
    }
}

