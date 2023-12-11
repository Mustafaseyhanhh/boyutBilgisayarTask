using EczaneV3.Data.Abstract;
using EczaneV3.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
	{
		private readonly EczaneV3DbContext _eczaneV3DbContext;
		public GenericRepository(EczaneV3DbContext eczaneV3DbContext)
		{
			_eczaneV3DbContext = eczaneV3DbContext;
		}

		public async Task<TEntity> AddAsync(TEntity entity)
		{
			await _eczaneV3DbContext.AddAsync(entity);
			await _eczaneV3DbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
		{
			await _eczaneV3DbContext.AddRangeAsync(entity);
			await _eczaneV3DbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<int> DeleteAsync(TEntity entity)
		{
			_ = _eczaneV3DbContext.Remove(entity);
			return await _eczaneV3DbContext.SaveChangesAsync();
		}

		public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
		{
			return await _eczaneV3DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
		}

		public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
		{
			return await (filter == null ? _eczaneV3DbContext.Set<TEntity>().AsTracking().ToListAsync(cancellationToken) : _eczaneV3DbContext.Set<TEntity>().AsTracking().Where(filter).ToListAsync(cancellationToken));
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			_ = _eczaneV3DbContext.Update(entity);
			await _eczaneV3DbContext.SaveChangesAsync();
			return entity;
		}

		public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
		{
			_eczaneV3DbContext.UpdateRange(entity);
			await _eczaneV3DbContext.SaveChangesAsync();
			return entity;
		}
	}
}
