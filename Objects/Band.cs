using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace BandTracker
{
    public class Band
    {
        private string _name;
        private int _id;

        public Band(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherBand)
        {
            if(!(otherBand is Band))
            {
                return false;
            }
            else
            {
                Band newBand = (Band) otherBand;
                bool idEquality = this.GetId() == newBand.GetId();
                bool nameEquality = this.GetName() == newBand.GetName();
                return (idEquality && nameEquality);
            }
        }

        public int GetId()
        {
            return _id;
        }
        public void SetName(string newName)
        {
            _name = newName;
        }
        public string GetName()
        {
            return _name;
        }

        public static List<Band> GetAll()
        {
            List<Band> allBands = new List<Band>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int bandId = rdr.GetInt32(0);
                string bandName = rdr.GetString(1);
                Band newBand = new Band(bandName, bandId);
                allBands.Add(newBand);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allBands;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

            cmd.Parameters.Add(new SqlParameter("@Name", this.GetName()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static Band Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @BandId;", conn);
            cmd.Parameters.Add(new SqlParameter("@BandId", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundBandId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundBandId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }
            Band foundBand = new Band(foundName, foundBandId);

            DB.CloseSqlConnection(rdr, conn);

            return foundBand;
        }

        public void Update(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName OUTPUT INSERTED.name WHERE id=@BandId;", conn);

            SqlParameter newNameParameter = new SqlParameter("@NewName", newName);
            cmd.Parameters.Add(newNameParameter);

            SqlParameter bandIdParameter = new SqlParameter("@BandId", this.GetId());
            cmd.Parameters.Add(bandIdParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public void Delete()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM bands WHERE id = @BandId;", conn);
            cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId()));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Venue> GetVenue()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (bands_venues.venue_id = venues.id) WHERE bands.id = @BandId;", conn);

            cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Venue> allVenues = new List<Venue> {};

            while(rdr.Read())
            {
                int venueId = rdr.GetInt32(0);
                string venueName = rdr.GetString(1);
                Venue newVenue = new Venue(venueName, venueId);
                allVenues.Add(newVenue);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allVenues;
        }

        public void AddVenue(int venueId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (band_id, venue_id) VALUES (@BandId, @VenueId);", conn);

            cmd.Parameters.Add(new SqlParameter("@BandId", this.GetId().ToString()));
            cmd.Parameters.Add(new SqlParameter("@VenueId", venueId.ToString()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public void UpdateVenue(Venue OldVenue, Venue newVenue)
        {
            int OldVenueId = OldVenue.GetId();

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE bands_venues SET venue_id = @NewVenueId WHERE venue_id = @OldVenueId AND band_id = @BandId;", conn);

            SqlParameter bandParameter = new SqlParameter("@BandId", this.GetId());
            SqlParameter oldVenueParameter = new SqlParameter("@OldVenueId", OldVenueId);
            SqlParameter newVenueParameter = new SqlParameter("@NewVenueId", newVenue.GetId());
            cmd.Parameters.Add(bandParameter);
            cmd.Parameters.Add(oldVenueParameter);
            cmd.Parameters.Add(newVenueParameter);

            SqlDataReader rdr = null;
            cmd.ExecuteNonQuery();

            DB.CloseSqlConnection(rdr, conn);
        }


        public static void DeleteAll()
        {
            DB.TableDeleteAll("bands");
        }
    }
}
