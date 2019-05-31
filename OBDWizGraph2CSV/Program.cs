using System;
using System.Xml;

namespace OBDWizGraph2CSV
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Syntax error: OBDWizGraph2CSV.exe FileToConvert.xml");
                return;
            }

            string[] xValues = { "", "", "", "" };
            string[] xAxisNames = { "", "", "", "" };
            string[] xAxisUnits = { "", "", "", "" };
            string[] yValues = { "", "", "", "" };
            string[] yAxisNames = { "", "", "", "" };
            string[] yAxisUnits = { "", "", "", "" };
            long maxArrayLength = 999999999;
            long idx = 0;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Runtime error: Unable to open the file specified [{0}]", args[0]);
                return;
            }

            XmlNodeList itemList = xmlDoc.GetElementsByTagName("Item");

            foreach (XmlNode item in itemList)
            {
                XmlNode graphData = item.SelectSingleNode("GraphData");

                XmlNode tempNode = graphData.SelectSingleNode("XPlotLabel/Name");
                xAxisNames[idx] = tempNode.InnerText;
                xAxisUnits[idx] = graphData.SelectSingleNode("XPlotLabel/Units").InnerText;

                tempNode = graphData.SelectSingleNode("YPlotLabel/Name");
                yAxisNames[idx] = tempNode.InnerText;
                yAxisUnits[idx] = graphData.SelectSingleNode("YPlotLabel/Units").InnerText;

                xValues[idx] = graphData.SelectSingleNode("XData").InnerText;
                yValues[idx] = graphData.SelectSingleNode("YData").InnerText;

                idx++;
            }

            double[] obdParam1_xValues = ConvertHexStringToDouble(xValues[0]);
            if (obdParam1_xValues.Length < maxArrayLength) maxArrayLength = obdParam1_xValues.Length;
            double[] obdParam2_xValues = ConvertHexStringToDouble(xValues[1]);
            if (obdParam2_xValues.Length < maxArrayLength) maxArrayLength = obdParam2_xValues.Length;
            double[] obdParam3_xValues = ConvertHexStringToDouble(xValues[2]);
            if (obdParam3_xValues.Length < maxArrayLength) maxArrayLength = obdParam3_xValues.Length;
            double[] obdParam4_xValues = ConvertHexStringToDouble(xValues[3]);
            if (obdParam4_xValues.Length < maxArrayLength) maxArrayLength = obdParam4_xValues.Length;
            double[] obdParam1_yValues = ConvertHexStringToDouble(yValues[0]);
            if (obdParam1_yValues.Length < maxArrayLength) maxArrayLength = obdParam1_yValues.Length;
            double[] obdParam2_yValues = ConvertHexStringToDouble(yValues[1]);
            if (obdParam2_yValues.Length < maxArrayLength) maxArrayLength = obdParam2_yValues.Length;
            double[] obdParam3_yValues = ConvertHexStringToDouble(yValues[2]);
            if (obdParam3_yValues.Length < maxArrayLength) maxArrayLength = obdParam3_yValues.Length;
            double[] obdParam4_yValues = ConvertHexStringToDouble(yValues[3]);
            if (obdParam4_yValues.Length < maxArrayLength) maxArrayLength = obdParam4_yValues.Length;

            Console.Write("{0},{1}", xAxisNames[0], yAxisNames[0]);
            for (int i = 1; i < 4; i++)
            {
                Console.Write(",{0},{1}", xAxisNames[i], yAxisNames[i]);
            }
            Console.Write("\n");

            Console.Write("{0},{1}", xAxisUnits[0], yAxisUnits[0]);
            for (int i = 1; i < 4; i++)
            {
                Console.Write(",{0},{1}", xAxisUnits[i], yAxisUnits[i]);
            }
            Console.Write("\n");

            for (int i = 0; i < maxArrayLength; i++)
            {
                Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", obdParam1_xValues[i], obdParam1_yValues[i], obdParam2_xValues[i], obdParam2_yValues[i], obdParam3_xValues[i], obdParam3_yValues[i], obdParam4_xValues[i], obdParam4_yValues[i]);
            }

        }
        static double[] ConvertHexStringToDouble(string inputHexDataString)
        {
            string[] hexStringValues = inputHexDataString.Split(' ');
            double[] returnValues = new double[hexStringValues.Length];
            int i = 0;
            foreach (string hs in hexStringValues)
            {
                string s = "";
                if (hs.CompareTo("00") == 0)
                    s = "0000000000000000";
                else
                    s = "" + hs;
                Int64 num = Int64.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] doubleVals = BitConverter.GetBytes(num);
                double d = BitConverter.ToDouble(doubleVals, 0);
                returnValues[i++] = d;
            }
            return returnValues;
        }

    }
}
