//using Microsoft.EntityFrameworkCore;
//using MyMoneyManager.API.Interfaces.IRepositories;
//using System.Linq.Expressions;

//namespace MyMoneyManager.API.Repositories;

//public class BaseRepository<T> : IBaseRepository<T> where T : class
//{
//    protected readonly AppDbContext _dbContext;
//    protected DbSet<T> _dbSet => _dbContext.Set<T>();

//    public BaseRepository(AppDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task<T> Create(T model, CancellationToken cancellationToken = default)
//    {
//        await _dbContext.Set<T>().AddAsync(model, cancellationToken);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//        return model;
//    }

//    public async Task CreateRange(List<T> model, CancellationToken cancellationToken)
//    {
//        await _dbContext.Set<T>().AddRangeAsync(model, cancellationToken);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }

//    public async Task Delete(T model, CancellationToken cancellationToken)
//    {
//        _dbContext.Set<T>().Remove(model);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }

//    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
//    {
//        var data = await _dbContext.Set<T>()
//            .AsNoTracking()
//            .ToListAsync(cancellationToken);

//        return data;
//    }

//    public async Task<IEnumerable<T>> GetAll(List<Expression<Func<T, bool>>> includeExpressions, CancellationToken cancellationToken = default)
//    {
//        var query = _dbContext.Set<T>().AsQueryable();
        
//        if (includeExpressions != null)
//        {
//            query = includeExpressions.Aggregate(query, (current, includeExpression) => current.Where(includeExpression));
//        }

//        var entities = await query.AsNoTracking().ToListAsync(cancellationToken);
//        return entities;
//    }

//    public async Task<T> GetById<Tid>(Tid id, CancellationToken cancellationToken)
//    {
//        var data = await _dbContext.Set<T>().FindAsync(id, cancellationToken);
//        if (data == null)
//            throw new Exception("No data found");
//        return data;
//    }

//    public async Task<T> GetById<Tid>(List<Expression<Func<T, bool>>> includeExpressions, Tid id, CancellationToken cancellationToken)
//    {
//        var query = _dbContext.Set<T>().AsQueryable();

//        if (includeExpressions != null)
//        {
//            query = includeExpressions.Aggregate(query, (current, include) => current.Where(include));
//        }

//        var data = await query.SingleOrDefaultAsync(x => EF.Property<Tid>(x, "Id").Equals(id), cancellationToken);

//        if (data == null)
//        {
//            throw new Exception("No data found");
//        }

//        return data;
//    }

//    public async Task<bool> IsExists<Tvalue>(string key, Tvalue value, CancellationToken cancellationToken)
//    {
//        var parameter = Expression.Parameter(typeof(T), "x");
//        var property = Expression.Property(parameter, key);
//        var constant = Expression.Constant(value);
//        var equality = Expression.Equal(property, constant);
//        var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

//        return await _dbContext.Set<T>().AnyAsync(lambda, cancellationToken);
//    }

//    public async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value, CancellationToken cancellationToken)
//    {
//        var parameter = Expression.Parameter(typeof(T), "x");
//        var property = Expression.Property(parameter, key);
//        var constant = Expression.Constant(value);
//        var equality = Expression.Equal(property, constant);

//        var idProperty = Expression.Property(parameter, "Id");
//        var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

//        var combinedExpression = Expression.AndAlso(equality, idEquality);
//        var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

//        return await _dbContext.Set<T>().AnyAsync(lambda, cancellationToken);
//    }

//    public async Task SaveChangeAsync(CancellationToken cancellationToken)
//    {
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }

//    public async Task Update(T model, CancellationToken cancellationToken)
//    {
//        _dbContext.Set<T>().Update(model);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }
//}
