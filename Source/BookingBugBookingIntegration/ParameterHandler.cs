using System;
using System.Collections.Generic;
using Core;

namespace BookingBugBookingIntegration
{
    //TODO: There's probably a better way to handle this but this is easy.
    public class ParameterHandler : IParameterHandler
    {
        public Dictionary<string, DateTime> GetDateParameters(IReadOnlyList<string> args, int expectedArgCount)
        {
            if (args.Count < expectedArgCount)
            {
                throw new InvalidOperationException("Not enough arguments for this call.");
            }

            //sometimes linq is hard?
            var argDictionary = new Dictionary<string, DateTime>();
            for (var i = 2; i < args.Count - 1; i += 2)
            {
                DateTime dateTime;
                if (!DateTime.TryParse(args[i+1], out dateTime))
                    throw new InvalidOperationException("The " + args[i] + " must be a valid date");
                argDictionary.Add(args[i].ToLower(), dateTime);
            }
            return argDictionary;
        }

        public int GetIdFromParams(IReadOnlyList<string> args)
        {
            if (args.Count < 3)
                throw new IndexOutOfRangeException("Not enough arguments, you must specify an ID");
            int itemId;
            if (!int.TryParse(args[2], out itemId))
                throw new InvalidOperationException("The ID must be an integer");
            return itemId;
        }
    }

    public interface IParameterHandler : IDependency
    {
        Dictionary<string, DateTime> GetDateParameters(IReadOnlyList<string> args, int expectedArgCount);
        int GetIdFromParams(IReadOnlyList<string> args);
    }
}
