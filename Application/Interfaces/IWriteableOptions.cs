using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }

}
