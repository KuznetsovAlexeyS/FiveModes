using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MapOfModes
{
	class CustomForm : Form
	{
		private MenuStrip menu;
		public ToolStripMenuItem openSettings { get; }
		public ToolStripMenuItem pause { get; }
		public ToolStripMenuItem save { get; }
		public ToolStripMenuItem read { get; }

		public CustomForm()
		{
			this.menu = new MenuStrip
			{
				Dock = DockStyle.Top,
				Stretch = true,
			};

			this.openSettings = new ToolStripMenuItem
			{
				Text = "���������"
			};

			this.pause = new ToolStripMenuItem
			{
				Text = "��������� �����"
			};
			pause.Click += (sender, args) =>
			{
				if (pause.Text == "��������� �����") pause.Text = "����� � �����";
				else pause.Text = "��������� �����";
			};

			this.save = new ToolStripMenuItem
			{
				Text = "��������� �����",
			};

			this.read = new ToolStripMenuItem
			{
				Text = "������� �����",
			};

			menu.Items.Add(openSettings);
			menu.Items.Add(pause);
			menu.Items.Add(save);
			menu.Items.Add(read);

			Controls.Add(menu);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// CustomForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Name = "CustomForm";
			this.ResumeLayout(false);
		}
	}
}