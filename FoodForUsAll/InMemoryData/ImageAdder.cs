using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace InMemoryData
{
    public class ImageAdder : Services.IImageAdder
	{
		public async Task<byte[]> Validate(IBrowserFile browserFile, long fileSizeLimit)
		{
			return null;
		}
	}
}