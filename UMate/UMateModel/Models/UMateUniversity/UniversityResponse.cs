using System;
using System.Collections.Generic;
using UMateModel.Entities.Common;
using UMateModel.Models;
using UMateModel.Models.UMatePlace;

namespace UMateModel.Models.UMateUniversity
{
    // post response

    public class UniversityPostResponse : CommonResponse
    {
        public University University { get; set; }
    }

    // get response

    public class UniversityListResponse: CommonResponse
    {
        public int TotalCount { get; set; }

        public List<UniversityDto> Universities { get; set; }
    }

    public class UniversityResponse: CommonResponse
    {
        public UniversityDto University { get; set; }
    }

}