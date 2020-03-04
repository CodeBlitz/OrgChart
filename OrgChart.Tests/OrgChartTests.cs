using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace OrganizationChart.Tests
{
    class OrgChartTests
    {       
        [SetUp]
        public void Setup()
        {


        }


        [Test]
        public void RemoveTest()
        {
            OrgChart.OrgChart orgChart = new OrgChart.OrgChart();

            orgChart.Add(10, "Sharilyn Gruber", -1);
            orgChart.Add(7, "Denice Mattice", 10);
            orgChart.Add(3, "Lawana Futrell", -1);
            orgChart.Add(34, "Lissette Gorney", 7);
            orgChart.Add(5, "Lan Puls", 3);
            orgChart.Remove(7);

            Assert.AreEqual(1, orgChart.Count(10));
            Assert.AreEqual(0, orgChart.Count(7));
            Assert.AreEqual(0, orgChart.Count(5));
        }


    }
}
