using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MapOfModes
{
	class SettingForm : Form
	{
		public ComboBox mode { get; }
		public ComboBox horizontalValue { get; }
		public ComboBox verticalValue { get; }
		private Button cancel;
		public Button accept { get; }
		public TextBox PrStart { get; }
		public TextBox eStart { get; }
		public TextBox rStart { get; }
		public TextBox nuStart { get; }
		public TextBox kStart { get; } 
		public TextBox horizontalValueEnd { get; }
		public TextBox verticalValueEnd { get; }
		public TextBox startModeX { get; }
		public TextBox startModeY { get; }
		public TextBox startModeZ { get; }
		public TextBox startModeV { get; }
		public TextBox startModeW { get; }
		public CheckBox continuationByMode { get; }

		public SettingForm()
		{
			this.Text = "Настройки";
			this.Width = 400;
			this.Height = 300;
			this.ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			this.PrStart = new TextBox
			{
				Location = new Point(110, 65),
				Size = new Size(50, 10),
				Text = "100.0",
			};
			Label PrLabel = new Label
			{
				Location = new Point(5, 65),
				Text = "Число Прандтля Pr",
			};

			this.eStart = new TextBox
			{
				Location = new Point(110, 90),
				Size = new Size(50, 10),
				Text = "67.0",
			};
			Label eLabel = new Label
			{
				Location = new Point(10, 90),
				Text = "Электрическое число Рэлея e",
				Visible = true,
			};

			this.rStart = new TextBox
			{
				Location = new Point(110, 120),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label rLabel = new Label
			{
				Location = new Point(10, 120),
				Text = "Тепловое число Рэлея r",
				Visible = true,
			};

			this.nuStart = new TextBox
			{
				Location = new Point(110, 150),
				Size = new Size(50, 10),
				Text = "0.05",
			};
			Label nuLabel = new Label
			{
				Location = new Point(10, 150),
				Text = "Частота nu",
			};

			this.kStart = new TextBox
			{
				Location = new Point(110, 170),
				Size = new Size(50, 10),
				Text = "0.962",
			};
			Label kLabel = new Label
			{
				Location = new Point(10, 170),
				Text = "Волновое число k",
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
				Location = new Point(30, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "X",
			};

			this.horizontalValue = new ComboBox
			{
				Location = new Point(85, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};

			this.verticalValue = new ComboBox
			{
				Location = new Point(180, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};

			Label modeLabel = new Label
			{
				Location = new Point(35, 15),
				Size = new Size(50, 30),
				Text = "Мода",

			};

			Label horizontalValueLabel = new Label
			{
				Location = new Point(85, 5),
				Size = new Size(95, 30),
				Text = "Горизонтальный параметр",
			};

			Label verticalValueLabel = new Label
			{
				Location = new Point(180, 5),
				Size = new Size(120, 30),
				Text = "Вертикальный параметр",
			};

			Label finalNumbers = new Label
			{
				Location = new Point(190, 78),
				Text = "Конечное значение",
			};

			this.horizontalValueEnd = new TextBox
			{
				Location = new Point(195, 120),
				Size = new Size(50, 10),
				Text = "0.6",
			};
			Label horizontalValueEndLabel = new Label
			{
				Location = new Point(190, 105),
				Text = "по горизонтали:",
			};

			this.verticalValueEnd = new TextBox
			{
				Location = new Point(195, 155),
				Size = new Size(50, 10),
				Text = "70.0",
			};
			Label verticalValueEndLabel = new Label
			{
				Location = new Point(190, 140),
				Text = "по вертикали:",
			};

			Label startModes = new Label
			{
				Location = new Point(293, 45),
				Size = new Size(80, 30),
				Text = "Cтартовые значения мод",
			};

			this.startModeX = new TextBox
			{
				Location = new Point(310, 80),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeXLabel = new Label
			{
				Location = new Point(293, 83),
				Text = "X:",
			};

			this.startModeY = new TextBox
			{
				Location = new Point(310, 105),
				Size = new Size(50, 10),
				Text = "0.5",
			};
			Label startModeYLabel = new Label
			{
				Location = new Point(293, 108),
				Text = "Y:",
			};

			this.startModeZ = new TextBox
			{
				Location = new Point(310, 130),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeZLabel = new Label
			{
				Location = new Point(293, 133),
				Text = "Z:",
			};

			this.startModeV = new TextBox
			{
				Location = new Point(310, 155),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeVLabel = new Label
			{
				Location = new Point(293, 158),
				Text = "V:",
			};

			this.startModeW = new TextBox
			{
				Location = new Point(310, 180),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeWLabel = new Label
			{
				Location = new Point(293, 183),
				Size = new Size(20, 20),
				Text = "W:",
			};

			this.continuationByMode = new CheckBox
			{
				Location = new Point (280, 210),
				Size = new Size(100, 40),
				Text = "Продолжение по параметру",
			};

			mode.Items.AddRange(new string[] { "X", "Y", "Z", "V", "W" });
			mode.SelectedItem = mode.Items[0];
			horizontalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			horizontalValue.SelectedItem = horizontalValue.Items[1];
			verticalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			verticalValue.SelectedItem = verticalValue.Items[2];

			Controls.Add(PrLabel);
			Controls.Add(PrStart);
			Controls.Add(eLabel);
			Controls.Add(eStart);
			Controls.Add(rLabel);
			Controls.Add(rStart);
			Controls.Add(nuLabel);
			Controls.Add(nuStart);
			Controls.Add(kLabel);
			Controls.Add(kStart);
			Controls.Add(accept);
			Controls.Add(cancel);
			Controls.Add(mode);
			Controls.Add(horizontalValue);
			Controls.Add(verticalValue);
			Controls.Add(modeLabel);
			Controls.Add(horizontalValueLabel);
			Controls.Add(verticalValueLabel);
			Controls.Add(horizontalValueEnd);
			Controls.Add(finalNumbers);
			Controls.Add(horizontalValueEndLabel);
			Controls.Add(verticalValueEnd);
			Controls.Add(verticalValueEndLabel);
			Controls.Add(startModes);
			Controls.Add(startModeX);
			Controls.Add(startModeXLabel);
			Controls.Add(startModeY);
			Controls.Add(startModeYLabel);
			Controls.Add(startModeZ);
			Controls.Add(startModeZLabel);
			Controls.Add(startModeV);
			Controls.Add(startModeVLabel);
			Controls.Add(startModeW);
			Controls.Add(startModeWLabel);
			Controls.Add(continuationByMode);
		}
	}
}