using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BandTracker
{
    public class VenueTest : IDisposable
    {
        public VenueTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_VenuesEmptyAtFirst()
        {
            //Arrange, Act
            int result = Venue.GetAll().Count;

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Save_AssignsIdToVenueObject()
        {
            //Arrange
            Venue testVenue = new Venue("ven");
            testVenue.Save();

            //Act
            Venue savedVenue = Venue.GetAll()[0];

            int result = savedVenue.GetId();
            int testId = testVenue.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_Find_FindsVenueInDatabase()
        {
            //Arrange
            Venue testVenue1 = new Venue("ven1");
            testVenue1.Save();

            Venue testVenue2 = new Venue("ven2");
            testVenue2.Save();

            //Act
            Venue result = Venue.Find(testVenue2.GetId());

            //Assert
            Assert.Equal(testVenue2, result);
        }

        [Fact]
        public void Update_OneVenue_NewName()
        {
            Venue testVenue1 = new Venue("ven1");
            testVenue1.Save();
            Venue testVenue2 = new Venue("ven2");
            testVenue2.Save();

            testVenue1.Update("ven11");

            string newName = testVenue1.GetName();
            string result = "ven11";

            Assert.Equal(newName, result);
        }

        [Fact]
        public void Delete_OneVenue_VenueDeleted()
        {
            Venue testVenue1 = new Venue("ven1");
            testVenue1.Save();
            Venue testVenue2 = new Venue("ven2");
            testVenue2.Save();

            testVenue1.Delete(testVenue1.GetId());

            List<Venue> allVenues = Venue.GetAll();
            List<Venue> expected = new List<Venue>{testVenue2};

            Assert.Equal(expected, allVenues);
        }

        [Fact]
        public void UpdateBand_OneVenue_VenueAndNewBand()
        {
            Venue testVenue1 = new Venue("ven");
            testVenue1.Save();

            Band testBand1 = new Band("band1");
            testBand1.Save();

            Band testBand2 = new Band("band2");
            testBand2.Save();

            testVenue1.AddBand(testBand1.GetId());
            testVenue1.UpdateBand(testBand1, testBand2);

            List<Band> allBands = testVenue1.GetBand();
            List<Band> expected = new List<Band>{testBand2};

            Assert.Equal(expected, allBands);
        }


        public void Dispose()
        {
            Venue.DeleteAll();
            Band.DeleteAll();
        }
    }
}
