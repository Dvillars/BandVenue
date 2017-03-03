using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class BandTest : IDisposable
    {
        public BandTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_BandsEmptyAtFirst()
        {
            //Arrange, Act
            int result = Band.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Save_AssignsIdToBandObject()
        {
            //Arrange
            Band testBand = new Band("band");
            testBand.Save();

            //Act
            Band savedBand = Band.GetAll()[0];

            int result = savedBand.GetId();
            int testId = testBand.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_Find_FindsBandInDatabase()
        {
            //Arrange
            Band testBand1 = new Band("band1");
            testBand1.Save();

            Band testBand2 = new Band("band2");
            testBand2.Save();

            //Act
            Band result = Band.Find(testBand2.GetId());

            //Assert
            Assert.Equal(testBand2, result);
        }

        [Fact]
        public void Update_OneBand_NewName()
        {
            Band testBand1 = new Band("band1");
            testBand1.Save();
            Band testBand2 = new Band("band2");
            testBand2.Save();

            testBand1.Update("band11");

            string newName = testBand1.GetName();
            string result = "band11";

            Assert.Equal(newName, result);
        }

        [Fact]
        public void Delete_OneBand_BandDeleted()
        {
            Band testBand1 = new Band("band1");
            testBand1.Save();
            Band testBand2 = new Band("band2");
            testBand2.Save();

            testBand1.Delete();

            List<Band> allBands = Band.GetAll();
            List<Band> expected = new List<Band>{testBand2};

            Assert.Equal(expected, allBands);
        }

        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
