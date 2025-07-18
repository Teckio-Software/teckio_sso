using System.Linq.Expressions;

namespace SistemaERP.DAL.Repositorios.Contrato
{
    public interface IGenericRepository<TModel, T>
    {
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);

        Task<List<TModel>> ObtenerTodos(Expression<Func<TModel, bool>> filtro);
        Task<List<TModel>> ObtenerTodos();
        Task<TModel> Crear(TModel modelo);
        Task<bool> CrearMultiple(List<TModel> modelo);
        Task<bool> EditarMultiple(List<TModel> modelo);
        Task<bool> Editar(TModel modelo);
        Task<bool> Eliminar(TModel modelo);
        Task<bool> EliminarMultiple(List<TModel> modelo);
    }
    public interface ISSORepositorio<TModel>
    {
        Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro);
        Task<List<TModel>> ObtenerTodos(
            Expression<Func<TModel, bool>> filtro, 
            Func<IQueryable<TModel>, IQueryable<TModel>>? include = null, 
            Func<IQueryable<TModel>, IQueryable<TModel>>? include2 = null);
        Task<List<TModel>> ObtenerTodos();
        Task<TModel> Crear(TModel modelo);
        Task<bool> Editar(TModel modelo);
        Task<bool> Eliminar(TModel modelo);
        Task<IQueryable<TModel>> Consultar(Expression<Func<TModel, bool>> filtro = null);
    }
}
