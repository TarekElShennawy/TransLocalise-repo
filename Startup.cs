using Translator_Project_Management.Services;

namespace Translator_Project_Management
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			ServiceRegistration.AddServices(services);
		}
	}
}