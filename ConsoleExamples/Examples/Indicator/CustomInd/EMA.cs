﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Cloud.Indicator.Basic;
using Encog.Cloud.Indicator.Server;
using Encog.Util.CSV;

namespace Encog.Examples.Indicator.CustomInd
{
    public class EMA : BasicIndicator
    {
        public EMA()
            : base(true)
        {
            RequestData("CLOSE");
            RequestData("THIS[2]");
        }

        public override void NotifyPacket(IndicatorPacket packet)
        {
            if (string.Compare(packet.Command, IndicatorLink.PACKET_BAR,true) == 0)
            {
                String security = packet.Args[1];
                long when = long.Parse(packet.Args[0]);

                double dataClose = CSVFormat.EgFormat.Parse(packet.Args[2]);
                double lastValue = CSVFormat.EgFormat.Parse(packet.Args[4]);
                double period = 14;

                double result;

                if (double.IsNaN(lastValue))
                    result = dataClose;
                else
                    result = dataClose * (2.0 / (1 + period)) + (1 - (2.0 / (1 + period))) * lastValue;

                String[] args = { 
					CSVFormat.EgFormat.Format(result,EncogFramework.DefaultPrecision),
					"?",
					"?",
					"?",
					"?",
					"?",
					"?",
					"?"};

                this.Link.WritePacket(IndicatorLink.PACKET_IND, args);
            }
        }

        public override void NotifyTermination()
        {
            // don't really care		
        }
    }
}
