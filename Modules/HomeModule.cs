using Nancy;
using Nancy.ViewEngines.Razor;
using Inventory;
// using System;
// using System.Web;
using System.Collections.Generic;
// using System.Text.RegularExpressions;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Post["/add_item"] = _ => {
        Item newItem = new Item(Request.Form["category"], Request.Form["name"], Request.Form["description"], Request.Form["amount"], Request.Form["price"]);
        newItem.Save();
        List<Item> allItems = new List<Item>(){};
        allItems = Item.GetAll();
        return View["item_list.cshtml", allItems];
      };
      Get["/clear_all"] = _ => {
        Item.DeleteAll();
        return View["cleared.cshtml"];
      };

    }
  }
}
