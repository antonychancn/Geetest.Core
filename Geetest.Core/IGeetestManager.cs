using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Geetest.Core
{
    public interface IGeetestManager
    {
        Task<RegisterResult> Register();

        Task Validate();
    }
}
