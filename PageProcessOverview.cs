using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Gtk;
using Mono.Posix;
using Mono.Security.X509;
using System.IO;
using GLib;

namespace ProcessMonitor
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PageProcessOverview : Gtk.Bin
	{
		private ListStore lsProcessList = new ListStore(typeof(PerformanceCounter));

//		private Stack<int> sortColumnIds = new AbandonedMutexException();
		private int sortColumnId;
		private SortType sortOrder;
		private TreeViewColumn sortColumn;

		public PageProcessOverview()
		{
			Build();
			Init();
		}

		protected void Init()
		{
			lsProcessList.SortColumnChanged += (object sender, EventArgs e) =>
			{
//				int sortColumnId;
//				SortType sortOrder;
				if (lsProcessList.GetSortColumnId(out sortColumnId, out sortOrder))
				{
					sortColumn = nvProcessList.Columns.FirstOrDefault((tvc) => tvc.SortColumnId == sortColumnId);
					if (sortColumn == null)
						throw new InvalidDataException("Could not retrieve a TreeViewColumn with the specified SortColumnId");
					
				}
			};

			lsProcessList.SetSortFunc(0, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).CategoryName.CompareTo(GetModelPC(b).CategoryName));
			lsProcessList.SetSortFunc(1, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).CounterType.CompareTo(GetModelPC(b).CounterType));
			lsProcessList.SetSortFunc(2, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).CounterName.CompareTo(GetModelPC(b).CounterName));
			lsProcessList.SetSortFunc(3, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).CounterHelp.CompareTo(GetModelPC(b).CounterHelp));
			lsProcessList.SetSortFunc(4, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).InstanceName.CompareTo(GetModelPC(b).InstanceName));
			lsProcessList.SetSortFunc(5, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).RawValue.CompareTo(GetModelPC(b).RawValue));
			lsProcessList.SetSortFunc(6, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).ReadOnly.CompareTo(GetModelPC(b).ReadOnly));
			lsProcessList.SetSortFunc(7, (TreeModel model, TreeIter a, TreeIter b) =>
				(sortOrder == SortType.Ascending ? +1 : +1) * GetModelPC(a).InstanceLifetime.CompareTo(GetModelPC(b).InstanceLifetime));

//			lsProcessList.DefaultSortFunc = (TreeModel model, TreeIter a, TreeIter b) =>
//			{
//				PerformanceCounter pcA = (PerformanceCounter)model.GetValue(a, 0);
//				PerformanceCounter pcB = (PerformanceCounter)model.GetValue(b, 0);
//				if (pcA == null || pcB == null)
//					throw new InvalidDataException("Could not retrieve data from model for sorting");
//				int ret;
//				switch (sortColumn.Title)
//				{
//					case "Category Name": ret = pcA.CategoryName.CompareTo(pcB.CategoryName); break;
//					case "Counter Type": ret = pcA.CounterType.CompareTo(pcB.CounterType); break;
//					case "Counter Name": ret = pcA.CounterName.CompareTo(pcB.CounterName); break;
//					case "Counter Help": ret = pcA.CounterHelp.CompareTo(pcB.CounterHelp); break;
//					case "Instance Name": ret = pcA.InstanceName.CompareTo(pcB.InstanceName); break;
//					case "Raw Value": ret = pcA.RawValue.CompareTo(pcB.RawValue); break;
//					case "ReadOnly": ret = pcA.ReadOnly.CompareTo(pcB.ReadOnly); break;
//					case "Lifetime": ret = pcA.InstanceLifetime.CompareTo(pcB.InstanceLifetime); break;
//					default: throw new InvalidDataException(string.Format("Invalid sort column, with title=\"{0}\"", sortColumn.Title));
//				}
//				return sortOrder == SortType.Ascending ? ret : -ret;
//			};

			foreach (PerformanceCounterCategory pcc in PerformanceCounterCategory.GetCategories())
			{
				foreach (PerformanceCounter pc in pcc.GetCounters())
					lsProcessList.AppendValues(pc);
				foreach (string instanceName in pcc.GetInstanceNames())
					foreach (PerformanceCounter pc in pcc.GetCounters(instanceName))
						lsProcessList.AppendValues(pc);
			}

//			nvProcessList.HeadersClickable = true;
			TreeViewColumn column;
			CellRenderer renderer;
	
			column = new TreeViewColumn("Category Name", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 0 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).CategoryName);
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Counter Type", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 1 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).CounterType.ToString());
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Counter Name", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 2 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).CounterName);
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Counter Help", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 3 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).CounterHelp);
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Instance Name", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 4 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).InstanceName);
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Raw Value", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 5 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).RawValue.ToString());
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("ReadOnly", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 6 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).ReadOnly.ToString());
			nvProcessList.AppendColumn(column);

		column = new TreeViewColumn("Lifetime", renderer = new CellRendererText()) { Clickable = true, Alignment = 0, Resizable = true, SortColumnId = 7 };
			column.SetCellDataFunc(renderer, (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) => ((CellRendererText)cell).Text = ((PerformanceCounter)((ListStore)tree_model).GetValue(iter, 0)).InstanceLifetime.ToString());
			nvProcessList.AppendColumn(column);

			nvProcessList.Model = lsProcessList;
		}
	
		private PerformanceCounter GetModelPC(TreeIter iter)
		{
			return (PerformanceCounter)lsProcessList.GetValue(iter, 0);
		}
	}
}

