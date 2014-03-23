using System;
using Gtk;
using ProcessMonitor;
using System.Diagnostics;

public partial class MainWindow: Gtk.Window
{
	public PageProcessOverview ProcessOverview { get; protected set; }

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		Init();
	}

	protected void Init()
	{
		nbMain.CurrentPage = nbMain.AppendPage(new PageProcessOverview(), new Label("Process Overview"));
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
				
	}
}
