using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	// ??? Intended for the ultimate migration to saving images to the file system instead of the database.
    public class ImageAdder : IImageAdder
	{
		public ImageAdder(string connectionString)
		{
			_foodForUsAllConnectionString = connectionString;
		}

		public async Task Add()
		{
			if (_foodForUsAllConnectionString == null)
				throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");
		}

		#region private

		readonly string _foodForUsAllConnectionString;

		#endregion
	}
}