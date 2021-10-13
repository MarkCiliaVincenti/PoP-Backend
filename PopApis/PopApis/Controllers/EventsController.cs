using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PopLibrary;
using PopLibrary.Data;
using PopLibrary.SqlModels;

namespace PopApis.Controllers
{
    public class EventsController : Controller
    {
        private List<Event> events = new();
        private EventData _data;

        public EventsController(EventData data)
        {
            _data = data;
        }
        // GET: EventsController
        public ActionResult Index()
        {
            return View("Index", _data.GetEvents());
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        public ActionResult Create(Event itemToAdd)
        {
            _data.AddOrUpdateEvent(itemToAdd);
            return RedirectToAction("Index", _data.GetEvents());
        }

        public ActionResult Edit(int id)
        {
            return View(_data.GetEvents().ToList().Find(t => t.Id == id));
        }

        // GET: EventsController/Edit
        [HttpPost]
        public ActionResult Edit(int id, Event itemToUpdate)
        {
                _data.AddOrUpdateEvent(itemToUpdate);
                return RedirectToAction("Index", _data.GetEvents());
        }

        public ActionResult Delete(int id)
        {
            _data.DeleteEvent(id);
            return RedirectToAction("Index", _data.GetEvents());
        }
    }
}
