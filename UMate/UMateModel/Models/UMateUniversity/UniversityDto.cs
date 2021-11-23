using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UMateModel.Models;
using UMateModel.Entities.Place;
using UMateModel.Entities.Common;

namespace UMateModel.Models.UMatePlace
{
    public class UniversityDto
    {
        public int UniversityId { get; set; }

        public string Name { get; set; }

        public string Homepage { get; set; }
        public string Portal { get; set; }
        public string Library { get; set; }
        public string Map { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public UniversityDto(University university)
        {
            UniversityId = university.UniversityId;
            Name = university.Name;

            Homepage = university.Homepage;
            Portal = university.Portal;
            Library = university.Library;
            Map = university.Map;

            Latitude = university.Latitude;
            Longitude = university.Longitude;
        }
    }

    public class UniversityPlaceMainDto
    {
        public int UniversityId { get; set; }

        public string Name { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public UniversityPlaceMainDto(University university)
        {
            UniversityId = university.UniversityId;
            Name = university.Name;

            Latitude = university.Latitude;
            Longitude = university.Longitude;
        }
    }
}