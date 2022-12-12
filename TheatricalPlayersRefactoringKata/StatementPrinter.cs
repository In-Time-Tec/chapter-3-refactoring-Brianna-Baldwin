using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> listOfPlays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var performance in invoice.Performances)
            {
                var play = listOfPlays[performance.PlayID];
                var playCost = 0;
                playCost = FindPlayCostFromPlayType(play, playCost, performance);
                volumeCredits = CalculateVolumeCredits(performance, play, volumeCredits);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(playCost / 100), performance.Audience);
                totalAmount += playCost;
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private int FindPlayCostFromPlayType(Play play, Int32 playCost, Performance performance)
        {
            switch (play.Type)
            {
                case "tragedy":
                    playCost = CalculateTragedyPlayCost(playCost, performance);
                    break;
                case "comedy":
                    playCost = CalculateComedyPlayCost(playCost, performance);
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }
            return playCost;
        }

        private int CalculateTragedyPlayCost(Int32 playCost, Performance performance)
        {
            playCost = 40000;
            if (performance.Audience > 30)
            {
                playCost += 1000 * (performance.Audience - 30);
            }

            return playCost;
        }

        private int CalculateComedyPlayCost(Int32 playCost, Performance performance)
        {
            playCost = 30000;
            if (performance.Audience > 20)
            {
                playCost += 10000 + 500 * (performance.Audience - 20);
            }
            playCost += 300 * performance.Audience;

            return playCost;
        }

        private int CalculateVolumeCredits(Performance performance, Play play, int volumeCredits)
        {
            // add volume credits
            volumeCredits += Math.Max(performance.Audience - 30, 0);
            // add extra credit for every ten comedy attendees
            if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)performance.Audience / 5);
            return volumeCredits;
        }
    }
}
