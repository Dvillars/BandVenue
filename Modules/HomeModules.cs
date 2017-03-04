using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;


namespace BandTracker
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>
      {
        return View["index.cshtml"];
      };

      Get["/venues"] = _ =>
      {
          return View["venues.cshtml", ModelMaker()];
      };

      Get["/bands"] = _ =>
      {
        return View["bands.cshtml", ModelMaker()];
      };

      Post["/bands"] = _ =>
      {
        Band newBand = new Band(Request.Form["band"]);
        newBand.Save();
        return View["bands.cshtml", ModelMaker()];
      };

      Post["/venues"] = _ =>
      {
          Venue newVenue = new Venue(Request.Form["venue"]);
          newVenue.Save();
          return View["venues.cshtml", ModelMaker()];
      };

    //   Get["/bands/{id}"]= parameters =>
    //  {
    //    Band newBand = Band.Find(parameters.id);
    //    Dictionary<string, object> model = ModelMaker();
    //    model.Add("Band Object", newBand);
    //    model.Add("Venue List", Venue.GetBand());
    //    return View["band.cshtml", model];
    //  };
    //
    //  Get["/venues/{id}"]= parameters =>
    // {
    //   Venue newVenue = Venue.Find(parameters.id);
    //   Dictionary<string, object> model = ModelMaker();
    //   model.Add("Venue Object", newVenue);
    //   model.Add("Band List", Band.GetVenue());
    //   return View["venue.cshtml", model];
    // };

    //  Get["/venues"] = _ =>
    //   {
    //     return View["venues.cshtml", ModelMaker()];
    //   };
    //
    //   Post["/venues"] = _ =>
    //   {
    //     Venue newVenue = new Venue(Request.Form["venue"], Request.Form["s-id"]);
    //     newVenue.Save();
    //     return View["venues.cshtml", ModelMaker()];
    //   };
    }

    public static Dictionary<string, object> ModelMaker()
    {
      Dictionary<string, object> model = new Dictionary<string, object>{};
      model.Add("Bands", Band.GetAll());
      model.Add("Venues", Venue.GetAll());
      return model;
    }
  }
}
