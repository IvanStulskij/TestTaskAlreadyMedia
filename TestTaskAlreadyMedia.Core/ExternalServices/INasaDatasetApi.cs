using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskAlreadyMedia.Core.Models;

namespace TestTaskAlreadyMedia.Core.ExternalServices;

public interface INasaDatasetApi
{
    [Get("/refs/heads/main/y77d-th95.json")]
    Task<IEnumerable<NasaObjectDto>> GetNasaObjects();
}
