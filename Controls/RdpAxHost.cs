using System;
using System.Windows.Forms;

namespace RdpMultiSessionManager.Controls;

internal sealed class RdpAxHost : AxHost
{
    private const string MsRdpClient9NotSafeForScriptingClsid = "8B918B82-7985-4C24-89DF-C33AD2BBFBCD";

    public RdpAxHost()
        : base(MsRdpClient9NotSafeForScriptingClsid)
    {
        Dock = DockStyle.Fill;
    }

    public object GetComObject() => GetOcx();
}
