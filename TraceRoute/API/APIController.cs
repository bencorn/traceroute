using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace TraceRoute.API
{
    public class APIController
    {
        public APIController()
        {
        }

        /// <summary>
        /// Performs traceroute on specified hostname.
        /// </summary>
        /// <returns>JSON array of hops and the round trip time.</returns>
        /// <param name="destination">Hostname IP / URL</param>
        [HttpGet("api/trace/{destination}")]
        public JsonResult TraceRoute(string destination)
        {
            List<Trace> traces = new List<Trace>();
            JsonResult response;

            string trace = "traceroute -n -m 30 -w1 -q 1 " + destination;
            var traceResult = trace.Bash();
            var hops = traceResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            if (hops[0].Contains("traceroute"))
            {
                hops.RemoveAt(0);
            }

            if (hops.Count() == 0)
            {
                response = new JsonResult(traceResult);
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            foreach (string hop in hops)
            {

                var hopData = hop.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!hopData[1].Contains("*"))
                {
                    Trace t = new Trace();
                    t.HopAddress = hopData[1];
                    t.TripTime = float.Parse(hopData[2]);
                    t.Coordinates = Locate(t);
                    traces.Add(t);
                }
            }

            traces = traces.GroupBy(x => new { x.Coordinates.Latitude , x.Coordinates.Longitude}).Select(x => x.First()).ToList();

            response = new JsonResult(traces);
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }

        public Coordinate Locate(Trace trace)
        {
            var client = new RestClient("https://api.ipdata.co/" + trace.HopAddress);
            var request = new RestRequest(Method.GET);
            IRestResponse<Coordinate> response =  client.Execute<Coordinate>(request);

            return response.Data;
        }
    }

    public class Trace
    {
        public string HopAddress { get; set; }
        public float TripTime { get; set; }
        public Coordinate Coordinates { get; set; }
    }

    public class TraceRequest
    {
        public string Hostname { get; set; }
    }

    public class Coordinate
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
