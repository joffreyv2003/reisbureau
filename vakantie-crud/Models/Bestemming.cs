using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vakantie_crud.Models
{
    public class Bestemming
    {
        public int id { get; set; }
        public string bestemmingNaam { get; set; }
        public string bestemmingBeschrijving { get; set; }
        public int bestemmingPrijs { get; set; }
        public string afbeelding { get; set; }
        public string modalBeschrijving { get; set; }

        public Bestemming()
        {

        }
    }
}
