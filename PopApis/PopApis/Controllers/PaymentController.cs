﻿using Microsoft.AspNetCore.Mvc;
using PopLibrary.Data;
using PopLibrary.SqlModels;
using System.Linq;

namespace PopApis.Controllers
{
    public class PaymentController : CommonController
    {
        private PaymentData _data;

        public PaymentController(PaymentData data)
        {
            _data = data;
        }

        // GET: Payment
        public ActionResult Index()
        {
            return ViewWithSession("Index", _data.GetPayments());
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment/Create
        [HttpPost]
        public ActionResult Create(Payment itemToAdd)
        {
            _data.AddOrUpdatePayment(itemToAdd);
            return RedirectToAction("Index", _data.GetPayments());
        }

        public ActionResult Edit(int id)
        {
            return View(_data.GetPayments().ToList().Find(t => t.Id == id));
        }

        // POST: Payment/Edit
        [HttpPost]
        public ActionResult Edit(int id, Payment itemToUpdate)
        {
            _data.AddOrUpdatePayment(itemToUpdate);
            return RedirectToAction("Index", _data.GetPayments());
        }

        public ActionResult Delete(int id)
        {
            _data.DeletePayment(id);
            return RedirectToAction("Index", _data.GetPayments());
        }
    }
}