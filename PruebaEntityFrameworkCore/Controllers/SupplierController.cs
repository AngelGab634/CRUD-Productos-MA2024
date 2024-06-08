using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaEntityFrameworkCore.Entidades;
using PruebaEntityFrameworkCore.Models;

namespace PruebaEntityFrameworkCore.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult SupplierList()
        {
            List<SupplierModel> categories 
            = _context.Proveedores.Select(Supplier => new SupplierModel()
            {
                Id = Supplier.Id,
                Name = Supplier.Nombre
            }).ToList();

            return View(categories);
        }   

        public IActionResult SupplierAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SupplierAdd(SupplierModel Supplier)
        {
            if (!ModelState.IsValid)
            {
                return View(Supplier);
            }

            var SupplierEntity = new Proveedor();
            SupplierEntity.Id = new Guid();
            SupplierEntity.Nombre = Supplier.Name;

            this._context.Proveedores.Add(SupplierEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("SupplierList","Supplier");
        }

        public IActionResult SupplierEdit(Guid Id)
        {
            // SENTENCIA EN LINQ
            Proveedor? Supplier = this._context.Proveedores
                .Where(p => p.Id == Id).FirstOrDefault();
            
            //VALIDACION SI NO LO ENCUENTRA 
            if (Supplier == null)
            {
                return RedirectToAction("SupplierList","Supplier");
            }

            //Se asigna la info de la BD al MODELO.
            SupplierModel model = new SupplierModel();
            model.Id = Supplier.Id;
            model.Name = Supplier.Nombre;

            //PASAMOS LA INFORMACION AL MODELO
            return View(model);
        }

        [HttpPost]
        public IActionResult SupplierEdit(SupplierModel Supplier)
        {
            //Carga la información de la BD
            Proveedor SupplierEntity = this._context.Proveedores
             .Where(p => p.Id == Supplier.Id).First();

            //VALIDACION
            if (SupplierEntity == null)
            {
                return View(Supplier);
            }
            
            if (!ModelState.IsValid)
            {
                return View(Supplier);
            }

            //REmplaza lo del modelo en el objeto de la BD
            SupplierEntity.Nombre = Supplier.Name;

            //Actualiza y guarda
            this._context.Proveedores.Update(SupplierEntity);
            this._context.SaveChanges();
            
            //Muestra otravez la lista 
            return RedirectToAction("SupplierList","Supplier");
        }

        public IActionResult SupplierDeleted(Guid Id)
        {
            Proveedor? Supplier = this._context.Proveedores
            .Where(p => p.Id == Id).FirstOrDefault();
            
            if (Supplier == null)
            {
                return RedirectToAction("SupplierList","Supplier");
            }


            SupplierModel model = new SupplierModel();
            model.Id = Supplier.Id;
            model.Name = Supplier.Nombre;

            return View(model);
        }

        [HttpPost]
        public IActionResult SupplierDeleted(SupplierModel Supplier)
        {
            bool exits = this._context.Proveedores.Any(p => p.Id == Supplier.Id);
            if (!exits)
            {
                return View(Supplier);
            }


            Proveedor SupplierEntity = this._context.Proveedores
            .Where(p => p.Id == Supplier.Id).First();

            this._context.Proveedores.Remove(SupplierEntity);
            this._context.SaveChanges();
            
            return RedirectToAction("SupplierList","Supplier");
        }

    }
}