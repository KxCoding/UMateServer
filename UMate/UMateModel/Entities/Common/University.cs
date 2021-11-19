using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using UMateModel.Entities.Place;

namespace UMateModel.Entities.Common
{
    public class University
    {
        public int UniversityId { get; set; }

        public string Name { get; set; }

        public string Homepage { get; set; }
        public string Portal { get; set; }
        public string Library { get; set; }
        public string Map { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [JsonIgnore]
        public List<Place.Place> Places { get; set; }
    }
}