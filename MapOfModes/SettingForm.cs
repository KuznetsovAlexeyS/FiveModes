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
		public ComboBox horizontalParameter { get; }
		public ComboBox verticalParameter { get; }
		private Button cancel;
		public Button accept { get; }
		public TextBox PrStart { get; }
		public TextBox eStart { get; }
		public TextBox rStart { get; }
		public TextBox nuStart { get; }
		public TextBox kStart { get; } 
		public TextBox horizontalValueStart { get; }
		public TextBox verticalValueStart { get; }
		public TextBox horizontalValueStep { get; }
		public TextBox verticalValueStep { get; }
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
			this.Width = 500;
			this.Height = 300;
			this.ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			this.PrStart = new TextBox
			{
				Location = new Point(110, 25),
				Size = new Size(50, 10),
				Text = "100.0",
			};
			Label PrLabel = new Label
			{
				Location = new Point(10, 25),
				Text = "Число Прандтля Pr",
			};

			this.eStart = new TextBox
			{
				Location = new Point(110, 65),
				Size = new Size(50, 10),
				Text = "67.0",
			};
			Label eLabel = new Label
			{
				Location = new Point(10, 65),
				Text = "Электрическое число Рэлея e",
				Visible = true,
			};

			this.rStart = new TextBox
			{
				Location = new Point(110, 100),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label rLabel = new Label
			{
				Location = new Point(10, 100),
				Text = "Тепловое число Рэлея r",
				Visible = true,
			};

			this.nuStart = new TextBox
			{
				Location = new Point(110, 135),
				Size = new Size(50, 10),
				Text = "0.05",
			};
			Label nuLabel = new Label
			{
				Location = new Point(10, 135),
				Size = new Size(50, 10),
				Text = "Частота nu",
			};

			this.kStart = new TextBox
			{
				Location = new Point(110, 165),
				Size = new Size(50, 10),
				Text = "0.962",
			};
			Label kLabel = new Label
			{
				Location = new Point(10, 167),
				Text = "Волновое число k",
			};

			this.cancel = new Button
			{
				Location = new Point(10, 225),
				Size = new Size(70, 30),
				Text = "Отмена",
				FlatStyle = FlatStyle.Flat,
			};
			cancel.Click += (sender, args) => this.Hide();

			this.accept = new Button
			{
				Location = new Point(90, 220),
				Size = new Size(90, 40),
				Text = "Принять и начать расчёт",
				FlatStyle = FlatStyle.Flat,
			};

			this.horizontalParameter = new ComboBox
			{
				Location = new Point(85, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};

			this.mode = new ComboBox
			{
				Location = new Point(410, 25),
				Size = new Size(50, 20),
				Visible = true,
				Text = "X",
			};
			Label modeLabel = new Label
			{
				Location = new Point(415, 5),
				Size = new Size(50, 30),
				Text = "Мода",

			};

			this.horizontalParameter = new ComboBox
			{
				Location = new Point(210, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};
			Label horizontalParameterLabel = new Label
			{
				Location = new Point(205, 5),
				Size = new Size(95, 30),
				Text = "Горизонтальный параметр",
			};

			this.verticalParameter = new ComboBox
			{
				Location = new Point(310, 35),
				Size = new Size(50, 20),
				Visible = true,
				Text = "Pr",
			};
			Label verticalParameterLabel = new Label
			{
				Location = new Point(305, 5),
				Size = new Size(120, 30),
				Text = "Вертикальный параметр",
			};

			this.horizontalValueStart = new TextBox
			{
				Location = new Point(210, 107),
				Size = new Size(50, 10),
				Text = "0.05",
			};
			Label horizontalValueStartLabel = new Label
			{
				Location = new Point(205, 65),
				Size = new Size(70, 40),
				Text = "Начальное значение по горизонтали"
			};

			this.horizontalValueStep = new TextBox
			{
				Location = new Point(210, 165),
				Size = new Size(50, 10),
				Text = "0.005",
			};
			Label horizontalValueStepLabel = new Label
			{
				Location = new Point(205, 135),
				Size = new Size(70, 25),
				Text = "шаг по горизонтали"
			};

			this.horizontalValueEnd = new TextBox
			{
				Location = new Point(210, 230),
				Size = new Size(50, 10),
				Text = "0.06",
			};
			Label horizontalValueEndLabel = new Label
			{
				Location = new Point(205, 190),
				Size = new Size(70, 35),
				Text = "конечное значение по горизонтали", // по горизонтали
			};

			this.verticalValueStart = new TextBox
			{
				Location = new Point(310, 107),
				Size = new Size(50, 10),
				Text = "67.0",
			};
			Label verticalValueStartLabel = new Label
			{
				Location = new Point(305, 65),
				Size = new Size(70, 40),
				Text = "Начальное значение по вертикали",
			};
			this.verticalValueStep = new TextBox
			{
				Location = new Point(310, 165),
				Size = new Size(50, 10),
				Text = "0.1",
			};
			Label verticalValueStepLabel = new Label
			{
				Location = new Point(305, 135),
				Size = new Size(70, 25),
				Text = "шаг по вертикали"
			};

			this.verticalValueEnd = new TextBox
			{
				Location = new Point(310, 230),
				Size = new Size(50, 10),
				Text = "70",
			};
			Label verticalValueEndLabel = new Label
			{
				Location = new Point(305, 190),
				Size = new Size(70, 35),
				Text = "конечное значение по вертикали", // по вертикали
			};


			Label startModes = new Label
			{
				Location = new Point(393, 55),
				Size = new Size(80, 30),
				Text = "Cтартовые значения мод",
			};

			this.startModeX = new TextBox
			{
				Location = new Point(410, 90),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeXLabel = new Label
			{
				Location = new Point(393, 93),
				Text = "X:",
			};

			this.startModeY = new TextBox
			{
				Location = new Point(410, 115),
				Size = new Size(50, 10),
				Text = "0.5",
			};
			Label startModeYLabel = new Label
			{
				Location = new Point(393, 118),
				Text = "Y:",
			};

			this.startModeZ = new TextBox
			{
				Location = new Point(410, 140),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeZLabel = new Label
			{
				Location = new Point(393, 143),
				Text = "Z:",
			};

			this.startModeV = new TextBox
			{
				Location = new Point(410, 165),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeVLabel = new Label
			{
				Location = new Point(393, 168),
				Text = "V:",
			};

			this.startModeW = new TextBox
			{
				Location = new Point(410, 190),
				Size = new Size(50, 10),
				Text = "0.0",
			};
			Label startModeWLabel = new Label
			{
				Location = new Point(393, 193),
				Size = new Size(20, 20),
				Text = "W:",
			};

			this.continuationByMode = new CheckBox
			{
				Location = new Point (385, 220),
				Size = new Size(100, 40),
				Text = "Продолжение по параметру",
			};

			mode.Items.AddRange(new string[] { "X", "Y", "Z", "V", "W" });
			mode.SelectedItem = mode.Items[0];
			horizontalParameter.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			horizontalParameter.SelectedItem = horizontalParameter.Items[1];
			verticalParameter.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			verticalParameter.SelectedItem = verticalParameter.Items[2];

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
			Controls.Add(horizontalParameter);
			Controls.Add(verticalParameter);
			Controls.Add(modeLabel);
			Controls.Add(horizontalParameterLabel);
			Controls.Add(verticalParameterLabel);
			Controls.Add(horizontalValueStart);
			Controls.Add(horizontalValueStartLabel);
			Controls.Add(horizontalValueStep);
			Controls.Add(horizontalValueStepLabel);
			Controls.Add(horizontalValueEnd);
			Controls.Add(horizontalValueEndLabel);
			Controls.Add(verticalValueStart);
			Controls.Add(verticalValueStartLabel);
			Controls.Add(verticalValueStep);
			Controls.Add(verticalValueStepLabel);
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