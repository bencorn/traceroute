using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

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

            string trace = "traceroute " + destination;
            var traceResult = trace.Bash();
            var hops = traceResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (hops.Count() == 0)
            {
                response = new JsonResult("Traceroute returned empty path for URL.");
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            foreach (string hop in hops)
            {

                var hopData = hop.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                int index = -1;

                for (int i = 0; i < hopData.Count(); i++)
                {
                    if (hopData[i].Contains("("))
                        index = i;
                }

                if (index != -1)
                {
                    traces.Add(new Trace()
                    {
                        HopAddress = Regex.Replace(hopData.ElementAt(index), "[()]", ""),
                        TripTime = float.Parse(hopData.ElementAt(index+1))
                    });
                }
            }

            response = new JsonResult(traces);
            response.StatusCode = (int)HttpStatusCode.OK;

            return response;
        }
    }

    public class Trace
    {
        public string HopAddress { get; set; }
        public float TripTime { get; set; }
    }

    public class TraceRequest
    {
        public string Hostname { get; set; }
    }
}
