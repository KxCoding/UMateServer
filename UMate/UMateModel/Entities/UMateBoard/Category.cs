using System;
using System.Text.Json.Serialization;

namespace UMateModel.Entities.UMateBoard
{
    public class Category
    {
        public int CategoryId { get; set; }


        public int BoardId { get; set; }
        [JsonIgnore]
        public Board Board { get; set; }


        public string Name { get; set; }
    }
}
