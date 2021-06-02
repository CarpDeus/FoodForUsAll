using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Services
{
	public interface IImageAdder
	{
		Task<byte[]> Validate(IBrowserFile browserFile, long fileSizeLimit);
	}
}
