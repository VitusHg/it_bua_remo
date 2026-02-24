using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RdpMultiSessionManager.Models;

namespace RdpMultiSessionManager;

internal sealed class MainForm : Form
{
    private readonly TextBox _hostInput;
    private readonly Button _newSessionButton;
    private readonly TabControl _tabControl;
    private readonly Dictionary<TabPage, RdpSessionView> _sessions = new();

    public MainForm()
    {
        Text = "RDP Multi-Session Manager";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 1400;
        Height = 900;
        MinimumSize = new Size(960, 640);

        var topPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 52,
            Padding = new Padding(12, 10, 12, 10)
        };

        _hostInput = new TextBox
        {
            Dock = DockStyle.Fill,
            PlaceholderText = "Hostname oder IP"
        };

        _newSessionButton = new Button
        {
            Dock = DockStyle.Right,
            Text = "Neue RDP-Session",
            Width = 180
        };
        _newSessionButton.Click += (_, _) => CreateSession();

        topPanel.Controls.Add(_hostInput);
        topPanel.Controls.Add(_newSessionButton);

        _tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            DrawMode = TabDrawMode.OwnerDrawFixed,
            Padding = new Point(20, 4)
        };
        _tabControl.DrawItem += DrawTabWithCloseButton;
        _tabControl.MouseDown += HandleTabCloseClick;

        Controls.Add(_tabControl);
        Controls.Add(topPanel);

        Resize += (_, _) => ResizeActiveSessions();
        FormClosing += HandleFormClosing;
    }

    private void CreateSession()
    {
        var host = _hostInput.Text.Trim();
        if (string.IsNullOrWhiteSpace(host))
        {
            MessageBox.Show("Bitte Hostname oder IP angeben.", "Eingabe erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var session = new RdpSessionView(host);
            var tab = session.TabPage;
            _sessions[tab] = session;

            _tabControl.TabPages.Add(tab);
            _tabControl.SelectedTab = tab;
            session.Connect();
            _hostInput.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"RDP-Sitzung konnte nicht erstellt werden: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ResizeActiveSessions()
    {
        foreach (var session in _sessions.Values)
        {
            session.ResizeToContainer();
        }
    }

    private void HandleFormClosing(object? sender, FormClosingEventArgs e)
    {
        foreach (var session in _sessions.Values.ToList())
        {
            session.Dispose();
        }

        _sessions.Clear();
    }

    private void HandleTabCloseClick(object? sender, MouseEventArgs e)
    {
        for (var i = 0; i < _tabControl.TabPages.Count; i++)
        {
            var tabRect = _tabControl.GetTabRect(i);
            var closeRect = new Rectangle(tabRect.Right - 18, tabRect.Top + 6, 12, 12);
            if (!closeRect.Contains(e.Location))
            {
                continue;
            }

            var tab = _tabControl.TabPages[i];
            CloseTab(tab);
            break;
        }
    }

    private void CloseTab(TabPage tab)
    {
        if (_sessions.TryGetValue(tab, out var session))
        {
            session.Dispose();
            _sessions.Remove(tab);
        }

        if (_tabControl.TabPages.Contains(tab))
        {
            _tabControl.TabPages.Remove(tab);
        }
    }

    private void DrawTabWithCloseButton(object? sender, DrawItemEventArgs e)
    {
        var tab = _tabControl.TabPages[e.Index];
        var tabRect = _tabControl.GetTabRect(e.Index);

        TextRenderer.DrawText(e.Graphics, tab.Text, Font, tabRect, SystemColors.ControlText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        var closeRect = new Rectangle(tabRect.Right - 18, tabRect.Top + 6, 12, 12);
        e.Graphics.DrawRectangle(Pens.DimGray, closeRect);
        TextRenderer.DrawText(e.Graphics, "×", Font, closeRect, Color.DimGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}
