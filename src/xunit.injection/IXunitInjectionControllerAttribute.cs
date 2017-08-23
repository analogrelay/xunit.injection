using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.Injection
{
    public interface IXunitInjectionControllerAttribute
    {
        IXunitInjectionController CreateInjectionController();
    }
}
