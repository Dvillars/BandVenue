using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
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

        

        public static void DeleteAll()
        {
            DB.TableDeleteAll("bands");
        }
    }
}
