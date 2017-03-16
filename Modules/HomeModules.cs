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

      Get["/bands/{id}"]= parameters =>
     {
       Dictionary<string, object> model = ModelMaker();
       model.Add("Band Object", Band.Find(parameters.id));
       model.Add("Venue List", Venue.GetAll());
       return View["band.cshtml", model];
     };

     Get["/venues/{id}"]= parameters =>
    {
      Dictionary<string, object> model = ModelMaker();
      model.Add("Venue Object", Venue.Find(parameters.id));
      model.Add("Band List", Band.GetAll());
      return View["venue.cshtml", model];
    };

     Delete["/venues/{id}"] = parameters =>
      {
        Venue.Find(parameters.id).Delete(parameters.id);
        return View["venues.cshtml", ModelMaker()];
      };

      Patch["/venues/{id}"] = parameters =>
      {
        Venue.Find(parameters.id).Update(Request.Form["venue"]);
        Dictionary<string, object> model = ModelMaker();
        model.Add("Venue Object", Venue.Find(parameters.id));
        model.Add("Band List", Band.GetAll());
        Console.WriteLine(Venue.Find(parameters.id));
        return View["venue.cshtml", model];
      };
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
