using SistemaERP.DAL.Repositorios.Contrato;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SistemaERP.DAL.DBContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SistemaERP.DAL.Repositorios
{
    public class GenericRepository<TModelo, T> : IGenericRepository<TModelo, T> where TModelo : class, new() where T : DbContext
    {
        private readonly T _dbcontext;

        public GenericRepository(T dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                var modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                if (modelo != null)
                    return modelo;
                return new TModelo();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TModelo>> ObtenerTodos(Expression<Func<TModelo, bool>> filtro)
        {
                if (filtro != null)
                {
                    List<TModelo> modelos = await _dbcontext.Set<TModelo>().Where(filtro).ToListAsync();
                    return modelos;
                }
                else
                {
                    List<TModelo> modelos = await _dbcontext.Set<TModelo>().ToListAsync();
                    return modelos;
                }
        }
        public async Task<List<TModelo>> ObtenerTodos()
        {
            try
            {
                List<TModelo> modelos = await _dbcontext.Set<TModelo>().ToListAsync();
                return modelos;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<TModelo>();
            }
        }

        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return new TModelo();
            }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarMultiple(List<TModelo> modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().RemoveRange(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CrearMultiple(List<TModelo> modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().AddRange(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

        public async Task<bool> EditarMultiple(List<TModelo> modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().UpdateRange(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }
    }

    public class SSORepositorio<TModelo> : ISSORepositorio<TModelo> where TModelo : class, new()
    {
        private readonly SSOContext _dbcontext;
        //private readonly TModelo _Clase;
        public SSORepositorio(
             //TModelo clase
             SSOContext dbcontext)
        {
            _dbcontext = dbcontext;
            //_Clase = clase;
        }

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                var modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                if (modelo != null)
                    return modelo;
                return new TModelo();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TModelo>> ObtenerTodosInclude(Expression<Func<TModelo, bool>> filtro,
            Func<IQueryable<TModelo>, IQueryable<TModelo>>? include = null)
        {
            IQueryable<TModelo> query = _dbcontext.Set<TModelo>().Where(filtro);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }


        public async Task<List<TModelo>> ObtenerTodos
        (
            Expression<Func<TModelo, bool>> filtro,
            Func<IQueryable<TModelo>, IQueryable<TModelo>>? include = null,
            Func<IQueryable<TModelo>, IQueryable<TModelo>>? include2 = null
        )
        {
            try
            {
                var query = _dbcontext.Set<TModelo>().AsQueryable();
                if (include != null)
                {
                    query = include(query);
                }
                if (include2 != null)
                {
                    query = include2(query);
                }
                if (filtro != null)
                {
                    query = query.Where(filtro);
                }
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<TModelo>();
            }
        }

        public async Task<List<TModelo>> ObtenerTodos()
        {
            try
            {
                List<TModelo> modelos = await _dbcontext.Set<TModelo>().ToListAsync();
                return modelos;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return new List<TModelo>();
            }
        }

        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Add(modelo);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModelo = filtro == null ? _dbcontext.Set<TModelo>() : _dbcontext.Set<TModelo>().Where(filtro);
                return queryModelo;
            }
            catch
            {
                throw;
            }
        }
    }
}
