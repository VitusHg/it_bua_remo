using System;
using System.Drawing;
using System.Windows.Forms;
using MSTSCLib;
using RdpMultiSessionManager.Controls;

namespace RdpMultiSessionManager.Models;

internal sealed class RdpSessionView : IDisposable
{
    private readonly RdpAxHost _hostControl;
    private readonly IMsRdpClient9 _client;
    private readonly TabPage _tabPage;
    private readonly string _hostName;
    private bool _disposed;

    public RdpSessionView(string hostName)
    {
        _hostName = hostName;
        _tabPage = new TabPage(hostName);

        _hostControl = new RdpAxHost();
        _tabPage.Controls.Add(_hostControl);

        _client = (IMsRdpClient9)_hostControl.GetComObject();
        ConfigureClient();
        WireEvents();
    }

    public TabPage TabPage => _tabPage;

    public void Connect()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RdpSessionView));
        }

        if (_client.Connected == 0)
        {
            _client.Server = _hostName;
            ResizeToContainer();
            _client.Connect();
        }
    }

    public void ResizeToContainer()
    {
        if (_disposed)
        {
            return;
        }

        var width = Math.Max(_hostControl.Width, 800);
        var height = Math.Max(_hostControl.Height, 600);

        _client.DesktopWidth = width;
        _client.DesktopHeight = height;
        _client.UpdateSessionDisplaySettings((uint)width, (uint)height, 0, 0, 0, 1, 0, 0);
    }

    public void Disconnect()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            if (_client.Connected != 0)
            {
                _client.Disconnect();
            }
        }
        catch
        {
            // Best effort during shutdown.
        }
    }

    private void ConfigureClient()
    {
        _client.AdvancedSettings9.EnableCredSspSupport = true;
        _client.AdvancedSettings9.RedirectClipboard = true;
        _client.AdvancedSettings9.RedirectPrinters = false;
        _client.AdvancedSettings9.RedirectDrives = false;

        _client.ColorDepth = 32;
        _client.FullScreen = false;
    }

    private void WireEvents()
    {
        var events = (IMsTscAxEvents_Event)_client;
        events.OnConnected += () => UpdateTabTitle(_hostName);
        events.OnDisconnected += discReason => UpdateTabTitle($"{_hostName} (disconnected)");
    }

    private void UpdateTabTitle(string title)
    {
        if (_tabPage.IsDisposed)
        {
            return;
        }

        if (_tabPage.InvokeRequired)
        {
            _tabPage.BeginInvoke(new Action(() => _tabPage.Text = title));
            return;
        }

        _tabPage.Text = title;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Disconnect();

        try
        {
            _tabPage.Controls.Remove(_hostControl);
            _hostControl.Dispose();
            _tabPage.Dispose();
        }
        catch
        {
            // Suppress shutdown exceptions from ActiveX teardown.
        }
    }
}
