using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        private static List<Inventory> inventories = new List<Inventory>();
        private static int nextId;

        public ActionResult Index(string search ="", string status ="", string category="")
        {
            var result = inventories.AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                result = inventories.Where( i => i.Name.ToLower().Contains(search.ToLower()) ||
                                                 i.Category.ToLower().Contains(search.ToLower()) ||
                                                 i.Status.ToString().ToLower().Contains(search.ToLower()) ||
                                                 i.Quantity.ToString().Contains(search));
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse(status, out InventoryStatus parsedStatus))
                {
                    result = result.Where(i => i.Status == parsedStatus);
                }
            }
            if (!string.IsNullOrEmpty(category))
            {
                result = result.Where(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            return View(result.ToList());

        }

        [HttpPost]
        public ActionResult Add(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                inventory.Id = nextId++;
                inventories.Add(inventory);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var inv = inventories.FirstOrDefault(i => i.Id == id);
            if (inv == null)
            {
                return HttpNotFound();
            }
            else
            {
                               return View(inv);
            }
        }

        [HttpPost]
        public ActionResult Edit(Inventory Updatedinventory)
        {
            if (ModelState.IsValid)
            {
                var inv = inventories.FirstOrDefault(i => i.Id == Updatedinventory.Id);
                if (inv != null)
                {
                    inv.Name = Updatedinventory.Name;
                    inv.Category = Updatedinventory.Category;
                    inv.Quantity = Updatedinventory.Quantity;
                    inv.Price = Updatedinventory.Price;
                    inv.Status = Updatedinventory.Status;
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var inv = inventories.FirstOrDefault(i => i.Id == id);
            if (inv != null)
            {
                inventories.Remove(inv);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public static List<Inventory> GetInventories()
        {
            return inventories;
        }
    }
}