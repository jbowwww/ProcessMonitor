
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.VBox vbox1;
	private global::Gtk.MenuBar menuMain;
	private global::Gtk.Toolbar tbMain;
	private global::Gtk.Notebook nbMain;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menuMain'/></ui>");
		this.menuMain = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menuMain")));
		this.menuMain.Name = "menuMain";
		this.vbox1.Add (this.menuMain);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menuMain]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><toolbar name='tbMain'/></ui>");
		this.tbMain = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/tbMain")));
		this.tbMain.Name = "tbMain";
		this.tbMain.ShowArrow = false;
		this.vbox1.Add (this.tbMain);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.tbMain]));
		w3.Position = 1;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.nbMain = new global::Gtk.Notebook ();
		this.nbMain.CanFocus = true;
		this.nbMain.Name = "nbMain";
		this.nbMain.CurrentPage = -1;
		this.vbox1.Add (this.nbMain);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.nbMain]));
		w4.Position = 2;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 1257;
		this.DefaultHeight = 805;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
	}
}
