using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Services
{
    public class ImageAdder : IImageAdder
	{
		public async Task<byte[]> Validate(IBrowserFile browserFile, long fileSizeLimit)
		{
			var trustedFileNameForFileStorage = Path.GetRandomFileName();
			string path = Path.Combine(@"wwwroot\unsafe_uploads", trustedFileNameForFileStorage);

			await using (FileStream fs = new(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 1024, FileOptions.DeleteOnClose))
			{
				await browserFile.OpenReadStream(fileSizeLimit)
					.CopyToAsync(fs);
				fs.Position = 0;
				var imageByteArray = new byte[fs.Length];
				fs.Read(imageByteArray, 0, imageByteArray.Length);
				return imageByteArray;
			}
		}
	}
}