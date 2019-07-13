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
				Text = "Настройки"
			};

			this.pause = new ToolStripMenuItem
			{
				Text = "Поставить паузу"
			};
			pause.Click += (sender, args) =>
			{
				if (pause.Text == "Поставить паузу") pause.Text = "Снять с паузы";
				else pause.Text = "Поставить паузу";
			};

			this.save = new ToolStripMenuItem
			{
				Text = "Сохранить карту",
			};

			this.read = new ToolStripMenuItem
			{
				Text = "Открыть карту",
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