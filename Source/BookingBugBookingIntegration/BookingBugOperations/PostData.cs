﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Core;

namespace BookingBugBookingIntegration.BookingBugOperations
{
    //this thing is messy but really only for a one time use.
    class PostData : IBookingBugOperation
    {
        private readonly string _path = ConfigurationManager.AppSettings["PostDataFileLocation"];
        private readonly IBookingBugApi _bookingBugApi;
        private readonly ILogger _logger;

        public string Name { get { return "postdata"; } }
        public string Directions { get { return "specify 'postdata' along with the data type (client, bookings) EX: 'postdata bookings' and the postdata.csv file at the JsonFileLocation will be exported to bookingbug"; } }

        public PostData(IBookingBugApi bookingBugApi, ILogger logger)
        {
            _bookingBugApi = bookingBugApi;
            _logger = logger;
        }

        public void Execute(string[] args)
        {
            _logger.Info("Retrieving data.");
            var lines = File.ReadAllLines(_path);
            _logger.Info("Sending to booking bug.");
            var headers = lines.First().Split(',');
            foreach (var dictionary in lines.Skip(1)
                .Select(lineText => lineText.Split(','))
                .Select(line => line.ToDictionary(item => headers[Array.IndexOf(line, item)])))
            {
                //This type thing is sort of cheating to pull logic that doesn't really belong here up, it just made things easy and this post data is really a bit of a one-off
                _bookingBugApi.PostData(args[2], dictionary);
            }
        }
    }
}