using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelmanism.Model
{
	public interface IResponse<TResponce>
	{
		TResponce Responce { get; set; }
	}
}
