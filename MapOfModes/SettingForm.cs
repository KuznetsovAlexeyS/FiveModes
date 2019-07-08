using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MapOfModes
{
	class SettingForm : Form
	{
		public ComboBox mode { set; get; }
		public ComboBox horizontalValue { set; get; }
		public ComboBox verticalValue { set; get; }
		private Button cancel;
		public Button accept { set; get; }
		public TextBox PrStart { get; }
		public TextBox eStart { get; }
		public TextBox rStart { get; }
		public TextBox nuStart { get; }
		public TextBox kStart { get; } 

		public SettingForm()
		{
			this.Text = "Настройки";
			this.Width = 300;
			this.Height = 300;
			this.ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			this.PrStart = new TextBox
			{
				Location = new Point(50, 150),
				Size = new Size(50, 30),
			};

			this.cancel = new Button
			{
				Location = new Point(50, 225),
				Size = new Size(80, 30),
				Text = "Отмена",
				FlatStyle = FlatStyle.Flat,
			};
			cancel.Click += (sender, args) => this.Hide();

			this.accept = new Button
			{
				Location = new Point(140, 220),
				Size = new Size(100, 40),
				Text = "Принять и начать расчёт",
				FlatStyle = FlatStyle.Flat,
			};

			this.mode = new ComboBox
			{
				Location = new Point(40, 50),
				Size = new Size(50, 20),
				Visible = true,
				Text = "X",
			};

			this.horizontalValue = new ComboBox
			{
				Location = new Point(95, 50),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};

			this.verticalValue = new ComboBox
			{
				Location = new Point(190, 50),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};

			Label modeLabel = new Label
			{
				Location = new Point(45, 30),
				Size = new Size(50, 30),
				Visible = true,
				Text = "Мода",

			};

			Label horizontalValueLabel = new Label
			{
				Location = new Point(95, 20),
				Size = new Size(95, 30),
				Visible = true,
				Text = "Горизонтальный параметр",
			};

			Label verticalValueLabel = new Label
			{
				Location = new Point(190, 20),
				Size = new Size(120, 30),
				Visible = true,
				Text = "Вертикальный параметр",
			};

			mode.Items.AddRange(new string[] { "X", "Y", "Z", "V", "W" });
			horizontalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			verticalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });

			Controls.Add(PrStart);
			Controls.Add(accept);
			Controls.Add(cancel);
			Controls.Add(mode);
			Controls.Add(horizontalValue);
			Controls.Add(verticalValue);
			Controls.Add(modeLabel);
			Controls.Add(horizontalValueLabel);
			Controls.Add(verticalValueLabel);
		}
	}
}