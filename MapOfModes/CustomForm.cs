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
		public ToolStripMenuItem openSettings { set; get; }
		public ToolStripMenuItem pause { set; get; }

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

			//openSettings.DropDownItems.Add("���������");
			pause.Click += (sender, args) =>
			{
				if (pause.Text == "��������� �����") pause.Text = "����� � �����";
				else pause.Text = "��������� �����";
			};
			menu.Items.Add(openSettings);
			menu.Items.Add(pause);

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