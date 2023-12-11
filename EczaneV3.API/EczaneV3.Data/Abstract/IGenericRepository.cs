using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Abstract
{
	public interface IGenericRepository<T> where T : class, new()
	{
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
		Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task<int> DeleteAsync(T entity);
		Task<List<T>> AddRangeAsync(List<T> entity);
		Task<List<T>> UpdateRangeAsync(List<T> entity);
	}
}
