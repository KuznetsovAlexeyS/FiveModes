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
		public TextBox horizontalValueEnd { get; }
		public TextBox verticalValueEnd { get; }

		public SettingForm()
		{
			this.Text = "���������";
			this.Width = 300;
			this.Height = 300;
			this.ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;

			this.PrStart = new TextBox
			{
				Location = new Point(110, 65),
				Size = new Size(50, 10),
			};
			Label PrLabel = new Label
			{
				Location = new Point(5, 65),
				Text = "����� �������� Pr",
			};

			this.eStart = new TextBox
			{
				Location = new Point(110, 90),
				Size = new Size(50, 10),
			};
			Label eLabel = new Label
			{
				Location = new Point(10, 90),
				Text = "������������� ����� ����� e",
				Visible = true,
			};

			this.rStart = new TextBox
			{
				Location = new Point(110, 120),
				Size = new Size(50, 10),
			};
			Label rLabel = new Label
			{
				Location = new Point(10, 120),
				Text = "�������� ����� ����� r",
				Visible = true,
			};

			this.nuStart = new TextBox
			{
				Location = new Point(110, 150),
				Size = new Size(50, 10),
			};
			Label nuLabel = new Label
			{
				Location = new Point(10, 150),
				Text = "������� nu",
			};

			this.kStart = new TextBox
			{
				Location = new Point(110, 170),
				Size = new Size(50, 10),
			};
			Label kLabel = new Label
			{
				Location = new Point(10, 170),
				Text = "�������� ����� k",
			};

			this.cancel = new Button
			{
				Location = new Point(50, 225),
				Size = new Size(80, 30),
				Text = "������",
				FlatStyle = FlatStyle.Flat,
			};
			cancel.Click += (sender, args) => this.Hide();

			this.accept = new Button
			{
				Location = new Point(140, 220),
				Size = new Size(100, 40),
				Text = "������� � ������ ������",
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
				Text = "����",

			};

			Label horizontalValueLabel = new Label
			{
				Location = new Point(85, 5),
				Size = new Size(95, 30),
				Text = "�������������� ��������",
			};

			Label verticalValueLabel = new Label
			{
				Location = new Point(180, 5),
				Size = new Size(120, 30),
				Text = "������������ ��������",
			};

			Label finalNumbers = new Label
			{
				Location = new Point(205, 78),
				Text = "�������� ��������",
			};

			this.horizontalValueEnd = new TextBox
			{
				Location = new Point(210, 120),
				Size = new Size(50, 10),
			};
			Label horizontalValueEndLabel = new Label
			{
				Location = new Point(205, 105),
				Text = "�� �����������:",
			};

			this.verticalValueEnd = new TextBox
			{
				Location = new Point(210, 155),
				Size = new Size(50, 10),
			};
			Label verticalValueEndLabel = new Label
			{
				Location = new Point(205, 140),
				Text = "�� ���������:",
			};



			mode.Items.AddRange(new string[] { "X", "Y", "Z", "V", "W" });
			horizontalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });
			verticalValue.Items.AddRange(new string[] { "Pr", "nu", "e", "r", "k" });

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
		}
	}
}