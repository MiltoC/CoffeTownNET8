using CoffeTownNET8.AccesoDatos.Data.Repository.IRepository;
using CoffeTownNET8.Data;
using CoffeTownNET8.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeTownNET8.AccesoDatos.Data.Repository
{
    internal class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Producto producto)
        {
            var objDesdeDb = _db.Producto.FirstOrDefault(s => s.Id == producto.Id);
            objDesdeDb.Nombre = producto.Nombre;
            objDesdeDb.Descripcion = producto.Descripcion;
            objDesdeDb.Precio = producto.Precio;
            objDesdeDb.UrlImagen = producto.UrlImagen;
            objDesdeDb.CategoriaId = producto.CategoriaId;
            
            //_db.SaveChanges();
        }
    }
}
