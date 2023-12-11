using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data
{
	static class Configuration
	{
		static public string ConnectionString
		{
			get
			{
				ConfigurationManager configurationManager = new();
				configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EczaneV3.API"));
				configurationManager.AddJsonFile("appsettings.json");

				return configurationManager.GetConnectionString("PostgreSQL");
			}
		}
	}
}
