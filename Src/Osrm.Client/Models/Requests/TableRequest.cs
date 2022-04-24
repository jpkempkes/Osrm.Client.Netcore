using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Models.Requests
{
    public class TableRequest : BaseRequest
    {
        public TableRequest()
        {
            Sources = new uint[0];
            Destinations = new uint[0];
            Duration = true;
            Distance = false;
        }

        /// <summary>
        /// Use location with given index as source.
        /// {index};{index}[;{index} ...] or all (default)
        /// </summary>
        public uint[] Sources { get; set; }

        public bool Duration { get; set; }

        public bool Distance { get; set; }

        /// <summary>
        /// Use location with given index as destination.
        /// {index};{index}[;{index} ...] or all (default)
        /// </summary>
        public uint[] Destinations { get; set; }

        public override List<Tuple<string, string>> UrlParams
        {
            get
            {
                var urlParams = new List<Tuple<string, string>>(BaseUrlParams);

                var annotations = new List<string>();
                if (Duration)
                {
                    annotations.Add("duration");
                }

                if (Distance)
                {
                    annotations.Add("distance");
                }

                urlParams
                    .AddParams("sources", Sources.Select(x => x.ToString()).ToArray())
                    .AddParams("destinations", Destinations.Select(x => x.ToString()).ToArray())
                    .AddParams("annotations", new string[] {  string.Join(',', annotations)}, "duration");

            return urlParams;
            }
        }
    }
}