using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace BandTracker
{
    public class Venue
    {
        private string _name;
        private int _id;

        public Venue(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherVenue)
        {
            if(!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newVenue = (Venue) otherVenue;
                bool idEquality = this.GetId() == newVenue.GetId();
                bool nameEquality = this.GetName() == newVenue.GetName();
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

        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

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

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

            cmd.Parameters.Add(new SqlParameter("@Name", this.GetName()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static Venue Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @VenueId;", conn);
            cmd.Parameters.Add(new SqlParameter("@VenueId", id.ToString()));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundVenueId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundVenueId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }
            Venue foundVenue = new Venue(foundName, foundVenueId);

            DB.CloseSqlConnection(rdr, conn);

            return foundVenue;
        }

        public void Update(string newName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE venues SET name = @NewName OUTPUT INSERTED.name WHERE id=@VenueId;", conn);

            SqlParameter newNameParameter = new SqlParameter("@NewName", newName);
            cmd.Parameters.Add(newNameParameter);

            SqlParameter venueIdParameter = new SqlParameter("@VenueId", this.GetId());
            cmd.Parameters.Add(venueIdParameter);

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

            SqlCommand cmd = new SqlCommand("DELETE FROM venues WHERE id = @VenueId;", conn);
            cmd.Parameters.Add(new SqlParameter("@VenueId", this.GetId()));

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Band> GetBand()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT bands.* FROM venues JOIN bands_venues ON (venues.id = bands_venues.venue_id) JOIN bands ON (bands_venues.band_id = bands.id) WHERE venues.id = @VenueId;", conn);

            cmd.Parameters.Add(new SqlParameter("@VenueId", this.GetId().ToString()));

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Band> allBands = new List<Band> {};

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

        public void AddBand(int bandId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);", conn);

            cmd.Parameters.Add(new SqlParameter("@VenueId", this.GetId().ToString()));
            cmd.Parameters.Add(new SqlParameter("@BandId", bandId.ToString()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public void UpdateBand(Band OldBand, Band newBand)
        {
            int OldBandId = OldBand.GetId();

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE bands_venues SET band_id = @NewBandId WHERE band_id = @OldBandId AND venue_id = @VenueId;", conn);

            SqlParameter venueParameter = new SqlParameter("@VenueId", this.GetId());
            SqlParameter oldBandParameter = new SqlParameter("@OldBandId", OldBandId);
            SqlParameter newBandParameter = new SqlParameter("@NewBandId", newBand.GetId());
            cmd.Parameters.Add(venueParameter);
            cmd.Parameters.Add(oldBandParameter);
            cmd.Parameters.Add(newBandParameter);

            SqlDataReader rdr = null;
            cmd.ExecuteNonQuery();

            DB.CloseSqlConnection(rdr, conn);
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("venues");
        }
    }
}
