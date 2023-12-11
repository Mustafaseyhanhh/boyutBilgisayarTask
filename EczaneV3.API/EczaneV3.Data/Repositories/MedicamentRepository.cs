using EczaneV3.Data.Abstract;
using EczaneV3.Data.Contexts;
using EczaneV3.Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Repositories
{
	public class MedicamentRepository : GenericRepository<Medicament>, IMedicamentRepository
	{
		public MedicamentRepository(EczaneV3DbContext eczaneV3DbContext) : base(eczaneV3DbContext)
		{
		}
	}
}
