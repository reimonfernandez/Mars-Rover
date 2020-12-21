using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarsRover.Model
{
    public class MarsRoverPhotoList
    {
        [JsonProperty(PropertyName = "photos")]
        public List<MarsRoverPhoto> Photos { get; set; }
    }

    public class MarsRoverPhoto
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "sol")]
        public string SOL { get; set; }

        [JsonProperty(PropertyName = "camera")]
        public MarsRoverPhotoCamera Camera { get; set; }

        [JsonProperty(PropertyName = "img_src")]
        public string ImageSRC { get; set; }

        [JsonProperty(PropertyName = "earth_date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "rover")]
        public MarsRoverPhotoRover Rover { get; set; }
    }

    public class MarsRoverPhotoCamera
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "rover_id")]
        public long RoverId { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
    }

    public class MarsRoverPhotoRover
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "landing_date")]
        public string LandingDate { get; set; }

        [JsonProperty(PropertyName = "launch_date")]
        public string LaunchDate { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
