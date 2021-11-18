using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.Place
{
    public class University
    {
        public int UniversityId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Place> Places { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string homepage { get; set; }
        public string portal { get; set; }
        public string library { get; set; }
        public string map { get; set; }
    }
}